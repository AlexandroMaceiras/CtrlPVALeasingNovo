﻿using System;
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

        /// <summary>
        /// Cria um IEnumerable do modelo ContratosVeiculosViewModel vazio para se injetar na view pela primeira vez quando ela carrega sem ninguém.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ContratosVeiculosViewModel> GetContratosVeiculosViewModelPrimeira()
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();
            model.Add(new ContratosVeiculosViewModel(){ id = 0});
            return model;
        }

        /// <summary>
        /// Cria um IEnumerable do modelo ContratosVeiculosViewModel com -1 para se injetar na view quando for retorno de pesquisa sem resposta dando erro.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ContratosVeiculosViewModel> GetContratosVeiculosViewModelErro()
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();
            model.Add(new ContratosVeiculosViewModel() { id = -1});
            return model;
        }

        public ActionResult ConsultaContrato(string contrato, string cpf_cnpj_cliente)
        {
            if (contrato == "" || contrato == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return View(GetContratosVeiculosViewModelPrimeira());
            }

            if (cpf_cnpj_cliente != "" && cpf_cnpj_cliente != null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                string cpf_cnpj_clienteZEROS = cpf_cnpj_cliente.ToString().PadLeft(18, '0');
            }
            else
            {
                cpf_cnpj_cliente = "";
            }

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
            join b in db.Arm_Veiculos
            on a.contrato equals b.contrato
            where a.contrato.Contains(contrato)
            where a.cpf_cnpj_cliente.Contains(cpf_cnpj_cliente)
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
                origem_v = b.origem

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

                contrato_v = x.contrato_v,
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
                origem_v = x.origem_v

            }).OrderBy(x => x.placa).ToList();

            if (model.Count() == 0 || model == null)
            {
                return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaContrato");
            }

            if (model == null || model.Any() == false)
            {
                //return HttpNotFound();
                return RedirectToAction("ConsultaContrato");
            }
            return View(model);
        }


        // GET: Arm_LiquidadosEAtivos_Contrato/Details/5
        public ActionResult ConsultaVeiculo(string chassi, string placa, string renavam)
        {
            if (chassi == null)
                chassi = "";
            if (placa == null)
                placa = "";
            if (renavam == null)
                renavam = "";

            if (chassi == "" && placa == "" && renavam == "")
            {
                return View(GetContratosVeiculosViewModelPrimeira());
            }

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato
                     where b.chassi.Contains(chassi)
                     where b.placa.Contains(placa)
                     where b.renavam.Contains(renavam)
                     where a.origem.Equals("B")
                     where b.origem.Contains("RECIBO VEN")
                     select new
                     {
                         id = a.id,
                         contrato = a.contrato,
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
                         modelo = b.modelo,
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

                         contrato_v = x.contrato_v,
                         tipo_registro = x.tipo_registro,
                         marca = x.marca,
                         modelo = x.modelo,
                         tipo_v = x.tipo_v,
                         ano_fab = x.ano_fab,
                         ano_mod = x.ano_mod,
                         cor = x.cor,
                         renavam = x.renavam,
                         chassi = x.chassi,
                         placa = x.placa,
                         origem_v = x.origem_v
                     });

            if (model.Count() == 0 || model == null)
            {
                return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaVeiculo");
            }

            if (model == null || model.Any() == false)
            {
                //return HttpNotFound();
                return RedirectToAction("ConsultaVeiculo");
            }
            return View("ConsultaVeiculo", model);
        }
        
        //[Authorize, ActionName("ConsultaCliente"), HttpPost]
        public ActionResult ConsultaCliente(string cpf_cnpj_cliente)
        {
            if (cpf_cnpj_cliente == "" || cpf_cnpj_cliente == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return View(GetContratosVeiculosViewModelPrimeira());
            }

            string cpf_cnpj_clienteZEROS = cpf_cnpj_cliente.ToString().PadLeft(18, '0');

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos on a.contrato equals b.contrato
                     where
                       a.cpf_cnpj_cliente.Equals(cpf_cnpj_clienteZEROS) &&
                       a.origem == "B" &&
                       b.origem.Contains("RECIBO VEN")
                     group new { a, b } by new
                     {
                         a.nome_cliente,
                         a.cpf_cnpj_cliente,
                         a.fone_cliente_particular,
                         a.fone_cliente_cml,
                         a.end_cliente,
                         a.bairro_cliente,
                         a.cidade_cliente,
                         a.uf_cliente,
                         a.cep_cliente,
                         a.contrato,
                         a.status,
                         a.dta_inicio_contrato,
                         a.dta_vecto_contrato,
                         a.dta_ultimo_pagto,
                         a.origem,
                         column1 = b.origem
                     } into g
                      orderby g.Key.contrato,
                              g.Key.status,
                              g.Key.dta_inicio_contrato
                      select new
                     {
                         g.Key.nome_cliente,
                         g.Key.cpf_cnpj_cliente,
                         g.Key.fone_cliente_particular,
                         g.Key.fone_cliente_cml,
                         g.Key.end_cliente,
                         g.Key.bairro_cliente,
                         g.Key.cidade_cliente,
                         g.Key.uf_cliente,
                         g.Key.cep_cliente,
                         g.Key.contrato,
                         g.Key.status,
                         g.Key.dta_inicio_contrato,
                         g.Key.dta_vecto_contrato,
                         g.Key.dta_ultimo_pagto,
                         origemA = g.Key.origem,
                         origemB = g.Key.column1
                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         nome_cliente = x.nome_cliente,
                         cpf_cnpj_cliente = x.cpf_cnpj_cliente,
                         fone_cliente_particular = x.fone_cliente_particular,
                         fone_cliente_cml = x.fone_cliente_cml,
                         end_cliente = x.end_cliente,
                         bairro_cliente = x.bairro_cliente,
                         cidade_cliente = x.cidade_cliente,
                         uf_cliente = x.uf_cliente,
                         cep_cliente = x.cep_cliente,
                         contrato = x.contrato,
                         status = x.status,
                         dta_inicio_contrato = x.dta_inicio_contrato,
                         dta_vecto_contrato = x.dta_vecto_contrato,
                         dta_ultimo_pagto = x.dta_ultimo_pagto,
                         origem = x.origemA,
                         origem_v = x.origemB
                     });

            if (model.Count() == 0 || model == null)
            {
                return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaContrato");
            }

            if (model == null || model.Any() == false)
            {
                //return HttpNotFound();
                return RedirectToAction("ConsultaClienteo");
            }
            return View(model);
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
