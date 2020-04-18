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
    public class DataBaseTypesController : Controller
    {
        private DBAdminEntities db = new DBAdminEntities();

        // GET: DataBaseTypes
        public ActionResult Index()
        {
            return View(db.DataBaseTypes.ToList());
        }

        // GET: DataBaseTypes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBaseType dataBaseType = db.DataBaseTypes.Find(id);
            if (dataBaseType == null)
            {
                return HttpNotFound();
            }
            return View(dataBaseType);
        }

        // GET: DataBaseTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DataBaseTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DataBaseTypes,ID")] DataBaseType dataBaseType)
        {
            if (ModelState.IsValid)
            {
                db.DataBaseTypes.Add(dataBaseType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dataBaseType);
        }

        // GET: DataBaseTypes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBaseType dataBaseType = db.DataBaseTypes.Find(id);
            if (dataBaseType == null)
            {
                return HttpNotFound();
            }
            return View(dataBaseType);
        }

        // POST: DataBaseTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DataBaseTypes,ID")] DataBaseType dataBaseType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dataBaseType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dataBaseType);
        }

        // GET: DataBaseTypes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataBaseType dataBaseType = db.DataBaseTypes.Find(id);
            if (dataBaseType == null)
            {
                return HttpNotFound();
            }
            return View(dataBaseType);
        }

        // POST: DataBaseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DataBaseType dataBaseType = db.DataBaseTypes.Find(id);
            db.DataBaseTypes.Remove(dataBaseType);
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
