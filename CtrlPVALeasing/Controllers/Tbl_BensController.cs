using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CtrlPVALeasing.Models;

namespace CtrlPVALeasing.Controllers
{
    public class Tbl_BensController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        // GET: Tbl_Bens
        public ActionResult Index()
        {
            return View(db.Tbl_Bens.ToList());
        }

        // GET: Tbl_Bens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Bens tbl_Bens = db.Tbl_Bens.Find(id);
            if (tbl_Bens == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Bens);
        }

        // GET: Tbl_Bens/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tbl_Bens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,renavam,chassi,placa")] Tbl_Bens tbl_Bens)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Bens.Add(tbl_Bens);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Bens);
        }

        // GET: Tbl_Bens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Bens tbl_Bens = db.Tbl_Bens.Find(id);
            if (tbl_Bens == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Bens);
        }

        // POST: Tbl_Bens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,renavam,chassi,placa")] Tbl_Bens tbl_Bens)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Bens).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Bens);
        }

        // GET: Tbl_Bens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Bens tbl_Bens = db.Tbl_Bens.Find(id);
            if (tbl_Bens == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Bens);
        }

        // POST: Tbl_Bens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Bens tbl_Bens = db.Tbl_Bens.Find(id);
            db.Tbl_Bens.Remove(tbl_Bens);
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
