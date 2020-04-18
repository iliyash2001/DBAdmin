using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DBAdmin;
using DBAdmin.Shared;
using Microsoft.AspNet.Identity;

namespace DBAdmin.Controllers
{
    public class ServersController : Controller
    {
        private DBAdminEntities db = new DBAdminEntities();

        // GET: Servers
        public ActionResult Index()
        {
            var servers = db.Servers.Include(s => s.DataBaseType);
            return View(servers.ToList());
        }

        // GET: Servers/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Server server = db.Servers.Find(id);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        // GET: Servers/Create
        public ActionResult Create()
        {
            ViewBag.DBType = new SelectList(db.DataBaseTypes, "DataBaseTypes", "DataBaseTypes");
            return View();
        }

        // POST: Servers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,ServerName,UserId,Password,AppUserId,DBType")] Server server)
        {

            ConnectionState validateConnection = SharedLib.CheckConnection(server);
            if (validateConnection == ConnectionState.Open)
            {
                server.AppUserId = User.Identity.GetUserName();
                db.Servers.Add(server);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Connection can not be established.");
                ViewBag.DBType = new SelectList(db.DataBaseTypes, "DataBaseTypes", "DataBaseTypes", server.DBType);
                return View(server);
            }

            if (ModelState.IsValid)
            {
                db.Servers.Add(server);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DBType = new SelectList(db.DataBaseTypes, "DataBaseTypes", "DataBaseTypes", server.DBType);
            return View(server);
        }

        // GET: Servers/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Server server = db.Servers.Find(id);
            if (server == null)
            {
                return HttpNotFound();
            }
            ViewBag.DBType = new SelectList(db.DataBaseTypes, "DataBaseTypes", "DataBaseTypes", server.DBType);
            return View(server);
        }

        // POST: Servers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,ServerName,UserId,Password,AppUserId,DBType")] Server server)
        {
            if (ModelState.IsValid)
            {
                db.Entry(server).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DBType = new SelectList(db.DataBaseTypes, "DataBaseTypes", "DataBaseTypes", server.DBType);
            return View(server);
        }

        // GET: Servers/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Server server = db.Servers.Find(id);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        // POST: Servers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Server server = db.Servers.Find(id);
            db.Servers.Remove(server);
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
