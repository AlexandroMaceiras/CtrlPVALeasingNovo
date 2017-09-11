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
    public class Arm_LiquidadosEAtivos_ContratoController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        // GET: Arm_LiquidadosEAtivos_Contrato
        public ActionResult Index()
        {
            return View(db.Arm_LiquidadosEAtivos_Contrato.ToList());
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
        public ActionResult Create([Bind(Include = "id,contrato,tipo,agencia,dta_inicio_contrato,dta_vecto_contrato,origem,cpf_cnpj_cliente,nome_cliente,ddd_cliente_particular,fone_cliente_particular,rml_cliente_particular,end_cliente,bairro_cliente,cidade_cliente,uf_cliente,cep_cliente,filler,ddd_cliente_cml,fone_cliente_cml,dta_ultimo_pagto,tipo_de_baixa,data_da_baixa,cod_empresa,num_end_cliente,comp_end_cliente,status")] Arm_LiquidadosEAtivos_Contrato arm_LiquidadosEAtivos_Contrato)
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
        public ActionResult Edit([Bind(Include = "id,contrato,tipo,agencia,dta_inicio_contrato,dta_vecto_contrato,origem,cpf_cnpj_cliente,nome_cliente,ddd_cliente_particular,fone_cliente_particular,rml_cliente_particular,end_cliente,bairro_cliente,cidade_cliente,uf_cliente,cep_cliente,filler,ddd_cliente_cml,fone_cliente_cml,dta_ultimo_pagto,tipo_de_baixa,data_da_baixa,cod_empresa,num_end_cliente,comp_end_cliente,status")] Arm_LiquidadosEAtivos_Contrato arm_LiquidadosEAtivos_Contrato)
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








        // GET: Arm_LiquidadosEAtivos_Contrato/Create
        public ActionResult CriarContrato()
        {
            return View();
        }

        // POST: Arm_LiquidadosEAtivos_Contrato/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CriarContrato([Bind(Include = "id,contrato,tipo,agencia,dta_inicio_contrato,dta_vecto_contrato,origem,cpf_cnpj_cliente,nome_cliente,ddd_cliente_particular,fone_cliente_particular,rml_cliente_particular,end_cliente,bairro_cliente,cidade_cliente,uf_cliente,cep_cliente,filler,ddd_cliente_cml,fone_cliente_cml,dta_ultimo_pagto,tipo_de_baixa,data_da_baixa,cod_empresa,num_end_cliente,comp_end_cliente,status")] Arm_LiquidadosEAtivos_Contrato arm_LiquidadosEAtivos_Contrato)
        {
            try
            {

                // Controle de erros do ModelState
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

                if (ModelState.IsValid)
                {
                    var procuraRegistro = db.Arm_LiquidadosEAtivos_Contrato
                    .FirstOrDefault(c => c.contrato == arm_LiquidadosEAtivos_Contrato.contrato);

                    if (procuraRegistro == null)
                    {
                        procuraRegistro.nome_cliente.ToUpper();
                        procuraRegistro.end_cliente.ToUpper();
                        procuraRegistro.comp_end_cliente.ToUpper();
                        procuraRegistro.bairro_cliente.ToUpper();
                        procuraRegistro.cidade_cliente.ToUpper();
                        procuraRegistro.uf_cliente.ToUpper();
                        //procuraRegistro.marcas.ToUpper();

                        db.Arm_LiquidadosEAtivos_Contrato.Add(arm_LiquidadosEAtivos_Contrato);
                        db.SaveChanges();
                        ViewBag.Message = "Incluido com Sucesso!";
                    }
                    else
                    {
                        ViewBag.Message = "Este Contrato já está cadastrado! Caso queira mudar os dados deste contrato vá em Cadastro procure-o e edite o registro.";
                    }
                }
                else
                {
                    ViewBag.Message = "Erro: Algum campo está inválido!";
                }
            }
            catch(Exception e)
            {
                ViewBag.Message = e.Message.ToString();
            }
            return View();
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
