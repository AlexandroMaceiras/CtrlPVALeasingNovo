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
    public class Tbl_SituacaoController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        // GET: Tbl_Situacao
        public ActionResult Index()
        {
            return View(db.Tbl_Situacao.ToList());
        }

        // GET: Tbl_Situacao/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Situacao tbl_Situacao = db.Tbl_Situacao.Find(id);
            if (tbl_Situacao == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Situacao);
        }

        // GET: Tbl_Situacao/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tbl_Situacao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,contrato,ativa,localizada,outrasOper")] Tbl_Situacao tbl_Situacao)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Situacao.Add(tbl_Situacao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Situacao);
        }

        // GET: Tbl_Situacao/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Situacao tbl_Situacao = db.Tbl_Situacao.Find(id);
            if (tbl_Situacao == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Situacao);
        }

        // POST: Tbl_Situacao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,contrato,ativa,localizada,outrasOper")] Tbl_Situacao tbl_Situacao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Situacao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Situacao);
        }

        // GET: Tbl_Situacao/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Situacao tbl_Situacao = db.Tbl_Situacao.Find(id);
            if (tbl_Situacao == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Situacao);
        }

        // POST: Tbl_Situacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Situacao tbl_Situacao = db.Tbl_Situacao.Find(id);
            db.Tbl_Situacao.Remove(tbl_Situacao);
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
