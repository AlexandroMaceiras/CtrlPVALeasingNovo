using System;
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
    public class GerencialController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        IEnumerable<ContratosVeiculosViewModel> model = null;

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
            model.Add(new ContratosVeiculosViewModel() { id = 0, agencia = " " });
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

        public ActionResult Menu()
        {
            return View();
        }

        public ActionResult PagamentoIPVAPorStatusContrato_ANTIGO()
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato
                     join c in db.Tbl_DebitosEPagamentos_Veiculo
                     on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                     where a.origem.Equals("B")
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
                         uf_cobranca = c.uf_cobranca,
                         valor_pago_divida = c.valor_pago_divida,
                         valor_pago_custas = c.valor_pago_custas

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
                         uf_cobranca = x.uf_cobranca,
                         valor_pago_divida = x.valor_pago_divida,
                         valor_pago_custas = x.valor_pago_custas
                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);

            if (model.Count() == 0 || model == null)
            {
                return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaVeiculo");
            }

            if (model == null || model.Any() == false)
            {
                //return HttpNotFound();
                return RedirectToAction("PagamentoIPVAPorStatusContrato");
            }
            return View("PagamentoIPVAPorStatusContrato", model);
        }

        public ActionResult PagamentoIPVAPorStatusContrato(string escolha)
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato

                     from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv =>
                     (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa))

                     where a.origem.Equals("B")
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
                         dta_custas = c.dta_custas,
                         uf_pagamento = c.uf_pagamento,
                         valor_divida = c.valor_divida,
                         ano_exercicio = c.ano_exercicio,
                         valor_custas = c.valor_custas,
                         pagamento_efet_banco = c.pagamento_efet_banco,
                         valor_recuperado = c.valor_recuperado,
                         valor_total_recuperado = c.valor_total_recuperado,
                         divida_ativa_serasa = c.divida_ativa_serasa,
                         uf_cobranca = c.uf_cobranca,
                         valor_pago_divida = c.valor_pago_divida,
                         valor_pago_custas = c.valor_pago_custas,
                         tipo_cobranca = c.tipo_cobranca,
                         cda = c.cda,
                         debito_protesto = c.debito_protesto,
                         nome_cartorio = c.nome_cartorio,
                         protesto_serasa = c.protesto_serasa,
                         forma_pagamento_divida = c.forma_pagamento_divida,
                         forma_pagamento_custas = c.forma_pagamento_custas,
                         numero_miro_divida = c.numero_miro_divida,
                         numero_miro_custa = c.numero_miro_custa,
                         obs_pagamento = c.obs_pagamento,
                         valor_pago_total = c.valor_pago_total,
                         dta_pagamento_custas = c.dta_pagamento_custas,
                         dta_recuperacao = c.dta_recuperacao,
                         pci_debito_divida = c.pci_debito_divida,
                         pci_debito_custa = c.pci_debito_custa,
                         pci_credito = c.pci_credito,
                         grupo_safra = c.grupo_safra,
                         status_recuperacao = c.status_recuperacao


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
                         dta_custas = x.dta_custas,
                         uf_pagamento = x.uf_pagamento,
                         valor_divida = x.valor_divida,
                         ano_exercicio = x.ano_exercicio,
                         valor_custas = x.valor_custas,
                         pagamento_efet_banco = x.pagamento_efet_banco,
                         valor_recuperado = x.valor_recuperado,
                         valor_total_recuperado = x.valor_total_recuperado,
                         divida_ativa_serasa = x.divida_ativa_serasa,
                         uf_cobranca = x.uf_cobranca,
                         valor_pago_divida = x.valor_pago_divida,
                         valor_pago_custas = x.valor_pago_custas,
                         tipo_cobranca = x.tipo_cobranca,
                         cda = x.cda,
                         debito_protesto = x.debito_protesto,
                         nome_cartorio = x.nome_cartorio,
                         protesto_serasa = x.protesto_serasa,
                         forma_pagamento_divida = x.forma_pagamento_divida,
                         forma_pagamento_custas = x.forma_pagamento_custas,
                         numero_miro_divida = x.numero_miro_divida,
                         numero_miro_custa = x.numero_miro_custa,
                         obs_pagamento = x.obs_pagamento,
                         valor_pago_total = x.valor_pago_total,
                         dta_pagamento_custas = x.dta_pagamento_custas,
                         dta_recuperacao = x.dta_recuperacao,
                         pci_debito_divida = x.pci_debito_divida,
                         pci_debito_custa = x.pci_debito_custa,
                         pci_credito = x.pci_credito,
                         grupo_safra = x.grupo_safra,
                         status_recuperacao = x.status_recuperacao

                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);

            //Foi comentado pois o Bruno pediu para que gerasse um csv mesmo que estivesse vazio.

            //if (model.Count() == 0 || model == null)
            //{
            //    return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaVeiculo");
            //}

            //if (model == null || model.Any() == false)
            //{
            //    //return HttpNotFound();
            //    return RedirectToAction("PagamentoIPVAPorStatusContrato");
            //}

            if (escolha == "gcvs")
            {
                //Gera Resultado em CSV.
                StringWriter sw = new StringWriter();

                sw.WriteLine(
                    "Nº;" +
                    "Contrato;" +
                    "Chassi;" +
                    "Renavam;" +
                    "Placa;" +
                    "Nome Cliente;" +
                    "CPF/CNPJ;" +


                    "UF de Cobranca;" +
                    "Tipo de Cobranca (S,D ou C);" +
                    "Ano de Exercicio;" +

                    "CDA;" +
                    "Debito de Protesto;" +
                    "Nome do Cartorio;" +
                    "Divida Ativa no Serasa?;" +
                    "Protesto no Serasa;" +

                    "Pagamento Efetuado no Banco;" +
                    "Data da Dívida;" +
                    "Data de Pagamento Dívida;" +
                    "Data das Custas;" +
                    "Data Pagamento Custas;" +
                    "Data Recuperação;" +

                    "Número Miro da Dívida;" +
                    "Numero Miro das Custas;" +
                    "Forma de Pagamento da Divida;" +
                    "Forma de Pagamento de Custas;" +
                    "UF de Pagamento;" +




                    "Valor de Custas;" +
                    "Valor Pago Custas;" +
                    "Valor da Divida;" +
                    "Valor Pago da Divida;" +


                    "Valor do Debito Total;" +
                    "Valor Pago Total;" +
                    "Valor Recuperado;" +
                    "Valor Total Recuperado;" +

                    "Obs de Pagamento;" +
                    "Pci débito Dívida;" +
                    "Pci Débito Custa;" +
                    "Pci Crédito;" +
                    "Grupo Safra;" +
                    "Status;" +
                    "Status da Recuperação;" +
                    "Origem"
                    );

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=PagamentoIPVAPorStatusContrato.csv");
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = System.Text.Encoding.Default;

                var contador = 1;

                foreach (var elemento in model)
                {
                    sw.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25};{26};{27};{28};{29};{30};{31};{32};{33};{34};{35};{36};{37};{38};{39};{40};{41}",

                    contador++,
                    elemento.contrato,
                    elemento.chassi,
                    elemento.renavam,
                    elemento.placa,
                    elemento.nome_cliente,
                    elemento.cpf_cnpj_cliente,


                    elemento.uf_cobranca,
                    elemento.tipo_cobranca,
                    elemento.ano_exercicio,

                    elemento.cda,
                    elemento.debito_protesto.HasValue ? (elemento.debito_protesto == true ? "Sim" : "Não") : "Não",
                    elemento.nome_cartorio,
                    elemento.divida_ativa_serasa.HasValue ? (elemento.divida_ativa_serasa == true ? "Sim" : "Não") : "Não",
                    elemento.protesto_serasa.HasValue ? (elemento.protesto_serasa == true ? "Sim" : "Não") : "Não",

                    elemento.pagamento_efet_banco.HasValue ? (elemento.pagamento_efet_banco == true ? "Sim" : "Não") : "Não",
                    (elemento.dta_cobranca.HasValue ? elemento.dta_cobranca.ToString().Substring(0, 10) : ""),
                    (elemento.dta_pagamento_divida.HasValue ? elemento.dta_pagamento_divida.ToString().Substring(0, 10) : ""),
                    (elemento.dta_custas.HasValue ? elemento.dta_custas.ToString().Substring(0, 10) : ""),
                    (elemento.dta_pagamento_custas.HasValue ? elemento.dta_pagamento_custas.ToString().Substring(0, 10) : ""),
                    (elemento.dta_recuperacao.HasValue ? elemento.dta_recuperacao.ToString().Substring(0, 10) : ""),
                    elemento.numero_miro_divida,
                    elemento.numero_miro_custa,
                    elemento.forma_pagamento_divida,
                    elemento.forma_pagamento_custas,
                    elemento.uf_pagamento,





                    elemento.valor_custas,
                    elemento.valor_pago_custas,
                    elemento.valor_divida,
                    elemento.valor_pago_divida,


                    elemento.valor_debito_total,
                    (elemento.valor_pago_custas.HasValue ? elemento.valor_pago_custas : 0) + (elemento.valor_pago_divida.HasValue ? elemento.valor_pago_divida : 0),
                    elemento.valor_recuperado,
                    elemento.valor_total_recuperado,

                    elemento.obs_pagamento,
                    elemento.pci_debito_divida,
                    elemento.pci_debito_custa,
                    elemento.pci_credito,
                    elemento.grupo_safra,
                    elemento.status.HasValue ? (elemento.status == true ? "Ativo" : "liquidado") : "liquidado",
                    elemento.status_recuperacao,
                    elemento.origem
                    ));
                }
                Response.Write(sw.ToString());
                Response.End();
            }

            return View("PagamentoIPVAPorStatusContrato", model);
        }

        public ActionResult ExportarTodaBaseArm()
        {
            try
            {
                model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                         from b in db.Arm_Veiculos.Where(Tudo => Tudo.contrato == a.contrato).DefaultIfEmpty()
                         select new
                         {
                             id = a.id,
                             cpf_cnpj_cliente = a.cpf_cnpj_cliente,
                             nome_cliente = a.nome_cliente,
                             contrato = a.contrato,
                             origem = a.origem,
                             status = a.status,

                             renavam = b.renavam,
                             chassi = b.chassi,
                             placa = b.placa

                         }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                         {
                             id = x.id,
                             cpf_cnpj_cliente = x.cpf_cnpj_cliente,
                             nome_cliente = x.nome_cliente,
                             contrato = x.contrato,
                             origem = x.origem,
                             status = x.status,
                             renavam = x.renavam,
                             chassi = x.chassi,
                             placa = x.placa

                         }).OrderBy(x => x.contrato); //.OrderByDescending(x => x.dta_cobranca);


                StringWriter sw = new StringWriter();

                sw.WriteLine(
                    "Nº;" +
                    "CPF/CNPJ;" +
                    "Nome/Razão Social;" +
                    "Contrato;" +
                    "Chassi;" +
                    "Renavam;" +
                    "Placa;" +
                    "Origem;" +
                    "Status;"
                    );

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=PagamentoIPVAPorStatusContrato.csv");
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = System.Text.Encoding.Default;

                var contador = 1;

                foreach (var elemento in model)
                {
                    sw.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};",

                    contador++,
                    elemento.cpf_cnpj_cliente,
                    elemento.nome_cliente,
                    elemento.contrato,
                    elemento.chassi,
                    elemento.renavam,
                    elemento.placa,
                    elemento.origem,
                    elemento.status.HasValue ? (elemento.status == true ? "Ativo" : "liquidado") : "liquidado"
                    ));
                }
                Response.Write(sw.ToString());
                Response.End();

                return View("Menu", model);
            }
            catch(Exception e)
            {   
                return View();
            }
        }


        public ActionResult _valor_total_recuperado(string cpf_cnpj_cliente)
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
                     where a.cpf_cnpj_cliente.Contains(cpf_cnpj_cliente)
                     where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
                     //where a.status.Equals(1)
                     join c in db.Tbl_DebitosEPagamentos_Veiculo
                    on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
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

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
            join b in db.Arm_Veiculos
            on a.contrato equals b.contrato
            where a.contrato.Contains(contrato)
            where a.cpf_cnpj_cliente.Contains(cpf_cnpj_cliente)
            where a.origem.Equals("B")
            where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
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

                     join c in db.Tbl_DebitosEPagamentos_Veiculo
                     on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                     into j1
                     from c in j1.DefaultIfEmpty() //Isto é um LEFT JOIN

                     join d in db.Tbl_Bens
                     on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                     into j2
                     from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN

                     where b.chassi.Contains(chassi) 
                     where b.placa.Contains(placa)
                     where b.renavam.Contains(renavam)
                     where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
                     select new
                     {
                         id                         = a.id,
                         contrato                   = a.contrato,
                         tipo                       = a.tipo,
                         agencia                    = a.agencia,
                         dta_inicio_contrato        = a.dta_inicio_contrato,
                         dta_vecto_contrato         = a.dta_vecto_contrato,
                         origem                     = a.origem,
                         cpf_cnpj_cliente           = a.cpf_cnpj_cliente,
                         nome_cliente               = a.nome_cliente,
                         ddd_cliente_particular     = a.ddd_cliente_particular,
                         fone_cliente_particular    = a.fone_cliente_particular,
                         rml_cliente_particular     = a.rml_cliente_particular,
                         end_cliente                = a.end_cliente,
                         bairro_cliente             = a.bairro_cliente,
                         cidade_cliente             = a.cidade_cliente,
                         uf_cliente                 = a.uf_cliente,
                         cep_cliente                = a.cep_cliente,
                         filler                     = a.filler,
                         ddd_cliente_cml            = a.ddd_cliente_cml,
                         fone_cliente_cml           = a.fone_cliente_cml,
                         dta_ultimo_pagto           = a.dta_ultimo_pagto,
                         tipo_de_baixa              = a.tipo_de_baixa,
                         data_da_baixa              = a.data_da_baixa,
                         cod_empresa                = a.cod_empresa,
                         num_end_cliente            = a.num_end_cliente,
                         comp_end_cliente           = a.comp_end_cliente,
                         status                     = a.status,

                         contrato_v         = b.contrato,
                         tipo_registro      = b.tipo_registro,
                         marca              = b.marca,
                         modelo             = b.modelo,
                         tipo_v             = b.tipo,
                         ano_fab            = b.ano_fab,
                         ano_mod            = b.ano_mod,
                         cor                = b.cor,
                         renavam            = b.renavam,
                         chassi             = b.chassi,
                         placa              = b.placa,
                         origem_v           = b.origem,

                         dta_cobranca           = c.dta_cobranca,
                         valor_debito_total     = c.valor_debito_total,
                         dta_pagamento_divida          = c.dta_pagamento_divida,
                         valor_pago_total       = c.valor_pago_total,
                         divida_ativa_serasa    = c.divida_ativa_serasa,

                         renavam_bens   = d.renavam,
                         chassi_bens    = d.chassi,
                         placa_bens     = d.placa

                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         id                         = x.id,
                         contrato                   = x.contrato,
                         tipo                       = x.tipo,
                         agencia                    = x.agencia,
                         dta_inicio_contrato        = x.dta_inicio_contrato,
                         dta_vecto_contrato         = x.dta_vecto_contrato,
                         origem                     = x.origem,
                         cpf_cnpj_cliente           = x.cpf_cnpj_cliente,
                         nome_cliente               = x.nome_cliente,
                         ddd_cliente_particular     = x.ddd_cliente_particular,
                         fone_cliente_particular    = x.fone_cliente_particular,
                         rml_cliente_particular     = x.rml_cliente_particular,
                         end_cliente                = x.end_cliente,
                         bairro_cliente             = x.bairro_cliente,
                         cidade_cliente             = x.cidade_cliente,
                         uf_cliente                 = x.uf_cliente,
                         cep_cliente                = x.cep_cliente,
                         filler                     = x.filler,
                         ddd_cliente_cml            = x.ddd_cliente_cml,
                         fone_cliente_cml           = x.fone_cliente_cml,
                         dta_ultimo_pagto           = x.dta_ultimo_pagto,
                         tipo_de_baixa              = x.tipo_de_baixa,
                         data_da_baixa              = x.data_da_baixa,
                         cod_empresa                = x.cod_empresa,
                         num_end_cliente            = x.num_end_cliente,
                         comp_end_cliente           = x.comp_end_cliente,
                         status                     = x.status,

                         contrato_v         = x.contrato_v,
                         tipo_registro      = x.tipo_registro,
                         marca              = x.marca,
                         modelo             = x.modelo,
                         tipo_v             = x.tipo_v,
                         ano_fab            = x.ano_fab,
                         ano_mod            = x.ano_mod,
                         cor                = x.cor,
                         renavam            = x.renavam,
                         chassi             = x.chassi,
                         placa              = x.placa,
                         origem_v           = x.origem_v,

                         dta_cobranca           = x.dta_cobranca,
                         valor_debito_total     = x.valor_debito_total,
                         dta_pagamento_divida          = x.dta_pagamento_divida,
                         valor_pago_total       = x.valor_pago_total,
                         divida_ativa_serasa    = x.divida_ativa_serasa,

                         renavam_bens   = x.renavam_bens,
                         chassi_bens    = x.chassi_bens,
                         placa_bens     = x.placa_bens
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
                       a.cpf_cnpj_cliente.Contains(cpf_cnpj_cliente) && //Não estou usando o cpf_cnpj_clienteZEROS porque decidiu-se que aqui não deveria-se fazer uma pesquisa exata para podermos pesquisar por RAIZ de CNPJs de uma mesma empresa.
                       a.origem == "B" &&
                       (!b.origem.Contains("RECIBO VEN") || b.origem == null)
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
                      orderby g.Key.cpf_cnpj_cliente,
                              g.Key.contrato,
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
                return RedirectToAction("ConsultaCliente");
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
