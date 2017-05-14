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
    public class Tbl_CCLController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        // GET: Tbl_CCL
        public ActionResult Index()
        {
            return View(db.Tbl_CCL.ToList());
        }

        // GET: Tbl_CCL/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_CCL tbl_CCL = db.Tbl_CCL.Find(id);
            if (tbl_CCL == null)
            {
                return HttpNotFound();
            }
            return View(tbl_CCL);
        }

        // GET: Tbl_CCL/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tbl_CCL/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,cpf_cnpj_cliente,marca")] Tbl_CCL tbl_CCL)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_CCL.Add(tbl_CCL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_CCL);
        }

        // GET: Tbl_CCL/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_CCL tbl_CCL = db.Tbl_CCL.Find(id);
            if (tbl_CCL == null)
            {
                return HttpNotFound();
            }
            return View(tbl_CCL);
        }

        // POST: Tbl_CCL/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,cpf_cnpj_cliente,marca")] Tbl_CCL tbl_CCL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_CCL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_CCL);
        }

        // GET: Tbl_CCL/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_CCL tbl_CCL = db.Tbl_CCL.Find(id);
            if (tbl_CCL == null)
            {
                return HttpNotFound();
            }
            return View(tbl_CCL);
        }

        // POST: Tbl_CCL/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_CCL tbl_CCL = db.Tbl_CCL.Find(id);
            db.Tbl_CCL.Remove(tbl_CCL);
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
