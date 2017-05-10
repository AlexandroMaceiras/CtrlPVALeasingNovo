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
    public class Tbl_SCCController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        // GET: Tbl_SCC
        public ActionResult Index()
        {
            return View(db.Tbl_SCC.ToList());
        }

        // GET: Tbl_SCC/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_SCC tbl_SCC = db.Tbl_SCC.Find(id);
            if (tbl_SCC == null)
            {
                return HttpNotFound();
            }
            return View(tbl_SCC);
        }

        // GET: Tbl_SCC/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tbl_SCC/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,cpf_cnpj_cliente")] Tbl_SCC tbl_SCC)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_SCC.Add(tbl_SCC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_SCC);
        }

        // GET: Tbl_SCC/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_SCC tbl_SCC = db.Tbl_SCC.Find(id);
            if (tbl_SCC == null)
            {
                return HttpNotFound();
            }
            return View(tbl_SCC);
        }

        // POST: Tbl_SCC/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,cpf_cnpj_cliente")] Tbl_SCC tbl_SCC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_SCC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_SCC);
        }

        // GET: Tbl_SCC/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_SCC tbl_SCC = db.Tbl_SCC.Find(id);
            if (tbl_SCC == null)
            {
                return HttpNotFound();
            }
            return View(tbl_SCC);
        }

        // POST: Tbl_SCC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_SCC tbl_SCC = db.Tbl_SCC.Find(id);
            db.Tbl_SCC.Remove(tbl_SCC);
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
