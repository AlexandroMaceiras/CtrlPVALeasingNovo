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
    public class Tbl_DutController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        // GET: Tbl_Dut
        public ActionResult Index()
        {
            return View(db.Tbl_Dut.ToList());
        }

        // GET: Tbl_Dut/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Dut tbl_Dut = db.Tbl_Dut.Find(id);
            if (tbl_Dut == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Dut);
        }

        // GET: Tbl_Dut/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tbl_Dut/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,renavam,chassi,placa,comVenda,comDUT")] Tbl_Dut tbl_Dut)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Dut.Add(tbl_Dut);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Dut);
        }

        // GET: Tbl_Dut/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Dut tbl_Dut = db.Tbl_Dut.Find(id);
            if (tbl_Dut == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Dut);
        }

        // POST: Tbl_Dut/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,renavam,chassi,placa,comVenda,comDUT")] Tbl_Dut tbl_Dut)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Dut).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Dut);
        }

        // GET: Tbl_Dut/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Dut tbl_Dut = db.Tbl_Dut.Find(id);
            if (tbl_Dut == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Dut);
        }

        // POST: Tbl_Dut/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Dut tbl_Dut = db.Tbl_Dut.Find(id);
            db.Tbl_Dut.Remove(tbl_Dut);
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
