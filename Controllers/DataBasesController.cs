using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DBAdmin;

namespace DBAdmin.Controllers
{
    public class DataBasesController : Controller
    {
        private DBAdminEntities db = new DBAdminEntities();

        // GET: DataBases
        public ActionResult Index()
        {
            return View(db.DataBases.ToList());
        }

        // GET: DataBases/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBase dataBase = db.DataBases.Find(id);
            if (dataBase == null)
            {
                return HttpNotFound();
            }
            return View(dataBase);
        }

        // GET: DataBases/Create
        public ActionResult Create()
        {
            


            ViewBag.ServerName = new SelectList(db.Servers, "ServerName", "ServerName");
            return View();
        }

        // POST: DataBases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DBName,ServerName")] DataBase dataBase)
        {
                       
            if (ModelState.IsValid)
            {
                Server server = db.Servers.ToList().FirstOrDefault(m => m.ServerName == dataBase.ServerName);
                var isCreated = Shared.SharedLib.CreateDataBase(server, dataBase.DBName);
                db.DataBases.Add(dataBase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dataBase);
        }

        // GET: DataBases/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBase dataBase = db.DataBases.Find(id);
            if (dataBase == null)
            {
                return HttpNotFound();
            }
            return View(dataBase);
        }

        // POST: DataBases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DBName,ServerName")] DataBase dataBase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dataBase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dataBase);
        }

        // GET: DataBases/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBase dataBase = db.DataBases.Find(id);
            if (dataBase == null)
            {
                return HttpNotFound();
            }
            return View(dataBase);
        }

        // POST: DataBases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            DataBase dataBase = db.DataBases.Find(id);
            db.DataBases.Remove(dataBase);
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
    }
}
