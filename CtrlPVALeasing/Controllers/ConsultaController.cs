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
        IEnumerable<Arm_Veiculos> model2 = null;
        IEnumerable<Arm_LiquidadosEAtivos_Contrato> model3 = null;

        /// <summary>
        /// Cria um IEnumerable do modelo ContratosVeiculosViewModel vazio para se injetar na PARTIAL VIEW pela primeira vez quando ela carrega sem ninguém.
        /// Sem este IEnumarable epecífico para a Partial View ela lê o rodapé da _ViewStart na primeira vêz e imprime ele no local da partial View.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ContratosVeiculosViewModel> GetContratosVeiculosViewModelPrimeiraRodape()
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();
            model.Add(new ContratosVeiculosViewModel() { id = 0, agencia = "rodap" });
            return model;
        }

        /// <summary>
        /// Cria um IEnumerable do modelo ContratosVeiculosViewModel vazio para se injetar na view pela primeira vez quando ela carrega sem ninguém.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ContratosVeiculosViewModel> GetContratosVeiculosViewModelPrimeira()
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();
            model.Add(new ContratosVeiculosViewModel(){ id = 0, agencia = " "});
            return model;
        }

        /// <summary>
        /// Cria um IEnumerable do modelo ContratosVeiculosViewModel com -1 para se injetar na view quando for retorno de pesquisa sem resposta dando erro.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ContratosVeiculosViewModel> GetContratosVeiculosViewModelErro()
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();
            model.Add(new ContratosVeiculosViewModel() { id = -1, agencia = " " });
            return model;
        }

        private IEnumerable<ContratosVeiculosViewModel> Nada()
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();
            model.Add(new ContratosVeiculosViewModel() { id = -2, agencia = " " });
            return model;
        }

        public ActionResult ConsultaDebito(string chassi, string placa, string renavam)
        {
            if ((chassi == "" || chassi == null) && (placa == "" || placa == null) && (renavam == "" || renavam == null))
            {
                return View(Nada());
            }

            renavam = renavam.TrimStart('0');

            if (chassi == "" || chassi == null)
                chassi = "ø";
            if (placa == "" || placa == null)
                placa = "ø";
            if (renavam == "" || renavam == null)
                renavam = "ø";
            model2 = db.Arm_Veiculos.Where(c => c.chassi == chassi || c.renavam == renavam || c.placa == placa);
            if (model2.Count() != 0)
            {
                //Se for incluido manualmente
                if (model2.FirstOrDefault().flag_manual == true)
                {
                    var encontrado = model2.FirstOrDefault();
                    model = (from b in db.Arm_Veiculos.Where(LEAC => (encontrado.contrato == LEAC.contrato)).DefaultIfEmpty()
                                 //Isto é um LEFT JOIN de LINQ com lambda, feito logo na raiz por precisar ter todos os contratos mesmo não tendo nenhum veículo associado.

                             from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(Depv =>
                             (b.chassi == Depv.chassi) || (b.renavam == Depv.renavam) || (b.placa == Depv.placa)).DefaultIfEmpty()
                                 //on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                                 //into j1
                                 //from c in j1.DefaultIfEmpty() //Isto é um LEFT JOIN

                             from d in db.Tbl_Dut.Where(Dut =>
                             (b.chassi == Dut.chassi) || (b.renavam == Dut.renavam) || (b.placa == Dut.placa)).DefaultIfEmpty()
                                 //on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                                 //into j2
                                 //from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN

                             from e in db.Tbl_Bens.Where(Bens => (b.chassi == Bens.chassi) || (b.renavam == Bens.renavam) || (b.placa == Bens.placa)).DefaultIfEmpty()
                                 //on new { b.chassi, b.renavam, b.placa } equals new { e.chassi, e.renavam, e.placa }
                                 //into j3
                                 //from e in j3.DefaultIfEmpty() //Isto é um LEFT JOIN

                             where (b.chassi.Equals(chassi) || b.placa.Equals(placa) || b.renavam.Equals(renavam))
                             //where a.origem.Equals("B")
                             where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
                             select new
                             {
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
                                 origem_v = b.origem,
                                 flag_manual = b.flag_manual,

                                 valor_debito_total = c.valor_debito_total,
                                 dta_cobranca = c.dta_cobranca,
                                 uf_pagamento = c.uf_pagamento,
                                 valor_divida = c.valor_divida,
                                 ano_exercicio = c.ano_exercicio,
                                 valor_custas = c.valor_custas,
                                 pagamento_efet_banco = c.pagamento_efet_banco,
                                 valor_recuperado = c.valor_recuperado,
                                 valor_total_recuperado = c.valor_total_recuperado,
                                 divida_ativa_serasa = c.divida_ativa_serasa,
                                 dta_pagamento_divida = c.dta_pagamento_divida,
                                 dta_pagamento_custas = c.dta_pagamento_custas,

                                 comVenda = d.comVenda,

                                 renavam_bens = e.renavam,
                                 chassi_bens = e.chassi,
                                 placa_bens = e.placa,

                             }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                             {
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
                                 origem_v = x.origem_v,
                                 flag_manual = x.flag_manual,

                                 valor_debito_total = x.valor_debito_total,
                                 dta_cobranca = x.dta_cobranca,
                                 uf_pagamento = x.uf_pagamento,
                                 valor_divida = x.valor_divida,
                                 ano_exercicio = x.ano_exercicio,
                                 valor_custas = x.valor_custas,
                                 pagamento_efet_banco = x.pagamento_efet_banco,
                                 valor_recuperado = x.valor_recuperado,
                                 valor_total_recuperado = x.valor_total_recuperado,
                                 divida_ativa_serasa = x.divida_ativa_serasa,
                                 dta_pagamento_divida = x.dta_pagamento_divida,
                                 dta_pagamento_custas = x.dta_pagamento_custas,

                                 comVenda = x.comVenda,

                                 renavam_bens = x.renavam,
                                 chassi_bens = x.chassi,
                                 placa_bens = x.placa,

                             }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);
                }
                else
                {
                    model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                             from b in db.Arm_Veiculos.Where(LEAC => (a.contrato == LEAC.contrato)).DefaultIfEmpty()
                                 //Isto é um LEFT JOIN de LINQ com lambda, feito logo na raiz por precisar ter todos os contratos mesmo não tendo nenhum veículo associado.

                             from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(Depv =>
                             (b.chassi == Depv.chassi) || (b.renavam == Depv.renavam) || (b.placa == Depv.placa)).DefaultIfEmpty()
                                 //on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                                 //into j1
                                 //from c in j1.DefaultIfEmpty() //Isto é um LEFT JOIN

                             from d in db.Tbl_Dut.Where(Dut =>
                             (b.chassi == Dut.chassi) || (b.renavam == Dut.renavam) || (b.placa == Dut.placa)).DefaultIfEmpty()
                                 //on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                                 //into j2
                                 //from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN

                             from e in db.Tbl_Bens.Where(Bens => (b.chassi == Bens.chassi) || (b.renavam == Bens.renavam) || (b.placa == Bens.placa)).DefaultIfEmpty()
                                 //on new { b.chassi, b.renavam, b.placa } equals new { e.chassi, e.renavam, e.placa }
                                 //into j3
                                 //from e in j3.DefaultIfEmpty() //Isto é um LEFT JOIN

                             join f in db.Tbl_SCC
                             on new { a.cpf_cnpj_cliente } equals new { f.cpf_cnpj_cliente }
                             into j4
                             from f in j4.DefaultIfEmpty() //Isto é um LEFT JOIN

                             where (b.chassi.Equals(chassi) || b.placa.Equals(placa) || b.renavam.Equals(renavam))
                             //where a.origem.Equals("B")
                             where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
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
                                 origem_v = b.origem,

                                 valor_debito_total = c.valor_debito_total,
                                 dta_cobranca = c.dta_cobranca,
                                 uf_pagamento = c.uf_pagamento,
                                 valor_divida = c.valor_divida,
                                 ano_exercicio = c.ano_exercicio,
                                 valor_custas = c.valor_custas,
                                 pagamento_efet_banco = c.pagamento_efet_banco,
                                 valor_recuperado = c.valor_recuperado,
                                 valor_total_recuperado = c.valor_total_recuperado,
                                 divida_ativa_serasa = c.divida_ativa_serasa,
                                 dta_pagamento_divida = c.dta_pagamento_divida,
                                 dta_pagamento_custas = c.dta_pagamento_custas,

                                 comVenda = d.comVenda,

                                 renavam_bens = e.renavam,
                                 chassi_bens = e.chassi,
                                 placa_bens = e.placa,

                                 conta = f.conta

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
                                 origem_v = x.origem_v,

                                 valor_debito_total = x.valor_debito_total,
                                 dta_cobranca = x.dta_cobranca,
                                 uf_pagamento = x.uf_pagamento,
                                 valor_divida = x.valor_divida,
                                 ano_exercicio = x.ano_exercicio,
                                 valor_custas = x.valor_custas,
                                 pagamento_efet_banco = x.pagamento_efet_banco,
                                 valor_recuperado = x.valor_recuperado,
                                 valor_total_recuperado = x.valor_total_recuperado,
                                 divida_ativa_serasa = x.divida_ativa_serasa,
                                 dta_pagamento_divida = x.dta_pagamento_divida,
                                 dta_pagamento_custas = x.dta_pagamento_custas,

                                 comVenda = x.comVenda,

                                 renavam_bens = x.renavam,
                                 chassi_bens = x.chassi,
                                 placa_bens = x.placa,

                                 conta = x.conta

                             }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);
                }
            }
            if (model.Count() == 0 || model == null)
            {
                return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaVeiculo");
            }

            if (chassi == "" && placa == "" && renavam == "")
            {
                return View(Nada()); //RedirectToAction("ConsultaVeiculo");
            }

            if (model == null || model.Any() == false)
            {
                //return HttpNotFound();
                return RedirectToAction("ConsultaDebito");
            }
            return View("ConsultaDebito", model);
        }

        public ActionResult _valor_total_recuperado_cnpj(string cpf_cnpj_cliente)
        {
            if (cpf_cnpj_cliente == "" || cpf_cnpj_cliente == null)
            {
                return View(GetContratosVeiculosViewModelPrimeiraRodape());
            }
            else if (cpf_cnpj_cliente != "" && cpf_cnpj_cliente != null)
            {
                string cpf_cnpj_clienteZEROS = cpf_cnpj_cliente.ToString().PadLeft(18, '0');
            }
            else
            {
                cpf_cnpj_cliente = "";
            }

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato

                     join c in db.Tbl_DebitosEPagamentos_Veiculo
                    on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                     where a.cpf_cnpj_cliente.Contains(cpf_cnpj_cliente)
                     where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
                     //where a.status.Equals(1)
                     group c by new { c.chassi, c.renavam, c.placa } into g
                     select new
                     {
                         soma_valor_total_recuperado = g.Sum(c => c.valor_total_recuperado)

                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         valor_total_recuperado = x.soma_valor_total_recuperado

                     });

            if (model.Count() == 0 || model == null)
            {
                return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaContrato");
            }

            if (model == null || model.Any() == false)
            {
                //return HttpNotFound();
                return RedirectToAction("ConsultaContrato");
            }

            return PartialView(model);
        }

        public ActionResult _valor_total_recuperado_contrato(string contrato)
        {
            if (contrato == "" || contrato == null)
            {
                return View(GetContratosVeiculosViewModelPrimeiraRodape());
            }
            else if (contrato != "" && contrato != null)
            {
                string contratoZEROS = contrato.ToString();
            }
            else
            {
                contrato = "";
            }

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato

                     join c in db.Tbl_DebitosEPagamentos_Veiculo
                    on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                     where a.contrato.Contains(contrato)
                     where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
                     //where a.status.Equals(1)
                     group c by new { c.chassi, c.renavam, c.placa } into g
                     select new
                     {
                         soma_valor_total_recuperado = g.Sum(c => c.valor_total_recuperado)

                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         valor_total_recuperado = x.soma_valor_total_recuperado

                     });

            if (model.Count() == 0 || model == null)
            {
                return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaContrato");
            }

            if (model == null || model.Any() == false)
            {
                //return HttpNotFound();
                return RedirectToAction("ConsultaContrato");
            }

            return PartialView(model);
        }

        public ActionResult _valor_total_recuperado_por_veiculo(string chassi, string placa, string renavam)
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
            model2 = db.Arm_Veiculos.Where(c => c.chassi == chassi || c.renavam == renavam || c.placa == placa);

            if (model2.FirstOrDefault().flag_manual == true)
            {
                model = (from b in db.Arm_Veiculos
                         join c in db.Tbl_DebitosEPagamentos_Veiculo
                         on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                         where b.chassi.Contains(chassi)
                         where b.placa.Contains(placa)
                         where b.renavam.Contains(renavam)
                         group c by new { c.chassi, c.renavam, c.placa } into g
                         select new
                         {
                             soma_valor_total_recuperado = g.Sum(c => c.valor_total_recuperado)

                         }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                         {
                             valor_total_recuperado = x.soma_valor_total_recuperado

                         });
            }
            else
            {
                model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                         join b in db.Arm_Veiculos
                         on a.contrato equals b.contrato

                         join c in db.Tbl_DebitosEPagamentos_Veiculo
                        on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                         where b.chassi.Contains(chassi)
                         where b.placa.Contains(placa)
                         where b.renavam.Contains(renavam)
                         where a.origem.Equals("B")
                         where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
                         //where a.status.Equals(1)
                         group c by new { c.chassi, c.renavam, c.placa } into g
                         select new
                         {
                             soma_valor_total_recuperado = g.Sum(c => c.valor_total_recuperado)

                         }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                         {
                             valor_total_recuperado = x.soma_valor_total_recuperado

                         });
            }
            if (model.Count() == 0 || model == null)
            {
                return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaContrato");
            }

            if (model == null || model.Any() == false)
            {
                //return HttpNotFound();
                return RedirectToAction("ConsultaContrato");
            }

            return PartialView(model);
        }

        public ActionResult _valor_debito_total_por_veiculo(string chassi, string placa, string renavam)
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
            model2 = db.Arm_Veiculos.Where(c => c.chassi == chassi || c.renavam == renavam || c.placa == placa);

            if (model2.FirstOrDefault().flag_manual == true)
            {
                model = (from b in db.Arm_Veiculos

                         join c in db.Tbl_DebitosEPagamentos_Veiculo
                        on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                         where b.chassi.Contains(chassi)
                         where b.placa.Contains(placa)
                         where b.renavam.Contains(renavam)
                         group c by new { c.chassi, c.renavam, c.placa } into g
                         select new
                         {
                             soma_valor_debito_total = g.Sum(c => c.valor_debito_total)

                         }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                         {
                             valor_debito_total = x.soma_valor_debito_total

                         });
            }
            else
            {
                model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                         join b in db.Arm_Veiculos
                         on a.contrato equals b.contrato

                         join c in db.Tbl_DebitosEPagamentos_Veiculo
                        on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                         where b.chassi.Contains(chassi)
                         where b.placa.Contains(placa)
                         where b.renavam.Contains(renavam)
                         where a.origem.Equals("B")
                         where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
                         //where a.status.Equals(1)
                         group c by new { c.chassi, c.renavam, c.placa } into g
                         select new
                         {
                             soma_valor_debito_total = g.Sum(c => c.valor_debito_total)

                         }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                         {
                             valor_debito_total = x.soma_valor_debito_total

                         });
            }
            if (model.Count() == 0 || model == null)
            {
                return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaContrato");
            }

            if (model == null || model.Any() == false)
            {
                //return HttpNotFound();
                return RedirectToAction("ConsultaContrato");
            }

            return PartialView(model);
        }

        public ActionResult _valor_debito_total(string cpf_cnpj_cliente)
        {
            if (cpf_cnpj_cliente == "" || cpf_cnpj_cliente == null)
            {
                return View(GetContratosVeiculosViewModelPrimeiraRodape());
            }else if (cpf_cnpj_cliente != "" && cpf_cnpj_cliente != null)
            {
                string cpf_cnpj_clienteZEROS = cpf_cnpj_cliente.ToString().PadLeft(18, '0');
            }
            else
            {
                cpf_cnpj_cliente = "";
            }

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                    join b in db.Arm_Veiculos
                    on a.contrato equals b.contrato
                    where a.cpf_cnpj_cliente.Contains(cpf_cnpj_cliente)
                    where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
                     //where a.status.Value.Equals(true)
                     join c in db.Tbl_DebitosEPagamentos_Veiculo
                    on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                     group c by new { c.chassi, c.renavam, c.placa } into g
                     select new
                    {
                         soma_valor_debito_total = g.Sum(c => c.valor_debito_total)

                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                    {
                         valor_debito_total = x.soma_valor_debito_total

                     });

            if (model.Count() == 0 || model == null)
            {
                return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaContrato");
            }

            if (model == null || model.Any() == false)
            {
                //return HttpNotFound();
                return RedirectToAction("ConsultaContrato");
            }

            return PartialView(model);
        }

        public ActionResult ConsultaContrato(string contrato, string cpf_cnpj_cliente)
        {
            if (contrato == "" || contrato == null || contrato == "-1")
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

            model3 = db.Arm_LiquidadosEAtivos_Contrato.Where(c => c.contrato == contrato);
            if (model3.Count() != 0)
            {
                //Se for incluido manualmente
                if (model3.FirstOrDefault().flag_manual == true)
                {
                    var encontrado = model3.FirstOrDefault();
                    model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                             where (a.contrato.Equals(encontrado.contrato))
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
                                 status = a.status

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
                             });
                }
                else
                {

                    model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                             from b in db.Arm_Veiculos.Where(LEAC => (a.contrato == LEAC.contrato)).DefaultIfEmpty()
                                 //Isto é um LEFT JOIN de LINQ com lambda, feito logo na raiz por precisar ter todos os contratos mesmo não tendo nenhum veículo associado.

                             join c in db.Tbl_Dut
                             on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                             into j1
                             from c in j1.DefaultIfEmpty() //Isto é um LEFT JOIN

                             join d in db.Tbl_CCL
                             on new { a.cpf_cnpj_cliente } equals new { d.cpf_cnpj_cliente }
                             into j2
                             from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN

                             join e in db.Tbl_SCC
                             on new { a.cpf_cnpj_cliente } equals new { e.cpf_cnpj_cliente }
                             into j3
                             from e in j3.DefaultIfEmpty() //Isto é um LEFT JOIN

                             where a.contrato == contrato
                             where a.cpf_cnpj_cliente.Contains(cpf_cnpj_cliente)
                             //where a.origem.Equals("B")
                             where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
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
                                 origem_v = b.origem,

                                 comDUT = c.comDUT,
                                 comVenda = c.comVenda,

                                 marcas = d.marca,

                                 conta = e.conta

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
                                 origem_v = x.origem_v,

                                 comDUT = x.comDUT,
                                 comVenda = x.comVenda,

                                 CCL_marca = x.marcas,

                                 conta = x.conta

                             }).OrderBy(x => x.placa).ToList();
                }
            }
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
            if ((chassi == "" || chassi == null) && (placa == "" || placa == null) && (renavam == "" || renavam == null))
            {
                return View(GetContratosVeiculosViewModelPrimeira());
            }

            if (chassi == "" || chassi == null)
                chassi = "ø";
            if (placa == "" || placa == null)
                placa = "ø";
            if (renavam == "" || renavam == null)
                renavam = "ø";

            model2 = db.Arm_Veiculos.Where(c => c.chassi == chassi || c.renavam == renavam || c.placa == placa);
            if (model2.Count() != 0)
            {
                //Se for incluido manualmente
                if (model2.FirstOrDefault().flag_manual == true)
                {
                    var encontrado = model2.FirstOrDefault();
                    model = (from a in db.Arm_Veiculos
                             where (a.chassi.Equals(encontrado.chassi) || a.placa.Equals(encontrado.placa) || a.renavam.Equals(encontrado.renavam))
                             select new
                             {
                                 id = a.id,
                                 contrato = a.contrato,
                                 tipo_registro = a.tipo_registro,
                                 marca = a.marca,
                                 modelo = a.modelo,
                                 tipo = a.tipo,
                                 ano_fab = a.ano_fab,
                                 ano_mod = a.ano_mod,
                                 cor = a.cor,
                                 renavam = a.renavam,
                                 chassi = a.chassi,
                                 placa = a.placa,
                                 origem = a.origem,
                                 contrato_v = a.contrato

                             }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                             {
                                 id = x.id,
                                 contrato = x.contrato,
                                 tipo_registro = x.tipo_registro,
                                 marca = x.marca,
                                 modelo = x.modelo,
                                 tipo = x.tipo,
                                 ano_fab = x.ano_fab,
                                 ano_mod = x.ano_mod,
                                 cor = x.cor,
                                 renavam = x.renavam,
                                 chassi = x.chassi,
                                 placa = x.placa,
                                 origem = x.origem,
                                 contrato_v = x.contrato
                             });
                }
                else
                {
                    model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                             join b in db.Arm_Veiculos
                             on a.contrato equals b.contrato

                             from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv =>
                             (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa)).DefaultIfEmpty()
                                 //on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                                 //into j1
                                 //from c in j1.DefaultIfEmpty() //Isto é um LEFT JOIN

                             join d in db.Tbl_Bens
                             on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                             into j2
                             from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN

                             join e in db.Tbl_Dut
                             on new { b.chassi, b.renavam, b.placa } equals new { e.chassi, e.renavam, e.placa }
                             into j3
                             from e in j3.DefaultIfEmpty() //Isto é um LEFT JOIN

                             join f in db.Tbl_CCL
                             on new { a.cpf_cnpj_cliente } equals new { f.cpf_cnpj_cliente }
                             into j4
                             from f in j4.DefaultIfEmpty() //Isto é um LEFT JOIN

                             join g in db.Tbl_SCC
                             on new { a.cpf_cnpj_cliente } equals new { g.cpf_cnpj_cliente }
                             into j5
                             from g in j5.DefaultIfEmpty() //Isto é um LEFT JOIN

                             where (b.chassi.Equals(chassi) || b.placa.Equals(placa) || b.renavam.Equals(renavam))
                             //where a.origem.Equals("B")
                             where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
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
                                 origem_v = b.origem,

                                 dta_cobranca = c.dta_cobranca,
                                 valor_debito_total = c.valor_debito_total,
                                 dta_pagamento_divida = c.dta_pagamento_divida,
                                 valor_pago_total = c.valor_pago_total,
                                 divida_ativa_serasa = c.divida_ativa_serasa,

                                 renavam_bens = d.renavam,
                                 chassi_bens = d.chassi,
                                 placa_bens = d.placa,

                                 comVenda = e.comVenda,

                                 marcas = f.marca,

                                 conta = g.conta

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
                                 origem_v = x.origem_v,

                                 dta_cobranca = x.dta_cobranca,
                                 valor_debito_total = x.valor_debito_total,
                                 dta_pagamento_divida = x.dta_pagamento_divida,
                                 valor_pago_total = x.valor_pago_total,
                                 divida_ativa_serasa = x.divida_ativa_serasa,

                                 renavam_bens = x.renavam_bens,
                                 chassi_bens = x.chassi_bens,
                                 placa_bens = x.placa_bens,

                                 comVenda = x.comVenda,

                                 CCL_marca = x.marcas,

                                 conta = x.conta
                             });
                }
            }

            if (model == null || model.Count() == 0)
            {
                return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaVeiculo");
            }
            return View("ConsultaVeiculo", model);
        }

        public ActionResult TesteComMascara()
        {
            return View();
        }

        //[Authorize, ActionName("ConsultaCliente"), HttpPost]
        public ActionResult ConsultaCliente(string cpf_cnpj_cliente)
        {
            if (cpf_cnpj_cliente == "" || cpf_cnpj_cliente == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return View(GetContratosVeiculosViewModelPrimeira());
            }

            string cpf_cnpj_clienteZEROS = cpf_cnpj_cliente.Trim().ToString().PadLeft(18, '0');

            model3 = db.Arm_LiquidadosEAtivos_Contrato.Where(c => c.cpf_cnpj_cliente == cpf_cnpj_clienteZEROS);
            if (model3.Count() != 0)
            {
                if (model3.FirstOrDefault().flag_manual == true)
                {
                    var encontrado = model3.FirstOrDefault();
                    model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                             where (a.cpf_cnpj_cliente.Equals(encontrado.cpf_cnpj_cliente))
                             select new
                             {
                                 id = a.id,
                                 nome_cliente = a.nome_cliente,
                                 cpf_cnpj_cliente = a.cpf_cnpj_cliente,
                                 ddd_cliente_particular = a.ddd_cliente_particular,
                                 fone_cliente_particular = a.fone_cliente_particular,
                                 ddd_cliente_cml = a.ddd_cliente_cml,
                                 fone_cliente_cml = a.fone_cliente_cml,
                                 end_cliente = a.end_cliente,
                                 bairro_cliente = a.bairro_cliente,
                                 cidade_cliente = a.cidade_cliente,
                                 uf_cliente = a.uf_cliente,
                                 cep_cliente = a.cep_cliente,
                                 contrato = a.contrato,
                                 status = a.status,
                                 dta_inicio_contrato = a.dta_inicio_contrato,
                                 dta_vecto_contrato = a.dta_vecto_contrato,
                                 dta_ultimo_pagto = a.dta_ultimo_pagto,
                                 origem = a.origem,
                                 data_da_baixa = a.data_da_baixa

                             }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                             {
                                 id = x.id,
                                 nome_cliente = x.nome_cliente,
                                 cpf_cnpj_cliente = x.cpf_cnpj_cliente,
                                 ddd_cliente_particular = x.ddd_cliente_particular,
                                 fone_cliente_particular = x.fone_cliente_particular,
                                 ddd_cliente_cml = x.ddd_cliente_cml,
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
                                 origem = x.origem,
                                 data_da_baixa = x.data_da_baixa
                             });
                }
                else
                {
                    model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                             join b in db.Arm_Veiculos
                             on a.contrato equals b.contrato

                             join c in db.Tbl_Dut
                             on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                             into j1
                             from c in j1.DefaultIfEmpty() //Isto é um LEFT JOIN

                             join d in db.Tbl_CCL
                             on new { a.cpf_cnpj_cliente } equals new { d.cpf_cnpj_cliente }
                             into j2
                             from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN

                             where a.cpf_cnpj_cliente.Contains(cpf_cnpj_cliente) && //Não estou usando o cpf_cnpj_clienteZEROS porque decidiu-se que aqui não deveria-se fazer uma pesquisa exata para podermos pesquisar por RAIZ de CNPJs de uma mesma empresa.
                                                                                    //a.origem == "B" &&
                               (!b.origem.Contains("RECIBO VEN") || b.origem == null)
                             group new { a, b } by new
                             {
                                 a.nome_cliente,
                                 a.cpf_cnpj_cliente,
                                 a.ddd_cliente_particular,
                                 a.fone_cliente_particular,
                                 a.ddd_cliente_cml,
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
                                 a.data_da_baixa,
                                 column1 = b.origem,
                                 d.marca
                             } into g
                             orderby g.Key.cpf_cnpj_cliente,
                                     g.Key.contrato,
                                     g.Key.status,
                                     g.Key.dta_inicio_contrato
                             select new
                             {
                                 g.Key.nome_cliente,
                                 g.Key.cpf_cnpj_cliente,
                                 g.Key.ddd_cliente_particular,
                                 g.Key.fone_cliente_particular,
                                 g.Key.ddd_cliente_cml,
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
                                 g.Key.data_da_baixa,
                                 origemA = g.Key.origem,
                                 origemB = g.Key.column1,
                                 g.Key.marca
                             }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                             {
                                 nome_cliente = x.nome_cliente,
                                 cpf_cnpj_cliente = x.cpf_cnpj_cliente,
                                 ddd_cliente_particular = x.ddd_cliente_particular,
                                 fone_cliente_particular = x.fone_cliente_particular,
                                 ddd_cliente_cml = x.ddd_cliente_cml,
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
                                 data_da_baixa = x.data_da_baixa,
                                 origem = x.origemA,
                                 origem_v = x.origemB,
                                 CCL_marca = x.marca
                             });
                }
            }

            if (model == null || model.Count() == 0)
            {
                return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaContrato");
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
