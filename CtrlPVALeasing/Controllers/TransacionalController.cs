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
    public class TransacionalController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        IEnumerable<ContratosVeiculosViewModel> model = null;
        Tbl_DadosDaVenda model2 = null;

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

        /// <summary>
        /// Cria um IEnumerable do modelo ContratosVeiculosViewModel com -1 para se injetar na view quando for retorno de pesquisa sem resposta dando erro.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ContratosVeiculosViewModel> GetErroDeVenda()
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();
            model.Add(new ContratosVeiculosViewModel() { id = -2, agencia = " " });
            return model;
        }

        public ActionResult Liquidados(DateTime? dataInicio, DateTime? dataFim, string uf_cliente, string impresso, string DUT, int? criterio)
        {
            if (dataInicio == null)
                dataInicio = null;
            if (dataFim == null)
                dataFim = null;

            if (dataInicio == null || dataFim == null)
            {
                return View(GetContratosVeiculosViewModelPrimeira());
            }

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                        join b in db.Arm_Veiculos
                        on a.contrato equals b.contrato

                        join c in db.Tbl_Dut
                        on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                        into j1
                        from c in j1.DefaultIfEmpty() //Isto é um LEFT JOIN

                        join d in db.Tbl_Impressao
                        on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                        into j2
                        from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN

                        where a.dta_inicio_contrato >= dataInicio
                        where a.dta_inicio_contrato <= dataFim

                        //where a.uf_cliente == uf_cliente

                        //where c.impresso == impresso

                        where a.status == false

                        where a.origem.Equals("B")
                        where !b.origem.Contains("RECIBO VEN")
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
                            comunicado_venda = b.comunicado_venda,

                            //id_debito = c.id,
                            chassi_dut = c.chassi,
                            renavam_dut = c.renavam,
                            placa_dut = c.placa,
                            tipo_impressao = d.tipo_impressao

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
                            comunicado_venda = x.comunicado_venda,

                            //id_debito = x.id_debito,
                            chassi_dut = x.chassi_dut,
                            renavam_dut = x.renavam_dut,
                            placa_dut = x.placa_dut,
                            tipo_impressao = x.tipo_impressao,
                            diferenca = (TimeSpan)(DateTime.Now - x.data_da_baixa),

                        }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);

            if (impresso.Trim() == "N")
            {
                //Não impresso
                model = model
                    .Where(x => x.tipo_impressao == "" || x.tipo_impressao == null); //Não está na tabela Tbl_Impressao

                if (model.Count() > 0)
                {

                    if (DUT.Trim() == "C")
                    {
                        //Com DUT
                        model = model
                            .Where(x => x.chassi.ToString().Trim() == x.chassi_dut)
                            .Where(x => x.renavam.ToString().Trim() == x.renavam_dut)
                            .Where(x => x.placa.ToString().Trim() == x.placa_dut);
                    }
                    else if (DUT.Trim() == "S")
                    {
                        //Com DUT
                        model = model
                            .Where(x => x.chassi.ToString().Trim() != x.chassi_dut)
                            .Where(x => x.renavam.ToString().Trim() != x.renavam_dut)
                            .Where(x => x.placa.ToString().Trim() != x.placa_dut);
                    }
                    else if (DUT.Trim() == ">")
                    {
                        //Com DUT
                        model = model
                            .Where(x => x.chassi.ToString().Trim() != x.chassi_dut)
                            .Where(x => x.renavam.ToString().Trim() != x.renavam_dut)
                            .Where(x => x.placa.ToString().Trim() != x.placa_dut);

                        // > Critério
                        model = model
                            .Where(x => x.diferenca.Days > criterio);
                    }
                }
            }
            else if (impresso.Trim() == "J")
            {
                //Já Impresso
                model = model
                    .Where(x => x.tipo_impressao == DUT);

                if (model.Count() > 0)
                {
                    if (DUT.Trim() == "C")
                    {
                        //com DUT
                        model = model
                            .Where(x => x.chassi.ToString().Trim() == x.chassi_dut)
                            .Where(x => x.renavam.ToString().Trim() == x.renavam_dut)
                            .Where(x => x.placa.ToString().Trim() == x.placa_dut);
                    }
                    else if (DUT.Trim() == "S")
                    {
                        //Com DUT
                        model = model
                            .Where(x => x.chassi.ToString().Trim() != x.chassi_dut)
                            .Where(x => x.renavam.ToString().Trim() != x.renavam_dut)
                            .Where(x => x.placa.ToString().Trim() != x.placa_dut);
                    }
                    else if (DUT.Trim() == ">")
                    {
                        //Com DUT
                        model = model
                            .Where(x => x.chassi.ToString().Trim() != x.chassi_dut)
                            .Where(x => x.renavam.ToString().Trim() != x.renavam_dut)
                            .Where(x => x.placa.ToString().Trim() != x.placa_dut);

                        // > Critério
                        model = model
                            .Where(x => x.diferenca.Days > criterio);
                    }
                }
            }
            else if (impresso.Trim() == "T")
            {

                if (DUT.Trim() == "C")
                {
                    //com DUT
                    model = model
                        .Where(x => x.chassi.ToString().Trim() == x.chassi_dut)
                        .Where(x => x.renavam.ToString().Trim() == x.renavam_dut)
                        .Where(x => x.placa.ToString().Trim() == x.placa_dut);
                }
                else if (DUT.Trim() == "S")
                {
                    //Com DUT
                    model = model
                        .Where(x => x.chassi.ToString().Trim() != x.chassi_dut)
                        .Where(x => x.renavam.ToString().Trim() != x.renavam_dut)
                        .Where(x => x.placa.ToString().Trim() != x.placa_dut);
                }
                else if (DUT.Trim() == ">")
                {
                    //Com DUT
                    model = model
                        .Where(x => x.chassi.ToString().Trim() != x.chassi_dut)
                        .Where(x => x.renavam.ToString().Trim() != x.renavam_dut)
                        .Where(x => x.placa.ToString().Trim() != x.placa_dut);

                    // > Critério
                    model = model
                        .Where(x => x.diferenca.Days > criterio);
                }
            }

            model = (uf_cliente.Trim() == "SP" ? model.Where(x => x.uf_cliente == uf_cliente) : model.Where(x => x.uf_cliente != "SP" || x.uf_cliente == null));

            try
            {
                if (model.Count() == 0 || model == null)
                {
                    return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaVeiculo");
                }

                if (model == null || model.Any() == false)
                {
                    //return HttpNotFound();
                    return RedirectToAction("Liquidados");
                }
            }
            catch
            {
                return View(GetContratosVeiculosViewModelErro());
            }
            return View("Liquidados", model);
        }

        // GET: Arm_LiquidadosEAtivos_Contrato/Details/5
        public ActionResult ImpressaoDUTAvulso(string chassi, string placa, string renavam,
            string nome_cliente, string ic, string ac, int? id_comprador, string nome_comprador, string cpf_cnpj_comprador, 
            string local_comprador, string rg_comprador, DateTime? dta_da_compra, string end_comprador, decimal? valor_da_compra)
        {
            try
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

                         join d in db.Tbl_DadosDaVenda
                         on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                         into j2
                         from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN

                         join e in db.Tbl_Bens
                         on new { b.chassi, b.renavam, b.placa } equals new { e.chassi, e.renavam, e.placa }
                         into j3
                         from e in j3.DefaultIfEmpty() //Isto é um LEFT JOIN

                         where b.chassi.Contains(chassi)
                         where b.placa.Contains(placa)
                         where b.renavam.Contains(renavam)
                         where a.origem.Equals("B")
                         where !b.origem.Contains("RECIBO VEN")
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
                             comunicado_venda   = b.comunicado_venda,

                             dta_cobranca           = c.dta_cobranca,
                             valor_debito_total     = c.valor_debito_total,
                             dta_pagamento          = c.dta_pagamento,
                             valor_pago_total       = c.valor_pago_total,
                             divida_ativa_serasa    = c.divida_ativa_serasa,

                             //id_comprador    = d.id,
                             nome_comprador     = d.nome_comprador,
                             cpf_cnpj_comprador = d.cpf_cnpj_comprador,
                             rg_comprador       = d.rg_comprador,
                             local_comprador    = d.local_comprador,
                             end_comprador      = d.end_comprador,
                             dta_da_compra      = d.dta_da_compra,
                             valor_da_compra    = d.valor_da_compra,

                             renavam_bens   = e.renavam,
                             chassi_bens    = e.chassi,
                             placa_bens     = e.placa

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
                             comunicado_venda   = x.comunicado_venda,

                             dta_cobranca           = x.dta_cobranca,
                             valor_debito_total     = x.valor_debito_total,
                             dta_pagamento          = x.dta_pagamento,
                             valor_pago_total       = x.valor_pago_total,
                             divida_ativa_serasa    = x.divida_ativa_serasa,

                             //id_comprador     = x.id_comprador,
                             nome_comprador     = x.nome_comprador,
                             cpf_cnpj_comprador = x.cpf_cnpj_comprador,
                             rg_comprador       = x.rg_comprador,
                             local_comprador    = x.local_comprador,
                             end_comprador      = x.end_comprador,
                             dta_da_compra      = x.dta_da_compra,
                             valor_da_compra    = x.valor_da_compra,

                             renavam_bens   = x.renavam,
                             chassi_bens    = x.chassi,
                             placa_bens     = x.placa

                         });

                try //A única maneira de contornar um erro no model.Count() quando se entra com um sequencia numérica no "where b.chassi.Contains(chassi)" do model, se for "where b.chassi.Equals(chassi)" não dá pau!
                {
                    if (model.Count() == 0 || model == null)
                    {
                        return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaVeiculo");
                    }
                }
                catch
                {
                    return View(GetContratosVeiculosViewModelErro());
                }

                if (model == null || model.Any() == false)
                {
                    //return HttpNotFound();
                    return RedirectToAction("ImpressaoDUTAvulso");
                }

                if (Request.HttpMethod == "POST" && ic == "true")
                {
                    // Controle de erros do ModelState
                    //var errors = ModelState
                    //.Where(x => x.Value.Errors.Count > 0)
                    //.Select(x => new { x.Key, x.Value.Errors })
                    //.ToArray();

                    if (ModelState.IsValid)
                    {
                        var procuraRegistro = db.Tbl_DadosDaVenda
                            .FirstOrDefault(c => c.chassi == chassi || c.renavam == renavam || c.placa == placa);

                        if (procuraRegistro != null)
                        {
                            procuraRegistro.chassi              = chassi;
                            procuraRegistro.renavam             = renavam;
                            procuraRegistro.placa               = placa;
                            procuraRegistro.cpf_cnpj_comprador  = cpf_cnpj_comprador;
                            procuraRegistro.dta_da_compra       = dta_da_compra;
                            procuraRegistro.end_comprador       = end_comprador;
                            procuraRegistro.local_comprador     = local_comprador;
                            procuraRegistro.nome_comprador      = nome_comprador;
                            procuraRegistro.rg_comprador        = rg_comprador;
                            procuraRegistro.valor_da_compra     = valor_da_compra;

                            db.Entry(procuraRegistro).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            model2 = new Tbl_DadosDaVenda
                            {
                                id                  = 0,
                                chassi              = chassi,
                                renavam             = renavam,
                                placa               = placa,
                                cpf_cnpj_comprador  = cpf_cnpj_comprador,
                                dta_da_compra       = dta_da_compra,
                                end_comprador       = end_comprador,
                                local_comprador     = local_comprador,
                                nome_comprador      = nome_comprador,
                                rg_comprador        = rg_comprador,
                                valor_da_compra     = valor_da_compra
                            };

                            if (db.Entry(model2).State == EntityState.Detached)
                            {
                                db.Tbl_DadosDaVenda.Add(model2);
                                db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        return View(GetErroDeVenda());
                    }
                }
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('" + e.Message + "');</script>");
            };

            return View("ImpressaoDUTAvulso", model);
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
