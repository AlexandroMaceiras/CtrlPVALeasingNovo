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
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        IEnumerable<ContratosVeiculosViewModel> model = null;

        // GET: Arm_LiquidadosEAtivos_Contrato
        public ActionResult Index()
        {

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato
                     select new
                     {
                         contrato   = a.contrato,
                         tipo = a.tipo,
                         agencia = a.agencia,
                         dta_inicio_contrato = a.dta_inicio_contrato,
                         dta_vecto_contrato = a.dta_vecto_contrato,
                         origem = a.origem,
                         cpf_cnpj_cliente = a.cpf_cnpj_cliente,
                         nome_cliente = a.nome_cliente,
                         ddd_cliente_particular = a.ddd_cliente_particular,
                         fone_cliente_particular = a.fone_cliente_particular,
                         rml_cliente_particular = a.rml_cliente_particular,
                         end_cliente = a.end_cliente,
                         bairro_cliente = a.bairro_cliente,
                         cidade_cliente = a.cidade_cliente,
                         uf_cliente = a.uf_cliente,
                         cep_cliente = a.cep_cliente,
                         filler = a.filler,
                         ddd_cliente_cml = a.ddd_cliente_cml,
                         fone_cliente_cml = a.fone_cliente_cml,
                         dta_ultimo_pagto = a.dta_ultimo_pagto,
                         tipo_de_baixa = a.tipo_de_baixa,
                         data_da_baixa = a.data_da_baixa,
                         cod_empresa = a.cod_empresa,
                         num_end_cliente = a.num_end_cliente,
                         comp_end_cliente = a.comp_end_cliente,
                         //status = a.status,

                         contrato_v = b.contrato,
                         tipo_registro = b.tipo_registro,
                         marca = b.marca,
                         modelo = b. modelo,
                         tipo_v = b.tipo,
                         ano_fab = b.ano_fab,
                         ano_mod = b.ano_mod,
                         cor = b.cor,
                         renavam = b.renavam,
                         chassi = b.chassi,
                         placa = b.placa,
                         origem_v = b.origem
                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         contrato = x.contrato,
                         tipo = x.tipo,
                         agencia = x.agencia,
                         dta_inicio_contrato = x.dta_inicio_contrato,
                         dta_vecto_contrato = x.dta_vecto_contrato,
                         origem = x.origem,
                         cpf_cnpj_cliente = x.cpf_cnpj_cliente,
                         nome_cliente = x.nome_cliente,
                         ddd_cliente_particular = x.ddd_cliente_particular,
                         fone_cliente_particular = x.fone_cliente_particular,
                         rml_cliente_particular = x.rml_cliente_particular,
                         end_cliente = x.end_cliente,
                         bairro_cliente = x.bairro_cliente,
                         cidade_cliente = x.cidade_cliente,
                         uf_cliente = x.uf_cliente,
                         cep_cliente = x.cep_cliente,
                         filler = x.filler,
                         ddd_cliente_cml = x.ddd_cliente_cml,
                         fone_cliente_cml = x.fone_cliente_cml,
                         dta_ultimo_pagto = x.dta_ultimo_pagto,
                         tipo_de_baixa = x.tipo_de_baixa,
                         data_da_baixa = x.data_da_baixa,
                         cod_empresa = x.cod_empresa,
                         num_end_cliente = x.num_end_cliente,
                         comp_end_cliente = x.comp_end_cliente,
                         //status = x.status,

                         contrato_v = x.contrato,
                         tipo_registro = x.tipo_registro,
                         marca = x.marca,
                         modelo = x.modelo,
                         tipo_v = x.tipo,
                         ano_fab = x.ano_fab,
                         ano_mod = x.ano_mod,
                         cor = x.cor,
                         renavam = x.renavam,
                         chassi = x.chassi,
                         placa = x.placa,
                         origem_v = x.origem

                     }).Take(100).OrderByDescending(x => x.contrato).ToList();

            //return View(db.Arm_LiquidadosEAtivos_Contrato.Take(5).ToList());
            if (model.Count() == 0 || model == null)
            {
                return View();
            }
            return View(model);
        }

        public ActionResult ConsultaContrato()
        {
            return View(GetContratosVeiculosViewModel());
        }

        /// <summary>
        /// Cria um IEnumerable do modelo ContratosVeiculosViewModel vazio para se injetar na view pela primeira vez quando ela carrega sem ninguém.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ContratosVeiculosViewModel> GetContratosVeiculosViewModel()
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();
            model.Add(new ContratosVeiculosViewModel(){ id = 0, agencia = "" });
            return model;
        }

        /// <summary>
        /// Cria um IEnumerable do modelo ContratosVeiculosViewModel vazio para se injetar na view pela primeira vez quando ela carrega sem ninguém.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ContratosVeiculosViewModel> GetContratosVeiculosViewModel2()
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();
            model.Add(new ContratosVeiculosViewModel() { id = 0, agencia = " " });
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

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
            join b in db.Arm_Veiculos
            on a.contrato equals b.contrato
            where a.contrato.Equals(contrato)
            where a.origem.Equals("B")
            where b.origem.Equals("RECIBO VEN")
            select new
            {
                id = a.id,
                contrato   = a.contrato,
                tipo = a.tipo,
                agencia = a.agencia,
                dta_inicio_contrato = a.dta_inicio_contrato,
                dta_vecto_contrato = a.dta_vecto_contrato,
                origem = a.origem,
                cpf_cnpj_cliente = a.cpf_cnpj_cliente,
                nome_cliente = a.nome_cliente,
                ddd_cliente_particular = a.ddd_cliente_particular,
                fone_cliente_particular = a.fone_cliente_particular,
                rml_cliente_particular = a.rml_cliente_particular,
                end_cliente = a.end_cliente,
                bairro_cliente = a.bairro_cliente,
                cidade_cliente = a.cidade_cliente,
                uf_cliente = a.uf_cliente,
                cep_cliente = a.cep_cliente,
                filler = a.filler,
                ddd_cliente_cml = a.ddd_cliente_cml,
                fone_cliente_cml = a.fone_cliente_cml,
                dta_ultimo_pagto = a.dta_ultimo_pagto,
                tipo_de_baixa = a.tipo_de_baixa,
                data_da_baixa = a.data_da_baixa,
                cod_empresa = a.cod_empresa,
                num_end_cliente = a.num_end_cliente,
                comp_end_cliente = a.comp_end_cliente,
                status = a.status,

                contrato_v = b.contrato,
                tipo_registro = b.tipo_registro,
                marca = b.marca,
                modelo = b. modelo,
                tipo_v = b.tipo,
                ano_fab = b.ano_fab,
                ano_mod = b.ano_mod,
                cor = b.cor,
                renavam = b.renavam,
                chassi = b.chassi,
                placa = b.placa,
                origem_v = b.origem//,
                //status_v = b.status
            }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
            {
                id = x.id,
                contrato = x.contrato,
                tipo = x.tipo,
                agencia = x.agencia,
                dta_inicio_contrato = x.dta_inicio_contrato,
                dta_vecto_contrato = x.dta_vecto_contrato,
                origem = x.origem,
                cpf_cnpj_cliente = x.cpf_cnpj_cliente,
                nome_cliente = x.nome_cliente,
                ddd_cliente_particular = x.ddd_cliente_particular,
                fone_cliente_particular = x.fone_cliente_particular,
                rml_cliente_particular = x.rml_cliente_particular,
                end_cliente = x.end_cliente,
                bairro_cliente = x.bairro_cliente,
                cidade_cliente = x.cidade_cliente,
                uf_cliente = x.uf_cliente,
                cep_cliente = x.cep_cliente,
                filler = x.filler,
                ddd_cliente_cml = x.ddd_cliente_cml,
                fone_cliente_cml = x.fone_cliente_cml,
                dta_ultimo_pagto = x.dta_ultimo_pagto,
                tipo_de_baixa = x.tipo_de_baixa,
                data_da_baixa = x.data_da_baixa,
                cod_empresa = x.cod_empresa,
                num_end_cliente = x.num_end_cliente,
                comp_end_cliente = x.comp_end_cliente,
                status = x.status,

                contrato_v = x.contrato,
                tipo_registro = x.tipo_registro,
                marca = x.marca,
                modelo = x.modelo,
                tipo_v = x.tipo,
                ano_fab = x.ano_fab,
                ano_mod = x.ano_mod,
                cor = x.cor,
                renavam = x.renavam,
                chassi = x.chassi,
                placa = x.placa,
                origem_v = x.origem_v//,
                //status_v = x.status_v

            }).ToList();

            //model = model.Where(s => s.contrato.Equals(contrato));
            //model = model.Where(b => b.origem.Equals("B"));
            //model = model.Where(w => w.origem_v.Contains("RECIBO VEN"));

            //return View(db.Arm_LiquidadosEAtivos_Contrato.Take(5).ToList());

            if (model.Count() == 0 || model == null)
            {
                return View(GetContratosVeiculosViewModel2()); //RedirectToAction("ConsultaContrato");
            }

            //IEnumerable<Arm_LiquidadosEAtivos_Contrato> arm_LiquidadosEAtivos_Contrato = db.Arm_LiquidadosEAtivos_Contrato.Where(x => x.contrato.Equals(contrato));

            if (model == null || model.Any() == false)
            {
                //return HttpNotFound();
                return RedirectToAction("ConsultaContrato");
            }
            return View(model);

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
