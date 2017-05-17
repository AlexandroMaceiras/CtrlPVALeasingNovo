﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CtrlPVALeasing.Models;
using System.IO;

namespace CtrlPVALeasing.Controllers
{
    public class RelatorioController : Controller
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
            model.Add(new ContratosVeiculosViewModel() { id = 0, agencia = " " });
            return model;
        }

        public ActionResult EntradaRelatorioPerdasOperacionais()
        {
            return View(GetContratosVeiculosViewModelPrimeira());
        }

        [HttpPost]
        public ActionResult EntradaRelatorioPerdasOperacionais(DateTime? dataInicio, DateTime? dataFim, bool escolha)
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato
                     join c in db.Tbl_DebitosEPagamentos_Veiculo
                     on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }

                     join f in db.Tbl_Agencias
                     on a.agencia equals f.agencia
                     into j1
                     from f in j1.DefaultIfEmpty()

                     where c.dta_pagamento >= dataInicio
                     where c.dta_pagamento <= dataFim
                     where a.origem.Equals("B")
                     where !b.origem.Contains("RECIBO VEN")
                     select new
                     {
                         chassi             = b.chassi,
                         contrato           = a.contrato,
                         grupo_safra        = c.grupo_safra,

                         descricao_agencia  = f.descricao_agencia,
                         agencia            = a.agencia,

                         c.dta_pagamento,
                         c.valor_divida,
                         c.pci_debito_divida


                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         chassi             = x.chassi,
                         contrato           = x.contrato,
                         grupo_safra        = x.grupo_safra,

                         descricao_agencia  = x.descricao_agencia,
                         agencia            = x.agencia,

                         dta_pagamento      = x.dta_pagamento,
                         valor_divida       = x.valor_divida,
                         pci_debito_divida  = x.pci_debito_divida

                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);

            //Gera Resultado em CSV.
            if (escolha)
            {
                StringWriter sw = new StringWriter();

                sw.WriteLine("Descrição(Chassi); Nº Contrato; Nome Empresa; Cód; Área Onde Ocorreu a Perda (Descrição Agência); Cód; Tipo do Evento - 1º Nível; Cód; Tipo do Evento - 2º Nível; Cód; Tipo do Evento - 3º Nível; Cód; Fator de Risco; Id Evento Perdas Raiz; Sistema Envolvido; Cód;Produto; Cód; Cód; Data da Descoberta; Tipo de Risco Vinculado; Área que Cadastrou a Perda; Cód; Segmento Comercial; Cód; Data Contábil; Valor; PCI / PCU");

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=RelatorioPerdasOperacionais.csv");
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = System.Text.Encoding.Default;

                var users = db.Tbl_DadosDaVenda.ToList();

                foreach (var elemento in model)
                {
                    sw.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25};{26};{27};",

                    elemento.chassi,
                    elemento.contrato,
                    (elemento.grupo_safra == "10" ? "BANCO SAFRA" : (elemento.grupo_safra == "20" ? "BANCO J. SAFRA" : (elemento.grupo_safra == "30" ? "SAFRA LEASING" : (elemento.grupo_safra == "40" ? "JS ADM RECURSOS" : "")))),
                    elemento.grupo_safra,
                    elemento.descricao_agencia,
                    elemento.agencia,
                    "7 - Práticas Inadequadas relativas a contrapartes, clientes",
                    "7",
                    "7_01 - Perdas na relação de negócios com clientes",
                    "7_01",
                    "7_01_03 - Outras perdas decorrentes da relação de negócios com clientes",
                    "7_01_03",
                    "Eventos Externos",
                    "",
                    "Arrendamento Mercantil - Stored",
                    "ARM",
                    "LEASING",
                    "104010000",
                    "1",
                    "????????",
                    "Crédito",
                    "BO LEASING PESADOSPOA",
                    "37470",
                    "Bancos",
                    "PEPD",
                    (elemento.dta_pagamento.HasValue ? elemento.dta_pagamento.ToString().Substring(0, 10) : ""),
                    elemento.valor_divida,
                    elemento.pci_debito_divida
                    ));
                }
                Response.Write(sw.ToString());
                Response.End();
            }
            else
            {
                return View("RelatorioPerdasOperacionais", model);
            }

            return View(GetContratosVeiculosViewModelPrimeira());
        }


        public ActionResult RelatorioBensApreendidosDebtoIPVA()
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato
                     join c in db.Tbl_DebitosEPagamentos_Veiculo
                     on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                     join d in db.Tbl_Bens
                     on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                     join e in db.Tbl_CCL
                     on a.cpf_cnpj_cliente equals e.cpf_cnpj_cliente

                     where (c.pagamento_efet_banco == false || c.pagamento_efet_banco == null)

                     where a.origem.Equals("B")
                     where !b.origem.Contains("RECIBO VEN")
                     select new
                     {
                         contrato               = a.contrato,
                         status                 = a.status,
                         nome_cliente           = a.nome_cliente,
                         cpf_cnpj_cliente       = a.cpf_cnpj_cliente,
                         marca                  = e.marca,
                         chassi                 = b.chassi,
                         renavam                = b.renavam,
                         placa                  = b.placa,
                         dta_cobranca           = c.dta_cobranca,
                         uf_cobranca            = c.uf_cobranca,
                         tipo_cobranca          = c.tipo_cobranca,
                         valor_divida           = c.valor_divida,
                         ano_exercicio          = c.ano_exercicio,
                         pagamento_efet_banco   = c.pagamento_efet_banco

                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         contrato               = x.contrato,
                         status                 = x.status,
                         nome_cliente           = x.nome_cliente,
                         cpf_cnpj_cliente       = x.cpf_cnpj_cliente,
                         marca                  = x.marca,
                         chassi                 = x.chassi,
                         renavam                = x.renavam,
                         placa                  = x.placa,
                         dta_cobranca           = x.dta_cobranca,
                         uf_cobranca            = x.uf_cobranca,
                         tipo_cobranca          = x.tipo_cobranca,
                         valor_divida           = x.valor_divida,
                         ano_exercicio          = x.ano_exercicio,
                         pagamento_efet_banco   = x.pagamento_efet_banco

                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);

            return View("", model);
        }

        public ActionResult RelatorioCobrancasSimplesParaPagamento()
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato
                     join c in db.Tbl_DebitosEPagamentos_Veiculo
                     on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                     join d in db.Tbl_Bens
                     on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                     join e in db.Tbl_CCL
                     on a.cpf_cnpj_cliente equals e.cpf_cnpj_cliente

                     where (d.chassi != c.chassi || d.renavam != c.renavam || d.placa != c.placa)

                     where (c.pagamento_efet_banco == false || c.pagamento_efet_banco == null)

                     where e.marca.Equals("JUR")

                     where a.origem.Equals("B")
                     where !b.origem.Contains("RECIBO VEN")
                     select new
                     {
                         contrato               = a.contrato,
                         status                 = a.status,
                         nome_cliente           = a.nome_cliente,
                         cpf_cnpj_cliente       = a.cpf_cnpj_cliente,
                         marca                  = e.marca,
                         chassi                 = b.chassi,
                         renavam                = b.renavam,
                         placa                  = b.placa,
                         dta_cobranca           = c.dta_cobranca,
                         uf_cobranca            = c.uf_cobranca,
                         tipo_cobranca          = c.tipo_cobranca,
                         valor_divida           = c.valor_divida,
                         ano_exercicio          = c.ano_exercicio,
                         pagamento_efet_banco   = c.pagamento_efet_banco

                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         contrato               = x.contrato,
                         status                 = x.status,
                         nome_cliente           = x.nome_cliente,
                         cpf_cnpj_cliente       = x.cpf_cnpj_cliente,
                         marca                  = x.marca,
                         chassi                 = x.chassi,
                         renavam                = x.renavam,
                         placa                  = x.placa,
                         dta_cobranca           = x.dta_cobranca,
                         uf_cobranca            = x.uf_cobranca,
                         tipo_cobranca          = x.tipo_cobranca,
                         valor_divida           = x.valor_divida,
                         ano_exercicio          = x.ano_exercicio,
                         pagamento_efet_banco   = x.pagamento_efet_banco

                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);

            return View("", model);
        }

        public ActionResult RelatorioCobrancaIPVAAreaBens()
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato
                     join c in db.Tbl_DebitosEPagamentos_Veiculo
                     on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                     join d in db.Tbl_Bens
                     on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }

                     join e in db.Tbl_CCL
                     on a.cpf_cnpj_cliente equals e.cpf_cnpj_cliente
                     into j1
                     from e in j1.DefaultIfEmpty() //Isto é um LEFT JOIN pra trazer quem esta na CCL e quem não está também.

                     where c.pagamento_efet_banco == true

                     where (c.dta_recuperacao == null || c.valor_recuperado == null || c.valor_total_recuperado == null)

                     where a.origem.Equals("B")
                     where !b.origem.Contains("RECIBO VEN")
                     select new
                     {
                         contrato           = a.contrato,
                         status             = a.status,
                         nome_cliente       = a.nome_cliente,
                         cpf_cnpj_cliente   = a.cpf_cnpj_cliente,
                         marca              = e.marca,
                         chassi             = b.chassi,
                         renavam            = b.renavam,
                         placa              = b.placa,
                         valor_divida       = c.valor_divida,
                         valor_custas       = c.valor_custas,
                         valor_debito_total = c.valor_debito_total,
                         ano_exercicio      = c.ano_exercicio,
                         valor_pago_total   = c.valor_divida + c.valor_custas


                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         contrato           = x.contrato,
                         status             = x.status,
                         nome_cliente       = x.nome_cliente,
                         cpf_cnpj_cliente   = x.cpf_cnpj_cliente,
                         marca              = x.marca,
                         chassi             = x.chassi,
                         renavam            = x.renavam,
                         placa              = x.placa,
                         valor_divida       = x.valor_divida,
                         valor_custas       = x.valor_custas,
                         valor_debito_total = x.valor_debito_total,
                         ano_exercicio      = x.ano_exercicio,
                         valor_pago_total   = x.valor_divida + x.valor_custas

                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);

            return View("", model);
        }


        public ActionResult RelatorioDebitoEmContaCorrente()
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato
                     join c in db.Tbl_DebitosEPagamentos_Veiculo
                     on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }

                     join d in db.Tbl_Bens
                     on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                     into j2
                     from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN pra trazer quem esta na Bens e quem não está também.

                     join f in db.Tbl_SCC
                     on a.cpf_cnpj_cliente equals f.cpf_cnpj_cliente


                     join e in db.Tbl_CCL
                     on a.cpf_cnpj_cliente equals e.cpf_cnpj_cliente
                     into j1
                     from e in j1.DefaultIfEmpty() //Isto é um LEFT JOIN pra trazer quem esta na CCL e quem não está também.

                     where c.pagamento_efet_banco == true

                     where a.origem.Equals("B")
                     where !b.origem.Contains("RECIBO VEN")

                     where (c.dta_recuperacao == null || c.valor_recuperado == null || c.valor_total_recuperado == null)

                     where (b.chassi != d.chassi || b.renavam != d.renavam || b.placa != b.placa)

                     where (f.perm_debito == true)

                     where (f.sinal == "+")

                     where (f.saldo >= c.valor_debito_total)

                     select new
                     {
                         contrato           = a.contrato,
                         status             = a.status,
                         nome_cliente       = a.nome_cliente,
                         cpf_cnpj_cliente   = a.cpf_cnpj_cliente,
                         chassi             = b.chassi,
                         agencia            = a.agencia,
                         conta              = f.conta,
                         valor_divida       = c.valor_divida,
                         valor_custas       = c.valor_custas,
                         valor_debito_total = c.valor_debito_total,
                         ano_exercicio      = c.ano_exercicio,
                         valor_pago_total   = c.valor_divida + c.valor_custas

                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         contrato           = x.contrato,
                         status             = x.status,
                         nome_cliente       = x.nome_cliente,
                         cpf_cnpj_cliente   = x.cpf_cnpj_cliente,
                         chassi             = x.chassi,
                         agencia            = x.agencia,
                         conta              = x.conta,
                         valor_divida       = x.valor_divida,
                         valor_custas       = x.valor_custas,
                         valor_debito_total = x.valor_debito_total,
                         ano_exercicio      = x.ano_exercicio,
                         valor_pago_total   = x.valor_divida + x.valor_custas

                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);

            return View("", model);
        }

        public ActionResult RelatorioParaIncorporacaoDeDividaJurRat(string RAT)
        {
            if (RAT != "true")
            {
                model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                          join b in db.Arm_Veiculos
                          on a.contrato equals b.contrato
                          join c in db.Tbl_DebitosEPagamentos_Veiculo
                          on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                          join d in db.Tbl_Bens
                          on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                          join e in db.Tbl_CCL
                          on a.cpf_cnpj_cliente equals e.cpf_cnpj_cliente

                          where c.pagamento_efet_banco == true

                          where (d.chassi != c.chassi || d.renavam != c.renavam || d.placa != c.placa)

                          where (c.dta_recuperacao == null || c.valor_recuperado == null || c.valor_total_recuperado == null)

                          where e.marca.Equals("JUR")

                          where a.origem.Equals("B")
                          where !b.origem.Contains("RECIBO VEN")
                          select new
                          {
                              contrato              = a.contrato,
                              status                = a.status,
                              nome_cliente          = a.nome_cliente,
                              cpf_cnpj_cliente      = a.cpf_cnpj_cliente,
                              marca                 = e.marca,
                              chassi                = b.chassi,
                              renavam               = b.renavam,
                              placa                 = b.placa,
                              valor_divida          = c.valor_divida,
                              valor_custas          = c.valor_custas,
                              valor_debito_total    = c.valor_debito_total,
                              ano_exercicio         = c.ano_exercicio,
                              valor_pago_total      = c.valor_divida + c.valor_custas

                          }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                          {
                              contrato              = x.contrato,
                              status                = x.status,
                              nome_cliente          = x.nome_cliente,
                              cpf_cnpj_cliente      = x.cpf_cnpj_cliente,
                              marca                 = x.marca,
                              chassi                = x.chassi,
                              renavam               = x.renavam,
                              placa                 = x.placa,
                              valor_divida          = x.valor_divida,
                              valor_custas          = x.valor_custas,
                              valor_debito_total    = x.valor_debito_total,
                              ano_exercicio         = x.ano_exercicio,
                              valor_pago_total      = x.valor_divida + x.valor_custas

                          }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);
            }
            else
            {
                model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                          join b in db.Arm_Veiculos
                          on a.contrato equals b.contrato
                          join c in db.Tbl_DebitosEPagamentos_Veiculo
                          on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                          join d in db.Tbl_Bens
                          on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                          join e in db.Tbl_CCL
                          on a.cpf_cnpj_cliente equals e.cpf_cnpj_cliente

                          where c.pagamento_efet_banco == true

                          where (d.chassi != c.chassi || d.renavam != c.renavam || d.placa != c.placa)

                          where (c.dta_recuperacao == null || c.valor_recuperado == null || c.valor_total_recuperado == null)

                          where e.marca != "JUR"

                          where a.origem.Equals("B")
                          where !b.origem.Contains("RECIBO VEN")
                          select new
                          {
                              contrato              = a.contrato,
                              status                = a.status,
                              nome_cliente          = a.nome_cliente,
                              cpf_cnpj_cliente      = a.cpf_cnpj_cliente,
                              marca                 = e.marca,
                              chassi                = b.chassi,
                              renavam               = b.renavam,
                              placa                 = b.placa,
                              valor_divida          = c.valor_divida,
                              valor_custas          = c.valor_custas,
                              valor_debito_total    = c.valor_debito_total,
                              ano_exercicio         = c.ano_exercicio,
                              valor_pago_total      = c.valor_divida + c.valor_custas

                          }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                          {
                              contrato              = x.contrato,
                              status                = x.status,
                              nome_cliente          = x.nome_cliente,
                              cpf_cnpj_cliente      = x.cpf_cnpj_cliente,
                              marca                 = x.marca,
                              chassi                = x.chassi,
                              renavam               = x.renavam,
                              placa                 = x.placa,
                              valor_divida          = x.valor_divida,
                              valor_custas          = x.valor_custas,
                              valor_debito_total    = x.valor_debito_total,
                              ano_exercicio         = x.ano_exercicio,
                              valor_pago_total      = x.valor_divida + x.valor_custas

                          }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);
            }

            return View("", model);
        }

        public ActionResult NotificacaoDeDebitosDeIPVAParaClientes()
        {

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato
                     join c in db.Tbl_DebitosEPagamentos_Veiculo
                     on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }

                     join d in db.Tbl_Bens
                     on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                     into j2
                     from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN pra trazer quem esta na Bens e quem não está também.

                     //join e in db.Tbl_CCL
                     //on a.cpf_cnpj_cliente equals e.cpf_cnpj_cliente

                     //where (c.pagamento_efet_banco == false || c.pagamento_efet_banco == null)

                     //where a.origem.Equals("B")
                     //where !b.origem.Contains("RECIBO VEN")

                     //where (b.chassi != d.chassi || b.renavam != d.renavam || b.placa != b.placa)

                     //where e.marca != "JUR"

                     //where c.tipo_cobranca == "C"

                     select new
                     {
                         contrato = a.contrato,
                         status = a.status,
                         nome_cliente = a.nome_cliente,
                         cpf_cnpj_cliente = a.cpf_cnpj_cliente,
                         end_cliente = a.end_cliente,
                         bairro_cliente = a.bairro_cliente,
                         cep_cliente = a.cep_cliente,
                         cidade_cliente = a.cidade_cliente,
                         uf_cliente = a.uf_cliente,
                         chassi = b.chassi,
                         renavam = b.renavam,
                         placa = b.placa,
                         dta_cobranca = c.dta_cobranca,
                         uf_cobranca = c.uf_cobranca,
                         valor_divida = c.valor_divida,
                         ano_exercicio = c.ano_exercicio

                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         contrato = x.contrato,
                         status = x.status,
                         nome_cliente = x.nome_cliente,
                         cpf_cnpj_cliente = x.cpf_cnpj_cliente,
                         end_cliente = x.end_cliente,
                         bairro_cliente = x.bairro_cliente,
                         cep_cliente = x.cep_cliente,
                         cidade_cliente = x.cidade_cliente,
                         uf_cliente = x.uf_cliente,
                         chassi = x.chassi,
                         renavam = x.renavam,
                         placa = x.placa,
                         dta_cobranca = x.dta_cobranca,
                         uf_cobranca = x.uf_cobranca,
                         valor_divida = x.valor_divida,
                         ano_exercicio = x.ano_exercicio

                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);//.Take(5);

            return View("", model);
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