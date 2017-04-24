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
    public class Tbl_DadosDaVendaController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        // GET: Tbl_DadosDaVenda
        public ActionResult Index()
        {
            return View(db.Tbl_DadosDaVenda.ToList());
        }

        // GET: Tbl_DadosDaVenda/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_DadosDaVenda tbl_DadosDaVenda = db.Tbl_DadosDaVenda.Find(id);
            if (tbl_DadosDaVenda == null)
            {
                return HttpNotFound();
            }
            return View(tbl_DadosDaVenda);
        }

        // GET: Tbl_DadosDaVenda/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tbl_DadosDaVenda/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,renavam,chassi,placa,nome_comprador,cpf_cnpj_comprador,rg_comprador,local_comprador,end_comprador,dta_da_compra,valor_da_compra")] Tbl_DadosDaVenda tbl_DadosDaVenda)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_DadosDaVenda.Add(tbl_DadosDaVenda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_DadosDaVenda);
        }

        // GET: Tbl_DadosDaVenda/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_DadosDaVenda tbl_DadosDaVenda = db.Tbl_DadosDaVenda.Find(id);
            if (tbl_DadosDaVenda == null)
            {
                return HttpNotFound();
            }
            return View(tbl_DadosDaVenda);
        }

        // POST: Tbl_DadosDaVenda/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,renavam,chassi,placa,nome_comprador,cpf_cnpj_comprador,rg_comprador,local_comprador,end_comprador,dta_da_compra,valor_da_compra")] Tbl_DadosDaVenda tbl_DadosDaVenda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_DadosDaVenda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_DadosDaVenda);
        }

        // GET: Tbl_DadosDaVenda/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_DadosDaVenda tbl_DadosDaVenda = db.Tbl_DadosDaVenda.Find(id);
            if (tbl_DadosDaVenda == null)
            {
                return HttpNotFound();
            }
            return View(tbl_DadosDaVenda);
        }

        // POST: Tbl_DadosDaVenda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_DadosDaVenda tbl_DadosDaVenda = db.Tbl_DadosDaVenda.Find(id);
            db.Tbl_DadosDaVenda.Remove(tbl_DadosDaVenda);
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
