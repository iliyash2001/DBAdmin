using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DBAdmin;
using Microsoft.AspNet.Identity;

namespace DBAdmin.Controllers
{
    [Authorize]



    public class ServerController1 : Controller
    {
        private SqlConnection connection;

        private DBAdminEntities db = new DBAdminEntities();

        // GET: Server
        public ActionResult Index()
        {
            return View(db.ConnectionStrings.ToList());
        }

        // GET: Server/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConnectionString connectionString = db.ConnectionStrings.Find(id);
            if (connectionString == null)
            {
                return HttpNotFound();
            }
            return View(connectionString);
        }

        // GET: Server/Create
        public ActionResult Create()
        {
            //ConnectionString con = new ConnectionString
            //{
            //    AppUserId = User.Identity.GetUserName()
            //};

            return View();
        }

        // POST: Server/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,ServerName,UserId,Password,AppUserId")] ConnectionString connectionString)
        {
            if (ModelState.IsValid)
            {
                ConnectionState validateConnection = CheckConnection(connectionString);
                if (validateConnection == ConnectionState.Open)
                {
                    connectionString.AppUserId = User.Identity.GetUserName();
                    db.ConnectionStrings.Add(connectionString);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Connection can not be established.");

                    return View("Create", connectionString);
                }
            }

            return View(connectionString);
        }

        // GET: Server/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConnectionString connectionString = db.ConnectionStrings.Find(id);
            if (connectionString == null)
            {
                return HttpNotFound();
            }
            return View(connectionString);
        }

        // POST: Server/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,CoonectionString,UserId,Password,AppUserId")] ConnectionString connectionString)
        {
            if (ModelState.IsValid)
            {
                db.Entry(connectionString).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(connectionString);
        }

        // GET: Server/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConnectionString connectionString = db.ConnectionStrings.Find(id);
            if (connectionString == null)
            {
                return HttpNotFound();
            }
            return View(connectionString);
        }

        // POST: Server/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            ConnectionString connectionString = db.ConnectionStrings.Find(id);
            db.ConnectionStrings.Remove(connectionString);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ConnectionState CheckConnection(ConnectionString connectionString)

        {
            connection = new SqlConnection("Data Source='" + connectionString.ServerName + "';Initial Catalog=DBAdmin;User ID='" + connectionString.UserId + "';Password='" + connectionString.Password + "'");
            

            try

            {
                connection.Open();
            }

            catch (Exception)

            {
              
               
            }

            return connection.State;

        }

    }
}
