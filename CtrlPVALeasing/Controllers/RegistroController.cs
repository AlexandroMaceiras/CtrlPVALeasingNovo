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
    public class RegistroController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        IEnumerable<ContratosVeiculosViewModel> model = null;
        Tbl_DebitosEPagamentos_Veiculo model2 = null;

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

        // GET: Arm_LiquidadosEAtivos_Contrato/Details/5
        public ActionResult RegistroDebitoIPVAManual(string chassi, string placa, string renavam, string rd,
            DateTime? dta_cobranca, string uf_cobranca, string tipo_cobranca, decimal? valor_divida, 
            string ano_exercicio, string cda, decimal? valor_custas, bool? debito_protesto, 
            string nome_cartorio, bool? divida_ativa_serasa, bool? protesto_serasa,decimal? valor_debito_total)
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

                        where b.chassi.Contains(chassi)
                        where b.placa.Contains(placa)
                        where b.renavam.Contains(renavam)
                        where a.origem.Equals("B")
                        where !b.origem.Contains("RECIBO VEN")
                        select new
                        {
                            id                      = a.id,
                            contrato                = a.contrato,
                            tipo                    = a.tipo,
                            agencia                 = a.agencia,
                            dta_inicio_contrato     = a.dta_inicio_contrato,
                            dta_vecto_contrato      = a.dta_vecto_contrato,
                            origem                  = a.origem,
                            cpf_cnpj_cliente        = a.cpf_cnpj_cliente,
                            nome_cliente            = a.nome_cliente,
                            ddd_cliente_particular  = a.ddd_cliente_particular,
                            fone_cliente_particular = a.fone_cliente_particular,
                            rml_cliente_particular  = a.rml_cliente_particular,
                            end_cliente             = a.end_cliente,
                            bairro_cliente          = a.bairro_cliente,
                            cidade_cliente          = a.cidade_cliente,
                            uf_cliente              = a.uf_cliente,
                            cep_cliente             = a.cep_cliente,
                            filler                  = a.filler,
                            ddd_cliente_cml         = a.ddd_cliente_cml,
                            fone_cliente_cml        = a.fone_cliente_cml,
                            dta_ultimo_pagto        = a.dta_ultimo_pagto,
                            tipo_de_baixa           = a.tipo_de_baixa,
                            data_da_baixa           = a.data_da_baixa,
                            cod_empresa             = a.cod_empresa,
                            num_end_cliente         = a.num_end_cliente,
                            comp_end_cliente        = a.comp_end_cliente,
                            status                  = a.status,

                            contrato_v      = b.contrato,
                            tipo_registro   = b.tipo_registro,
                            marca           = b.marca,
                            modelo          = b.modelo,
                            tipo_v          = b.tipo,
                            ano_fab         = b.ano_fab,
                            ano_mod         = b.ano_mod,
                            cor             = b.cor,
                            renavam         = b.renavam,
                            chassi          = b.chassi,
                            placa           = b.placa,
                            origem_v        = b.origem,

                            id_debito           = c.id,
                            dta_cobranca        = c.dta_cobranca,
                            uf_cobranca         = c.uf_cobranca,
                            tipo_cobranca       = c.tipo_cobranca,
                            valor_divida        = c.valor_divida,
                            ano_exercicio       = c.ano_exercicio,
                            cda                 = c.cda,
                            valor_custas        = c.valor_custas,
                            debito_protesto     = c.debito_protesto,
                            nome_cartorio       = c.nome_cartorio,
                            divida_ativa_serasa = c.divida_ativa_serasa,
                            protesto_serasa     = c.protesto_serasa,
                            valor_debito_total  = c.valor_debito_total


                        }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                        {
                            id                      = x.id,
                            contrato                = x.contrato,
                            tipo                    = x.tipo,
                            agencia                 = x.agencia,
                            dta_inicio_contrato     = x.dta_inicio_contrato,
                            dta_vecto_contrato      = x.dta_vecto_contrato,
                            origem                  = x.origem,
                            cpf_cnpj_cliente        = x.cpf_cnpj_cliente,
                            nome_cliente            = x.nome_cliente,
                            ddd_cliente_particular  = x.ddd_cliente_particular,
                            fone_cliente_particular = x.fone_cliente_particular,
                            rml_cliente_particular  = x.rml_cliente_particular,
                            end_cliente             = x.end_cliente,
                            bairro_cliente          = x.bairro_cliente,
                            cidade_cliente          = x.cidade_cliente,
                            uf_cliente              = x.uf_cliente,
                            cep_cliente             = x.cep_cliente,
                            filler                  = x.filler,
                            ddd_cliente_cml         = x.ddd_cliente_cml,
                            fone_cliente_cml        = x.fone_cliente_cml,
                            dta_ultimo_pagto        = x.dta_ultimo_pagto,
                            tipo_de_baixa           = x.tipo_de_baixa,
                            data_da_baixa           = x.data_da_baixa,
                            cod_empresa             = x.cod_empresa,
                            num_end_cliente         = x.num_end_cliente,
                            comp_end_cliente        = x.comp_end_cliente,
                            status                  = x.status,

                            contrato_v      = x.contrato_v,
                            tipo_registro   = x.tipo_registro,
                            marca           = x.marca,
                            modelo          = x.modelo,
                            tipo_v          = x.tipo_v,
                            ano_fab         = x.ano_fab,
                            ano_mod         = x.ano_mod,
                            cor             = x.cor,
                            renavam         = x.renavam,
                            chassi          = x.chassi,
                            placa           = x.placa,
                            origem_v        = x.origem_v,

                            id_debito           = x.id_debito,
                            dta_cobranca        = x.dta_cobranca,
                            uf_cobranca         = x.uf_cobranca,
                            tipo_cobranca       = x.tipo_cobranca,
                            valor_divida        = x.valor_divida,
                            ano_exercicio       = x.ano_exercicio,
                            cda                 = x.cda,
                            valor_custas        = x.valor_custas,
                            debito_protesto     = x.debito_protesto,
                            nome_cartorio       = x.nome_cartorio,
                            divida_ativa_serasa = x.divida_ativa_serasa,
                            protesto_serasa     = x.protesto_serasa,
                            valor_debito_total  = x.valor_debito_total

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
                return RedirectToAction("RegistroDebitoIPVAManual");
            }

            if (Request.HttpMethod == "POST" && rd == "true")
            {
                
                // Controle de erros do ModelState
                //var errors = ModelState
                //.Where(x => x.Value.Errors.Count > 0)
                //.Select(x => new { x.Key, x.Value.Errors })
                //.ToArray();

                if (ModelState.IsValid)
                {

                    //string chassi,
                    //string placa,
                    //string renavam,
                    //string rd,
                    //DateTime? dta_cobranca, string uf_cobranca, string tipo_cobranca, decimal? valor_divida,
                    //string ano_exercicio, string cda, decimal? valor_custas, bool debito_protesto,
                    //string nome_cartorio, bool divida_ativa_serasa, bool protesto_serasa,decimal? valor_debito_total

                    model2 = new Tbl_DebitosEPagamentos_Veiculo
                    {
                        //id                  = (id_debito.HasValue ? id_debito.Value : 0), //Se não houver um id desse cara ele põe 0
                        chassi              = chassi,
                        renavam             = renavam,
                        placa               = placa,
                        dta_cobranca        = dta_cobranca,
                        uf_cobranca         = uf_cobranca,
                        tipo_cobranca       = tipo_cobranca,
                        valor_divida        = valor_divida,
                        ano_exercicio       = ano_exercicio,
                        cda                 = cda,
                        valor_custas        = valor_custas,
                        debito_protesto     = debito_protesto,
                        nome_cartorio       = nome_cartorio,
                        divida_ativa_serasa = divida_ativa_serasa,
                        protesto_serasa     = protesto_serasa,
                        valor_debito_total  = valor_debito_total
                    };

                    //if (id_comprador.HasValue || id_comprador != 0)
                    //{
                    //    db.Entry(model2).State = EntityState.Modified;
                    //    db.SaveChanges();
                    //}
                    //else 
                    if (db.Entry(model2).State == EntityState.Detached)
                    {
                        db.Tbl_DebitosEPagamentos_Veiculo.Add(model2);
                        db.SaveChanges();
                    }
                }
            }
            return View("RegistroDebitoIPVAManual", model);
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
