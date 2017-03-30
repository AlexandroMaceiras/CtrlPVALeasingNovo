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
    public class ConsultaController : Controller
    {
        private CtrlPVALeasingContext db = new CtrlPVALeasingContext();

        // GET: Arm_LiquidadosEAtivos_Contrato
        public ActionResult Index()
        {
            return View(db.Arm_LiquidadosEAtivos_Contrato.ToList());
        }

        public ActionResult ConsultaContrato()
        {
            return View(GetArm_LiquidadosEAtivos_Contrato());
        }

        /// <summary>
        /// Cria um IEnumerable do modelo Arm_LiquidadosEAtivos_Contrato vazio para se injetar na view pela primeira vez quando ela carrega sem ninguém.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Arm_LiquidadosEAtivos_Contrato> GetArm_LiquidadosEAtivos_Contrato()
        {
            List<Arm_LiquidadosEAtivos_Contrato> model = new List<Arm_LiquidadosEAtivos_Contrato>();
            model.Add(new Arm_LiquidadosEAtivos_Contrato()); // { id = 0, agencia = "" });
            return model;
        }



        [Authorize, ActionName("ConsultaContrato"), HttpPost]
        public ActionResult ConsultaContrato2(string contrato)
        {
            //var model =
            //db.Arm_LiquidadosEAtivos_Contrato.OrderBy(x => x.contrato)
            //.Where(x => x.contrato.Contains(contrato));

            if (contrato == "" || contrato == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("ConsultaContrato");
            }
            IEnumerable<Arm_LiquidadosEAtivos_Contrato> arm_LiquidadosEAtivos_Contrato = db.Arm_LiquidadosEAtivos_Contrato.Where(x => x.contrato.Equals(contrato));
            if (arm_LiquidadosEAtivos_Contrato == null || arm_LiquidadosEAtivos_Contrato.Any() == false)
            {
                //return HttpNotFound();
                return RedirectToAction("ConsultaContrato");
            }
            return View(arm_LiquidadosEAtivos_Contrato);

            //if (contrato != "")
            //{
            //    return View("ConsultaContrato", model);
            //}
            //return RedirectToAction("ConsultaContrato");
        }


        // GET: Arm_LiquidadosEAtivos_Contrato/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arm_LiquidadosEAtivos_Contrato arm_LiquidadosEAtivos_Contrato = db.Arm_LiquidadosEAtivos_Contrato.Find(id);
            if (arm_LiquidadosEAtivos_Contrato == null)
            {
                return HttpNotFound();
            }
            return View(arm_LiquidadosEAtivos_Contrato);
        }

        // GET: Arm_LiquidadosEAtivos_Contrato/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Arm_LiquidadosEAtivos_Contrato/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,contrato,tipo,agencia,dta_inicio_contrato,dta_vecto_contrato,origem,cpf_cnpj_cliente,nome_cliente,ddd_cliente_particular,fone_cliente_particular,rml_cliente_particular,end_cliente,bairro_cliente,cidade_cliente,uf_cliente,cep_cliente,filler,ddd_cliente_cml,fone_cliente_cml,dta_ultimo_pagto,tipo_de_baixa,data_da_baixa,cod_empresa,num_end_cliente,comp_end_cliente")] Arm_LiquidadosEAtivos_Contrato arm_LiquidadosEAtivos_Contrato)
        {
            if (ModelState.IsValid)
            {
                db.Arm_LiquidadosEAtivos_Contrato.Add(arm_LiquidadosEAtivos_Contrato);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(arm_LiquidadosEAtivos_Contrato);
        }

        // GET: Arm_LiquidadosEAtivos_Contrato/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arm_LiquidadosEAtivos_Contrato arm_LiquidadosEAtivos_Contrato = db.Arm_LiquidadosEAtivos_Contrato.Find(id);
            if (arm_LiquidadosEAtivos_Contrato == null)
            {
                return HttpNotFound();
            }
            return View(arm_LiquidadosEAtivos_Contrato);
        }

        // POST: Arm_LiquidadosEAtivos_Contrato/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,contrato,tipo,agencia,dta_inicio_contrato,dta_vecto_contrato,origem,cpf_cnpj_cliente,nome_cliente,ddd_cliente_particular,fone_cliente_particular,rml_cliente_particular,end_cliente,bairro_cliente,cidade_cliente,uf_cliente,cep_cliente,filler,ddd_cliente_cml,fone_cliente_cml,dta_ultimo_pagto,tipo_de_baixa,data_da_baixa,cod_empresa,num_end_cliente,comp_end_cliente")] Arm_LiquidadosEAtivos_Contrato arm_LiquidadosEAtivos_Contrato)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arm_LiquidadosEAtivos_Contrato).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(arm_LiquidadosEAtivos_Contrato);
        }

        // GET: Arm_LiquidadosEAtivos_Contrato/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arm_LiquidadosEAtivos_Contrato arm_LiquidadosEAtivos_Contrato = db.Arm_LiquidadosEAtivos_Contrato.Find(id);
            if (arm_LiquidadosEAtivos_Contrato == null)
            {
                return HttpNotFound();
            }
            return View(arm_LiquidadosEAtivos_Contrato);
        }

        // POST: Arm_LiquidadosEAtivos_Contrato/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Arm_LiquidadosEAtivos_Contrato arm_LiquidadosEAtivos_Contrato = db.Arm_LiquidadosEAtivos_Contrato.Find(id);
            db.Arm_LiquidadosEAtivos_Contrato.Remove(arm_LiquidadosEAtivos_Contrato);
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
