using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CtrlPVALeasing.Models
{
    public class Tbl_AgenciasController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        // GET: Tbl_Agencias
        public ActionResult Index()
        {
            return View(db.Tbl_Agencias.ToList());
        }

        // GET: Tbl_Agencias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Agencias tbl_Agencias = db.Tbl_Agencias.Find(id);
            if (tbl_Agencias == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Agencias);
        }

        // GET: Tbl_Agencias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tbl_Agencias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,agencia,descricao_agencia")] Tbl_Agencias tbl_Agencias)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Agencias.Add(tbl_Agencias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Agencias);
        }

        // GET: Tbl_Agencias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Agencias tbl_Agencias = db.Tbl_Agencias.Find(id);
            if (tbl_Agencias == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Agencias);
        }

        // POST: Tbl_Agencias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,agencia,descricao_agencia")] Tbl_Agencias tbl_Agencias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Agencias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Agencias);
        }

        // GET: Tbl_Agencias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Agencias tbl_Agencias = db.Tbl_Agencias.Find(id);
            if (tbl_Agencias == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Agencias);
        }

        // POST: Tbl_Agencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Agencias tbl_Agencias = db.Tbl_Agencias.Find(id);
            db.Tbl_Agencias.Remove(tbl_Agencias);
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
