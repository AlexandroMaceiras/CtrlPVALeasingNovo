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
    public class Tbl_DebitosEPagamentos_VeiculoController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        // GET: Tbl_DebitosEPagamentos_Veiculo
        public ActionResult Index()
        {
            return View(db.Tbl_DebitosEPagamentos_Veiculo.ToList());
        }

        // GET: Tbl_DebitosEPagamentos_Veiculo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_DebitosEPagamentos_Veiculo tbl_DebitosEPagamentos_Veiculo = db.Tbl_DebitosEPagamentos_Veiculo.Find(id);
            if (tbl_DebitosEPagamentos_Veiculo == null)
            {
                return HttpNotFound();
            }
            return View(tbl_DebitosEPagamentos_Veiculo);
        }

        // GET: Tbl_DebitosEPagamentos_Veiculo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tbl_DebitosEPagamentos_Veiculo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,chassi,renavam,placa,dta_cobranca,uf_cobranca,tipo_cobranca,valor_divida,ano_exercicio,cda,valor_custas,debito_protesto,nome_cartorio,divida_ativa_serasa,protesto_serasa,valor_debito_total,pagamento_efet_banco,dta_pagamento_divida,uf_pagamento,forma_pagamento_divida,forma_pagamento_custas,valor_pago_divida,numero_miro_divida,numero_miro_custa,obs_pagamento,valor_pago_custas,valor_pago_total,valor_recuperado,valor_total_recuperado,dta_pagamento_custas,dta_recuperacao,pci_debito_divida,pci_debito_custa,pci_credito,grupo_safra")] Tbl_DebitosEPagamentos_Veiculo tbl_DebitosEPagamentos_Veiculo)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_DebitosEPagamentos_Veiculo.Add(tbl_DebitosEPagamentos_Veiculo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_DebitosEPagamentos_Veiculo);
        }

        // GET: Tbl_DebitosEPagamentos_Veiculo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_DebitosEPagamentos_Veiculo tbl_DebitosEPagamentos_Veiculo = db.Tbl_DebitosEPagamentos_Veiculo.Find(id);
            if (tbl_DebitosEPagamentos_Veiculo == null)
            {
                return HttpNotFound();
            }
            return View(tbl_DebitosEPagamentos_Veiculo);
        }

        // POST: Tbl_DebitosEPagamentos_Veiculo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,chassi,renavam,placa,dta_cobranca,uf_cobranca,tipo_cobranca,valor_divida,ano_exercicio,cda,valor_custas,debito_protesto,nome_cartorio,divida_ativa_serasa,protesto_serasa,valor_debito_total,pagamento_efet_banco,dta_pagamento_divida,uf_pagamento,forma_pagamento_divida,forma_pagamento_custas,valor_pago_divida,numero_miro_divida,numero_miro_custa,obs_pagamento,valor_pago_custas,valor_pago_total,valor_recuperado,valor_total_recuperado,dta_pagamento_custas,dta_recuperacao,pci_debito_divida,pci_debito_custa,pci_credito,grupo_safra")] Tbl_DebitosEPagamentos_Veiculo tbl_DebitosEPagamentos_Veiculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_DebitosEPagamentos_Veiculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_DebitosEPagamentos_Veiculo);
        }

        // GET: Tbl_DebitosEPagamentos_Veiculo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_DebitosEPagamentos_Veiculo tbl_DebitosEPagamentos_Veiculo = db.Tbl_DebitosEPagamentos_Veiculo.Find(id);
            if (tbl_DebitosEPagamentos_Veiculo == null)
            {
                return HttpNotFound();
            }
            return View(tbl_DebitosEPagamentos_Veiculo);
        }

        // POST: Tbl_DebitosEPagamentos_Veiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_DebitosEPagamentos_Veiculo tbl_DebitosEPagamentos_Veiculo = db.Tbl_DebitosEPagamentos_Veiculo.Find(id);
            db.Tbl_DebitosEPagamentos_Veiculo.Remove(tbl_DebitosEPagamentos_Veiculo);
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
