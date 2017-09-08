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
    public class Arm_VeiculosController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        // GET: Arm_Veiculos
        public ActionResult Index()
        {
            return View(db.Arm_Veiculos.ToList());
        }

        // GET: Arm_Veiculos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arm_Veiculos arm_Veiculos = db.Arm_Veiculos.Find(id);
            if (arm_Veiculos == null)
            {
                return HttpNotFound();
            }
            return View(arm_Veiculos);
        }

        // GET: Arm_Veiculos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Arm_Veiculos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,contrato,tipo_registro,marca,modelo,tipo,ano_fab,ano_mod,cor,renavam,chassi,placa,origem,status,comunicado_venda")] Arm_Veiculos arm_Veiculos)
        {
            if (ModelState.IsValid)
            {
                db.Arm_Veiculos.Add(arm_Veiculos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(arm_Veiculos);
        }

        // GET: Arm_Veiculos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arm_Veiculos arm_Veiculos = db.Arm_Veiculos.Find(id);
            if (arm_Veiculos == null)
            {
                return HttpNotFound();
            }
            return View(arm_Veiculos);
        }

        // POST: Arm_Veiculos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,contrato,tipo_registro,marca,modelo,tipo,ano_fab,ano_mod,cor,renavam,chassi,placa,origem,status,comunicado_venda")] Arm_Veiculos arm_Veiculos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arm_Veiculos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(arm_Veiculos);
        }

        // GET: Arm_Veiculos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arm_Veiculos arm_Veiculos = db.Arm_Veiculos.Find(id);
            if (arm_Veiculos == null)
            {
                return HttpNotFound();
            }
            return View(arm_Veiculos);
        }

        // POST: Arm_Veiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Arm_Veiculos arm_Veiculos = db.Arm_Veiculos.Find(id);
            db.Arm_Veiculos.Remove(arm_Veiculos);
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
