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
    public class RegistroController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        IEnumerable<ContratosVeiculosViewModel> model = null;
        Tbl_DebitosEPagamentos_Veiculo model2 = null;
        Tbl_Dut model3 = null;

        /// <summary>
        /// Cria um IEnumerable do modelo ContratosVeiculosViewModel com -3 para se injetar na view quando for retorno de pesquisa após inclusão.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ContratosVeiculosViewModel> GetContratosVeiculosViewModelRegistroOk()
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();
            model.Add(new ContratosVeiculosViewModel() { id = -3, agencia = " " });
            return model;
        }
        private IEnumerable<ContratosVeiculosViewModel> GetContratosVeiculosViewModelAtualizaRegistroOk()
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();
            model.Add(new ContratosVeiculosViewModel() { id = -2, agencia = " " });
            return model;
        }

        private IEnumerable<ContratosVeiculosViewModel> RecuperacaoOk()
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();
            model.Add(new ContratosVeiculosViewModel() { id = -12, agencia = " " });
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
        /// Cria um IEnumerable do modelo ContratosVeiculosViewModel com -2 para se injetar na view quando for retorno de pesquisa sem resposta dando erro.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ContratosVeiculosViewModel> GetContratosVeiculosViewModelErroRegistroDebito()
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();
            model.Add(new ContratosVeiculosViewModel() { id = -2, agencia = " " });
            return model;
        }

        /// <summary>
        /// Cria um IEnumerable do modelo ContratosVeiculosViewModel com -4 para se injetar na view quando for retorno de pesquisa sem resposta dando erro.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ContratosVeiculosViewModel> GetErroDeEntrada(ContratosVeiculosViewModel mmm)
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();

            if (mmm != null)
                model.Add(new ContratosVeiculosViewModel() { id = -4, agencia = " ", chassi = mmm.chassi, renavam = mmm.renavam, placa = mmm.placa, marca = mmm.marca, modelo = mmm.modelo, tipo_v = mmm.tipo_v, ano_fab = mmm.ano_fab, ano_mod = mmm.ano_mod, cor = mmm.cor });
            else
                model.Add(new ContratosVeiculosViewModel() { id = -4, agencia = " " });

            return model;
        }

        private IEnumerable<ContratosVeiculosViewModel> GetErroDeEntradaDesconhecido()
        {
            List<ContratosVeiculosViewModel> model = new List<ContratosVeiculosViewModel>();
            model.Add(new ContratosVeiculosViewModel() { id = -6, agencia = " " });
            return model;
        }

        public ActionResult UploadDebitoIPVA()
        {
            return View(GetContratosVeiculosViewModelPrimeira());
        }

        public ActionResult Teste()
        {
            int milliseconds = 10000;
            System.Threading.Thread.Sleep(milliseconds);
            return View();
        }

        public ActionResult Teste2()
        {
            System.Threading.Thread.Sleep(5000);
            return View();
        }


        public ActionResult RegistroContrato(string contrato, string cpf_cnpj_cliente)
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

                     join c in db.Tbl_Dut
                     on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                     into j1
                     from c in j1.DefaultIfEmpty() //Isto é um LEFT JOIN

                     where a.contrato.Contains(contrato)
                     where a.cpf_cnpj_cliente.Contains(cpf_cnpj_cliente)
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

                         comDUT = c.comDUT,
                         comVenda = c.comVenda

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
                         comVenda = x.comVenda

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
        public ActionResult RegistroVeiculo(string chassi, string placa, string renavam)
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

                     join e in db.Tbl_Dut
                     on new { b.chassi, b.renavam, b.placa } equals new { e.chassi, e.renavam, e.placa }
                     into j3
                     from e in j3.DefaultIfEmpty() //Isto é um LEFT JOIN

                     where b.chassi.Contains(chassi) 
                     where b.placa.Contains(placa) 
                     where b.renavam.Contains(renavam)
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

                         dta_cobranca = c.dta_cobranca,
                         valor_debito_total = c.valor_debito_total,
                         dta_pagamento_divida = c.dta_pagamento_divida,
                         valor_pago_total = c.valor_pago_total,
                         divida_ativa_serasa = c.divida_ativa_serasa,

                         renavam_bens = d.renavam,
                         chassi_bens = d.chassi,
                         placa_bens = d.placa,

                         comVenda = e.comVenda

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

                         comVenda = x.comVenda
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


        [HttpPost]
        public ActionResult UploadDebitoIPVA(string Upload1, HttpPostedFileBase Upload2)
        {
            int LinhasPular = 2; //Quantidade de linhas à pular no cabeçalho.

            //if (Upload1 != null && Upload1.Length > 0) //Upload com path completo sendo informado pelo usuário e leitura direta do arquivo.
            //{

            //}

            if (Upload2 != null && Upload2.ContentLength > 0) //Upload com browse de arquivo e armazenamentto do arquivo no servidor para depois ser lido.
            {
                var NomeArquivo = Path.GetFileName(Upload2.FileName);

                Upload2.SaveAs(Server.MapPath("../Upload/" + NomeArquivo));

                IEnumerable<Tbl_DebitosEPagamentos_Veiculo> ModelUpload = System.IO.File.ReadAllLines(Server.MapPath("../Upload/" + NomeArquivo), System.Text.Encoding.Default)
                .Skip(LinhasPular)
                .Select(x => x.Split(';'))
                .Select(x => new
                {
                    chassi = x[0],
                    renavam = x[1],
                    placa = x[2],
                    dta_cobranca = x[3],
                    uf_cobranca = x[4],
                    tipo_cobranca = x[5],
                    valor_divida = x[6],
                    ano_exercicio = x[7],
                    cda = x[8],
                    valor_custas = x[9],
                    debito_protesto = x[10],
                    nome_cartorio = x[11],
                    divida_ativa_serasa = x[12],
                    protesto_serasa = x[13],
                    valor_debito_total = x[14],
                    dta_custas = x[15],

                }).AsEnumerable().Select(a => new Tbl_DebitosEPagamentos_Veiculo
                {
                    id = 0,
                    chassi = (a.chassi.Trim() == "null" || a.chassi.Trim() == "NULL" ? null : a.chassi.Trim()),
                    renavam = (a.renavam.Trim() == "null" || a.renavam.Trim() == "NULL" ? null : a.renavam.Trim()),
                    placa = (a.placa.Trim() == "null" || a.placa.Trim() == "NULL" ? null : a.placa.Trim()),
                    dta_cobranca = (a.dta_cobranca.Trim() == "" || a.dta_cobranca.Trim() == "null" || a.dta_cobranca.Trim() == "NULL" ? (DateTime?)null : DateTime.Parse(a.dta_cobranca)),
                    uf_cobranca = (a.uf_cobranca.Trim() == "null" || a.uf_cobranca.Trim() == "NULL" ? null : a.uf_cobranca.Trim()),
                    tipo_cobranca = (a.tipo_cobranca.Trim() == "null" || a.tipo_cobranca.Trim() == "NULL" ? null : a.tipo_cobranca.Trim()),
                    valor_divida = (a.valor_divida.Trim() == "" || a.valor_divida.Trim() == "null" || a.valor_divida.Trim() == "NULL" ? (decimal?)null : decimal.Parse(a.valor_divida)),
                    ano_exercicio = (a.ano_exercicio.Trim() == "null" || a.ano_exercicio.Trim() == "NULL" ? null : a.ano_exercicio.Trim()),
                    cda = (a.cda.Trim() == "null" || a.cda.Trim() == "NULL" ? null : a.cda.Trim()),
                    valor_custas = (a.valor_custas.Trim() == "" || a.valor_custas.Trim() == "null" || a.valor_custas.Trim() == "NULL" ? (decimal?)null : decimal.Parse(a.valor_custas)),
                    debito_protesto = (a.debito_protesto.Trim() == "1" ? true : false),
                    nome_cartorio = (a.nome_cartorio.Trim() == "null" || a.nome_cartorio.Trim() == "NULL" ? null : a.nome_cartorio.Trim()),
                    divida_ativa_serasa = (a.divida_ativa_serasa.Trim() == "1" ? true : false),
                    protesto_serasa = (a.protesto_serasa.Trim() == "1" ? true : false),
                    valor_debito_total = (a.valor_debito_total.Trim() == "" || a.valor_debito_total.Trim() == "null" || a.valor_debito_total.Trim() == "NULL" ? (decimal?)null : decimal.Parse(a.valor_debito_total)),
                    dta_custas = (a.dta_custas.Trim() == "" || a.dta_custas.Trim() == "null" || a.dta_custas.Trim() == "NULL" ? (DateTime?)null : DateTime.Parse(a.dta_custas)),

                });
                bool salvouAlgum = false;
                try
                {
                    if (ModelState.IsValid)
                    {
                        foreach (var item in ModelUpload)
                        {
                            if (db.Entry(item).State == EntityState.Detached)
                            {
                                db.Tbl_DebitosEPagamentos_Veiculo.Add(item);
                                db.SaveChanges();
                                salvouAlgum = true;
                            }
                        }
                        if (salvouAlgum)
                            return View(GetContratosVeiculosViewModelRegistroOk());
                        if (!salvouAlgum)
                            return View(GetContratosVeiculosViewModelErro());

                    }
                    else
                    {
                        return View(GetErroDeEntrada(null));
                    }
                }
                catch
                {
                    return View(GetErroDeEntradaDesconhecido());
                }
            }
            return View(GetContratosVeiculosViewModelPrimeira());
        }

        // GET: Arm_LiquidadosEAtivos_Contrato/Details/5
        public ActionResult RegistroDUT(string chassi, string placa, string renavam, string chassiPesquisado, string placaPesquisada, string renavamPesquisado, string escolha, bool? comDUT, bool? comVenda)
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

                     join c in db.Tbl_Dut
                     on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                     into j1
                     from c in j1.DefaultIfEmpty() //Isto é um LEFT JOIN

                     join d in db.Tbl_SCC
                     on new { a.cpf_cnpj_cliente } equals new { d.cpf_cnpj_cliente }
                     into j2
                     from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN

                     where b.chassi.Contains(chassi)
                     where b.placa.Contains(placa)
                     where b.renavam.Contains(renavam)
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


                         renavam_bens = c.renavam,
                         chassi_bens = c.chassi,
                         placa_bens = c.placa,
                         comVenda = c.comVenda,
                         comDUT = c.comDUT,

                         conta = d.conta


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
                         

                         renavam_bens = x.renavam_bens,
                         chassi_bens = x.chassi_bens,
                         placa_bens = x.placa_bens,
                         comVenda = x.comVenda,
                         comDUT = x.comDUT,

                         conta = x.conta

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
                return RedirectToAction("RegistroDUT");
            }

            if (Request.HttpMethod == "POST" && (escolha == "rd" || escolha == "rc"))
            {


                // Controle de erros do ModelState
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();
                try
                {
                    if (ModelState.IsValid)
                    {
                        var procuraRegistro = db.Tbl_Dut
                            .FirstOrDefault(c => c.chassi == chassiPesquisado.Trim() || c.renavam == renavamPesquisado.Trim() || c.placa == placaPesquisada.Trim());
                        if (procuraRegistro != null)
                        {
                            procuraRegistro.chassi = chassiPesquisado.Trim();
                            procuraRegistro.renavam = renavamPesquisado.Trim();
                            procuraRegistro.placa = placaPesquisada.Trim();

                            if (escolha == "rd")
                                procuraRegistro.comDUT = true;
                            else if (escolha == "rc")
                                procuraRegistro.comVenda = true;

                            db.Entry(procuraRegistro).State = EntityState.Modified;
                            db.SaveChanges();
                            return View(GetContratosVeiculosViewModelAtualizaRegistroOk());
                        }
                        else
                        {
                            if (escolha == "rd")
                                comDUT = true;
                            else if (escolha == "rc")
                                comVenda = true;

                            model3 = new Tbl_Dut
                            {
                                id = 0,
                                chassi = chassiPesquisado.Trim(),
                                renavam = renavamPesquisado.Trim(),
                                placa = placaPesquisada.Trim(),
                                comDUT = comDUT,
                                comVenda = comVenda
                            };

                            if (db.Entry(model3).State == EntityState.Detached)
                            {
                                db.Tbl_Dut.Add(model3);
                                db.SaveChanges();
                                return View(GetContratosVeiculosViewModelRegistroOk());
                            }
                        }
                    }
                    //else
                    //{
                    //    return View(GetErroDeEntrada());
                    //}
                }
                catch(Exception e)
                {
                    Response.Write("<script>alert('" + e.InnerException + "')</script>");
                    return View(GetErroDeEntradaDesconhecido());
                }
                escolha = "nada";
            }
            return View("RegistroDUT", model);
        }


        public ActionResult _valor_total_recuperado_por_veiculo(string chassi, string placa, string renavam)
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

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato

                     from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv =>
                     (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa))

                     where (b.chassi.Contains(chassi) || b.placa.Contains(placa) || b.renavam.Contains(renavam))
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

        public ActionResult _valor_debito_total_por_veiculo(string chassi, string placa, string renavam)
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

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                    join b in db.Arm_Veiculos
                    on a.contrato equals b.contrato

                    from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv =>
                    (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa))

                    where (b.chassi.Contains(chassi) || b.placa.Contains(placa) || b.renavam.Contains(renavam))
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

        public ActionResult _valor_pago_total_por_veiculo(string chassi, string placa, string renavam)
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

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato

                     from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv =>
                     (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa))

                     where (b.chassi.Contains(chassi) || b.placa.Contains(placa) || b.renavam.Contains(renavam))
                     where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)

                     //where a.status.Equals(1)
                     group c by new { c.chassi, c.renavam, c.placa } into g
                     select new
                     {
                         soma_valor_debito_total = g.Sum(c => c.valor_pago_divida + c.valor_pago_custas)

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

        public ActionResult PagamentoDebitoIPVAManual2(int id_debito, string chassi, string placa, string renavam,
            bool? pagamento_efet_banco, DateTime? dta_pagamento_custas, DateTime? dta_pagamento_divida, string uf_pagamento, string grupo_safra, string pci_debito_divida, string pci_debito_custa, string cda, string rd,
            DateTime? dta_cobranca, DateTime? dta_custas, string uf_cobranca, string tipo_cobranca, decimal? valor_divida,
            string numero_miro_divida, string numero_miro_custa, string forma_pagamento_divida, string forma_pagamento_custas, string valor_pago_custas,
            string valor_pago_divida, decimal? valor_pago_total, string obs_pagamento, string pci_credito, DateTime? dta_recuperacao, string status_recuperacao,
            string ano_exercicio, decimal? valor_custas, bool? debito_protesto, string valor_recuperado,
            string nome_cartorio, bool? divida_ativa_serasa, bool? protesto_serasa, decimal? valor_debito_total, 
            DateTime? dta_contabil_divida, DateTime? dta_contabil_custa)
        {
            decimal? valor_pago_divida_unmask = null, valor_pago_custas_unmask = null, valor_recuperado_unmask = null;
            if (valor_divida != null)
            {
                try
                {
                    valor_pago_divida_unmask = Convert.ToDecimal((valor_pago_divida.Trim()));
                }
                catch { }
            }

            if (valor_custas != null)
            {
                try
                {
                    valor_pago_custas_unmask = Convert.ToDecimal((valor_pago_custas.Trim()));
                }
                catch { }
            }

            try
            {
                valor_recuperado_unmask = Convert.ToDecimal((valor_recuperado.Trim()));
            }
            catch { }


            if (chassi == null)
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

                     where (b.chassi.Contains(chassi) || b.placa.Contains(placa) || b.renavam.Contains(renavam))
                     where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)

                     where c.id.Equals(id_debito)
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

                         id_debito = c.id,
                         dta_cobranca = c.dta_cobranca,
                         uf_cobranca = c.uf_cobranca,
                         tipo_cobranca = c.tipo_cobranca,
                         valor_divida = c.valor_divida,
                         ano_exercicio = c.ano_exercicio,
                         cda = c.cda,
                         valor_custas = c.valor_custas,
                         debito_protesto = c.debito_protesto,
                         nome_cartorio = c.nome_cartorio,
                         divida_ativa_serasa = c.divida_ativa_serasa,
                         protesto_serasa = c.protesto_serasa,
                         valor_debito_total = c.valor_debito_total,
                         dta_custas = c.dta_custas,
                         dta_pagamento_custas = c.dta_pagamento_custas,
                         status_recuperacao = c.status_recuperacao,
                         dta_contabil_divida = c.dta_contabil_divida,
                         dta_contabil_custa = c.dta_contabil_custa,
                         dta_pagamento_divida = c.dta_pagamento_divida,
                         pagamento_efet_banco = c.pagamento_efet_banco,
                         uf_pagamento = c.uf_pagamento,
                         grupo_safra = c.grupo_safra,
                         pci_debito_divida = c.pci_debito_divida,
                         pci_debito_custa = c.pci_debito_custa,
                         numero_miro_divida = c.numero_miro_divida,
                         numero_miro_custa = c.numero_miro_custa,
                         forma_pagamento_divida = c.forma_pagamento_divida,
                         forma_pagamento_custas = c.forma_pagamento_custas,
                         valor_pago_custas = c.valor_pago_custas,
                         valor_pago_divida = c.valor_pago_divida,
                         valor_pago_total = c.valor_pago_total,
                         obs_pagamento = c.obs_pagamento,
                         pci_credito = c.pci_credito,
                         dta_recuperacao = c.dta_recuperacao,
                         valor_recuperado = c.valor_recuperado,

                         renavam_bens = d.renavam,
                         chassi_bens = d.chassi,
                         placa_bens = d.placa

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

                         id_debito = x.id_debito,
                         dta_cobranca = x.dta_cobranca,
                         uf_cobranca = x.uf_cobranca,
                         tipo_cobranca = x.tipo_cobranca,
                         valor_divida = x.valor_divida,
                         ano_exercicio = x.ano_exercicio,
                         cda = x.cda,
                         valor_custas = x.valor_custas,
                         debito_protesto = x.debito_protesto,
                         nome_cartorio = x.nome_cartorio,
                         divida_ativa_serasa = x.divida_ativa_serasa,
                         protesto_serasa = x.protesto_serasa,
                         valor_debito_total = x.valor_debito_total,
                         dta_custas = x.dta_custas,
                         dta_pagamento_custas = x.dta_pagamento_custas,
                         status_recuperacao = x.status_recuperacao,
                         dta_contabil_divida = x.dta_contabil_divida,
                         dta_contabil_custa = x.dta_contabil_custa,
                         dta_pagamento_divida = x.dta_pagamento_divida,
                         pagamento_efet_banco = x.pagamento_efet_banco,
                         uf_pagamento = x.uf_pagamento,
                         grupo_safra = x.grupo_safra,
                         pci_debito_divida = x.pci_debito_divida,
                         pci_debito_custa = x.pci_debito_custa,
                         numero_miro_divida = x.numero_miro_divida,
                         numero_miro_custa = x.numero_miro_custa,
                         forma_pagamento_divida = x.forma_pagamento_divida,
                         forma_pagamento_custas = x.forma_pagamento_custas,
                         valor_pago_custas = x.valor_pago_custas,
                         valor_pago_divida = x.valor_pago_divida,
                         valor_pago_total = x.valor_pago_total,
                         obs_pagamento = x.obs_pagamento,
                         pci_credito = x.pci_credito,
                         dta_recuperacao = x.dta_recuperacao,
                         valor_recuperado = x.valor_recuperado,

                         renavam_bens = x.renavam_bens,
                         chassi_bens = x.chassi_bens,
                         placa_bens = x.placa_bens

                     });


            try //A única maneira de contornar um erro no model.Count() quando se entra com um sequencia numérica no "where b.chassi.Contains(chassi)" do model, se for "where b.chassi.Equals(chassi)" não dá pau!
            {
                if (model.Count() == 0 || model == null)
                {
                    return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaVeiculo");
                }
            }
            catch(Exception e)
            {
                return View(GetContratosVeiculosViewModelErro());
            }

            if (model == null || model.Any() == false)
            {
                //return HttpNotFound();
                return RedirectToAction("PagamentoDebitoIPVAManual2");
            }

            if (Request.HttpMethod == "POST" && rd == "true")
            {

                // Controle de erros do ModelState
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

                if (ModelState.IsValid)
                {
                    var procuraRegistro = db.Tbl_DebitosEPagamentos_Veiculo
                        .FirstOrDefault(c => c.id == id_debito);

                    if (procuraRegistro != null)
                    {
                        procuraRegistro.dta_pagamento_divida = dta_pagamento_divida;
                        procuraRegistro.uf_pagamento = uf_pagamento;
                        procuraRegistro.numero_miro_divida = numero_miro_divida;
                        procuraRegistro.forma_pagamento_divida = forma_pagamento_divida;
                        procuraRegistro.valor_pago_total = valor_pago_total;
                        procuraRegistro.valor_pago_divida = valor_pago_divida_unmask;

                        procuraRegistro.pagamento_efet_banco = true;
                        procuraRegistro.grupo_safra = grupo_safra;
                        procuraRegistro.pci_debito_divida = pci_debito_divida;
                        procuraRegistro.pci_debito_custa = pci_debito_custa;
                        procuraRegistro.numero_miro_custa = numero_miro_custa;

                        procuraRegistro.dta_pagamento_custas = dta_pagamento_custas;
                        procuraRegistro.forma_pagamento_custas = forma_pagamento_custas;
                        procuraRegistro.valor_pago_custas = valor_pago_custas_unmask;
                        procuraRegistro.obs_pagamento = obs_pagamento;
                        procuraRegistro.dta_contabil_divida = dta_contabil_divida;
                        procuraRegistro.dta_contabil_custa = dta_contabil_custa;


                        procuraRegistro.pci_credito = pci_credito;
                        procuraRegistro.dta_recuperacao = dta_recuperacao;
                        procuraRegistro.valor_recuperado = valor_recuperado_unmask;

                        procuraRegistro.status_recuperacao = status_recuperacao;

                        db.Entry(procuraRegistro).State = EntityState.Modified;
                        db.SaveChanges();
                        return View(RecuperacaoOk());
                    }
                    else
                    {
                        model2 = new Tbl_DebitosEPagamentos_Veiculo
                        {
                            id = 0,
                            chassi = chassi,
                            renavam = renavam,
                            placa = placa,

                            dta_pagamento_divida = dta_pagamento_divida,
                            uf_pagamento = uf_pagamento,
                            numero_miro_divida = numero_miro_divida,
                            forma_pagamento_divida = forma_pagamento_divida,
                            valor_pago_total = valor_pago_total,
                            valor_pago_divida = valor_pago_divida_unmask,

                            pagamento_efet_banco = true,
                            grupo_safra = grupo_safra,
                            pci_debito_divida = pci_debito_divida,
                            pci_debito_custa = pci_debito_custa,
                            numero_miro_custa = numero_miro_custa,

                            dta_pagamento_custas = dta_pagamento_custas,
                            forma_pagamento_custas = forma_pagamento_custas,
                            valor_pago_custas = valor_pago_custas_unmask,
                            obs_pagamento = obs_pagamento,
                            dta_contabil_divida = dta_contabil_divida,
                            dta_contabil_custa = dta_contabil_custa,


                        pci_credito = pci_credito,
                            dta_recuperacao = dta_recuperacao,
                            valor_recuperado = valor_recuperado_unmask,

                            status_recuperacao = status_recuperacao,

                        };

                        if (db.Entry(model2).State == EntityState.Detached)
                        {
                            db.Tbl_DebitosEPagamentos_Veiculo.Add(model2);
                            db.SaveChanges();
                            return View(GetContratosVeiculosViewModelRegistroOk());
                        }
                    }
                }
                else
                {
                    return View(GetErroDeEntrada(null));
                }

            }
            return View("PagamentoDebitoIPVAManual2", model);
        }


        public ActionResult EditarRegistroDebitoIPVAManual(int id_debito, string chassi, string placa, string renavam,
            bool? pagamento_efet_banco, DateTime? dta_pagamento_divida, string uf_pagamento, string grupo_safra, string pci_debito_divida, string pci_debito_custa, string cda, string rd,
            DateTime? dta_cobranca, DateTime? dta_custas, string uf_cobranca, string tipo_cobranca, string valor_divida,
            string numero_miro_divida, string numero_miro_custa, string forma_pagamento_divida, string forma_pagamento_custas, decimal? valor_pago_custas,
            decimal? valor_pago_divida, decimal? valor_pago_total, string obs_pagamento, string pci_credito, DateTime? dta_recuperacao,
            string ano_exercicio, string valor_custas, bool? debito_protesto, decimal? valor_total_recuperado,
            string nome_cartorio, bool? divida_ativa_serasa, bool? protesto_serasa,
            string chassiPesquisado, string placaPesquisada, string renavamPesquisado)
        {
            if ((chassi == "" || chassi == null) && (placa == "" || placa == null) && (renavam == "" || renavam == null) && (rd == "false" || rd == null))
            {
                return View(GetContratosVeiculosViewModelPrimeira());
            }
            else if ((chassi == "" || chassi == null) && (placa == "" || placa == null) && (renavam == "" || renavam == null))
            {
                chassi = chassiPesquisado;
                placa = placaPesquisada;
                renavam = renavamPesquisado;
            }

            if (chassi == "" || chassi == null)
                chassi = "ø";
            if (placa == "" || placa == null)
                placa = "ø";
            if (renavam == "" || renavam == null)
                renavam = "ø";

            decimal? valor_divida_unmask = null, valor_custas_unmask = null;
            if (valor_divida != null)
            {
                try
                {
                    valor_divida_unmask = Convert.ToDecimal((valor_divida.Trim()));
                }
                catch { }
            }

            if (valor_custas != null)
            {
                try
                {
                    valor_custas_unmask = Convert.ToDecimal((valor_custas.Trim()));
                }
                catch { }
            }

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato

                     from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv =>
                     (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa)).DefaultIfEmpty()
                     //on new { b.chassi, b.renavam, b.placa } equals new { c.chassi, c.renavam, c.placa }
                     //into j1
                     //from c in j1.DefaultIfEmpty() //Isto é um LEFT JOIN

                     where (b.chassi.Contains(chassi) || b.placa.Contains(placa) || b.renavam.Contains(renavam))
                     where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
                     where c.id.Equals(id_debito)
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

                         id_debito = c.id,
                         dta_cobranca = c.dta_cobranca,
                         uf_cobranca = c.uf_cobranca,
                         tipo_cobranca = c.tipo_cobranca,
                         valor_divida = c.valor_divida,
                         ano_exercicio = c.ano_exercicio,
                         cda = c.cda,
                         valor_custas = c.valor_custas,
                         debito_protesto = c.debito_protesto,
                         nome_cartorio = c.nome_cartorio,
                         divida_ativa_serasa = c.divida_ativa_serasa,
                         protesto_serasa = c.protesto_serasa,
                         valor_debito_total_parc = c.valor_divida + c.valor_custas,
                         valor_debito_total = c.valor_debito_total,
                         dta_custas = c.dta_custas,

                         pagamento_efet_banco = c.pagamento_efet_banco,
                         dta_pagamento_divida = c.dta_pagamento_divida,
                         uf_pagamento = c.uf_pagamento,
                         grupo_safra = c.grupo_safra,
                         pci_debito_divida = c.pci_debito_divida,
                         pci_debito_custa = c.pci_debito_custa,
                         numero_miro_divida = c.numero_miro_divida,
                         numero_miro_custa = c.numero_miro_custa,
                         forma_pagamento_divida = c.forma_pagamento_divida,
                         forma_pagamento_custas = c.forma_pagamento_custas,
                         valor_pago_custas = c.valor_pago_custas,
                         valor_pago_divida = c.valor_pago_divida,
                         valor_pago_total = c.valor_pago_total,
                         obs_pagamento = c.obs_pagamento,
                         pci_credito = c.pci_credito,
                         dta_recuperacao = c.dta_recuperacao,
                         valor_total_recuperado = c.valor_total_recuperado,

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

                         id_debito = x.id_debito,
                         dta_cobranca = x.dta_cobranca,
                         uf_cobranca = x.uf_cobranca,
                         tipo_cobranca = x.tipo_cobranca,
                         valor_divida = x.valor_divida,
                         ano_exercicio = x.ano_exercicio,
                         cda = x.cda,
                         valor_custas = x.valor_custas,
                         debito_protesto = x.debito_protesto,
                         nome_cartorio = x.nome_cartorio,
                         divida_ativa_serasa = x.divida_ativa_serasa,
                         protesto_serasa = x.protesto_serasa,
                         valor_debito_total_parc = x.valor_divida + x.valor_custas,
                         valor_debito_total = x.valor_debito_total,
                         dta_custas = x.dta_custas,

                         pagamento_efet_banco = x.pagamento_efet_banco,
                         dta_pagamento_divida = x.dta_pagamento_divida,
                         uf_pagamento = x.uf_pagamento,
                         grupo_safra = x.grupo_safra,
                         pci_debito_divida = x.pci_debito_divida,
                         pci_debito_custa = x.pci_debito_custa,
                         numero_miro_divida = x.numero_miro_divida,
                         numero_miro_custa = x.numero_miro_custa,
                         forma_pagamento_divida = x.forma_pagamento_divida,
                         forma_pagamento_custas = x.forma_pagamento_custas,
                         valor_pago_custas = x.valor_pago_custas,
                         valor_pago_divida = x.valor_pago_divida,
                         valor_pago_total = x.valor_pago_total,
                         obs_pagamento = x.obs_pagamento,
                         pci_credito = x.pci_credito,
                         dta_recuperacao = x.dta_recuperacao,
                         valor_total_recuperado = x.valor_total_recuperado,

                     });


            try //A única maneira de contornar um erro no model.Count() quando se entra com um sequencia numérica no "where b.chassi.Contains(chassi)" do model, se for "where b.chassi.Equals(chassi)" não dá pau!
            {
                if (model.Count() == 0 || model == null)
                {
                    return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaVeiculo");
                }
            }
            catch (Exception e)
            {
                return View(GetContratosVeiculosViewModelErro());
            }

            if (model == null || model.Any() == false)
            {
                //return HttpNotFound();
                return RedirectToAction("EditarRegistroDebitoIPVAManual");
            }

            if (Request.HttpMethod == "POST" && rd == "true")
            {

                // Controle de erros do ModelState
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

                if (ModelState.IsValid)
                {
                    var procuraRegistro = db.Tbl_DebitosEPagamentos_Veiculo
                        .FirstOrDefault(c => c.id == id_debito);

                    if (procuraRegistro != null)
                    {
                        procuraRegistro.dta_custas = dta_custas;
                        procuraRegistro.uf_cobranca = uf_cobranca;
                        procuraRegistro.tipo_cobranca = tipo_cobranca;

                        procuraRegistro.valor_divida = valor_divida_unmask;
                        procuraRegistro.ano_exercicio = ano_exercicio;
                        procuraRegistro.cda = cda;

                        procuraRegistro.valor_custas = valor_custas_unmask;
                        procuraRegistro.debito_protesto = (debito_protesto == null ? false : true);
                        procuraRegistro.nome_cartorio = nome_cartorio;

                        procuraRegistro.divida_ativa_serasa = (divida_ativa_serasa == null ? false : true);
                        procuraRegistro.protesto_serasa = (protesto_serasa == null ? false : true);
                        procuraRegistro.dta_custas = dta_custas;
                        procuraRegistro.dta_cobranca = dta_cobranca;

                        procuraRegistro.valor_debito_total = (valor_divida_unmask.HasValue ? valor_divida_unmask : 0) + (valor_custas_unmask.HasValue ? valor_custas_unmask : 0);

                        db.Entry(procuraRegistro).State = EntityState.Modified;
                        db.SaveChanges();
                        return View(GetContratosVeiculosViewModelAtualizaRegistroOk());
                    }
                    else
                    {
                        return View(GetErroDeEntrada(null));
                    }
                }
                else
                {
                    return View(GetErroDeEntrada(null));
                }

            }
            return View("EditarRegistroDebitoIPVAManual", model);
        }


        public ActionResult PagamentoDebitoIPVAManual(string chassi, string placa, string renavam)
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

                     where (b.chassi.Contains(chassi) || b.placa.Contains(placa) || b.renavam.Contains(renavam))
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

                         id_debito = (c.id != null ? c.id : 0), //Isto é necessário apesar de aqui aparecer que não é. Sem isto, veículos sem débitos causam erro por vir null e não aparecem os dados dele.
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
                         dta_pagamento_custas = c.dta_pagamento_custas,
                         dta_pagamento_divida = c.dta_pagamento_divida,

                         renavam_bens = d.renavam,
                         chassi_bens = d.chassi,
                         placa_bens = d.placa,

                         comVenda = e.comVenda

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

                         id_debito             = x.id_debito,
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
                         dta_pagamento_custas = x.dta_pagamento_custas,
                         dta_pagamento_divida = x.dta_pagamento_divida,

                         renavam_bens = x.renavam_bens,
                         chassi_bens = x.chassi_bens,
                         placa_bens = x.placa_bens,

                         comVenda = x.comVenda

                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);

            try
            {
                if (model.Count() == 0 || model == null)
                {
                    return View(GetContratosVeiculosViewModelErro()); //RedirectToAction("ConsultaVeiculo");
                }

                if (model == null || model.Any() == false)
                {
                    //return HttpNotFound();
                    return RedirectToAction("PagamentoDebitoIPVAManual");
                }
            }
            catch
            {
                return View(GetContratosVeiculosViewModelErro());
            }
            return View("PagamentoDebitoIPVAManual", model);
        }


        // GET: Arm_LiquidadosEAtivos_Contrato/Details/5
        public ActionResult RegistroDebitoIPVAManual(string chassi, string placa, string renavam, string chassiPesquisado, string placaPesquisada, string renavamPesquisado, string rd,
            DateTime? dta_cobranca, DateTime? dta_custas, string uf_cobranca, string tipo_cobranca, string valor_divida, 
            string ano_exercicio, string cda, string valor_custas, bool? debito_protesto, 
            string nome_cartorio, bool? divida_ativa_serasa, bool? protesto_serasa)
        {
            if ((chassi == "" || chassi == null) && (placa == "" || placa == null) && (renavam == "" || renavam == null) && (rd == "false" || rd == null))
            {
                return View(GetContratosVeiculosViewModelPrimeira());
            }

            if (chassi == "" || chassi == null)
                chassi = "ø";
            if (placa == "" || placa == null)
                placa = "ø";
            if (renavam == "" || renavam == null)
                renavam = "ø";

            decimal? valor_divida_unmask = null, valor_custas_unmask = null;
            if (valor_divida != null)
            {
                try
                {
                    valor_divida_unmask = Convert.ToDecimal((valor_divida.Trim()));
                }
                catch { }
            }
            else
            {
                valor_divida_unmask = 0;
            }

            if (valor_custas != null)
            {
                try
                {
                    valor_custas_unmask = Convert.ToDecimal((valor_custas.Trim()));
                }
                catch { }
            }
            else
            {
                valor_custas_unmask = 0;
            }

                model = (
                        from a in db.Arm_LiquidadosEAtivos_Contrato
                        join b in db.Arm_Veiculos
                        on a.contrato equals b.contrato

                        from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv => 
                        (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa)).DefaultIfEmpty()
                        //on b.chassi equals c.chassi
                        //where (c.chassi == b.chassi || c.renavam == b.renavam)
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

                        where (b.chassi.Contains(chassi) || b.placa.Contains(placa) || b.renavam.Contains(renavam))
                        where a.origem.Equals("B")
                        where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
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

                            id_debito               = (c.id != null ? c.id : 0), //Isto é necessário apesar de aqui aparecer que não é. Sem isto, veículos sem débitos causam erro por vir null e não aparecem os dados dele.
                            dta_cobranca            = c.dta_cobranca,
                            uf_cobranca             = c.uf_cobranca,
                            uf_pagamento            = c.uf_pagamento,
                            tipo_cobranca           = c.tipo_cobranca,
                            valor_divida            = c.valor_divida,
                            ano_exercicio           = c.ano_exercicio,
                            cda                     = c.cda,
                            valor_custas            = c.valor_custas,
                            pagamento_efet_banco    = c.pagamento_efet_banco,
                            valor_recuperado        = c.valor_recuperado,
                            valor_total_recuperado  = c.valor_total_recuperado,
                            debito_protesto         = c.debito_protesto,
                            nome_cartorio           = c.nome_cartorio,
                            divida_ativa_serasa     = c.divida_ativa_serasa,
                            protesto_serasa         = c.protesto_serasa,
                            valor_debito_total_parc = c.valor_divida + c.valor_custas,
                            valor_debito_total      = c.valor_debito_total,
                            dta_custas              = c.dta_custas,
                            dta_pagamento_divida    = c.dta_pagamento_divida,

                            renavam_bens    = d.renavam,
                            chassi_bens     = d.chassi,
                            placa_bens      = d.placa,

                            comVenda = e.comVenda

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

                            id_debito               = x.id_debito, 
                            dta_cobranca            = x.dta_cobranca,
                            uf_cobranca             = x.uf_cobranca,
                            uf_pagamento            = x.uf_pagamento,
                            tipo_cobranca           = x.tipo_cobranca,
                            valor_divida            = x.valor_divida,
                            ano_exercicio           = x.ano_exercicio,
                            cda                     = x.cda,
                            valor_custas            = x.valor_custas,
                            pagamento_efet_banco    = x.pagamento_efet_banco,
                            valor_recuperado        = x.valor_recuperado,
                            valor_total_recuperado  = x.valor_total_recuperado,
                            debito_protesto         = x.debito_protesto,
                            nome_cartorio           = x.nome_cartorio,
                            divida_ativa_serasa     = x.divida_ativa_serasa,
                            protesto_serasa         = x.protesto_serasa,
                            valor_debito_total_parc = x.valor_divida + x.valor_custas,
                            valor_debito_total      = x.valor_debito_total,
                            dta_custas              = x.dta_custas,
                            dta_pagamento_divida    = x.dta_pagamento_divida,

                            renavam_bens    = x.renavam_bens,
                            chassi_bens     = x.chassi_bens,
                            placa_bens      = x.placa_bens,

                            comVenda = x.comVenda

                        }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);

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
                //CONTA e Compara com os registros já existentes no BD.
                int idDoCara = 0;
                try
                {
                    idDoCara = db.Tbl_DebitosEPagamentos_Veiculo
                    .Where(c => c.chassi == chassiPesquisado || c.renavam == renavamPesquisado || c.placa == placaPesquisada)
                    .Where(c => c.cda == cda)
                    .Where(c => DbFunctions.TruncateTime(c.dta_cobranca) == dta_cobranca)
                    .Where(c => DbFunctions.TruncateTime(c.dta_custas) == dta_custas)
                    .Where(c => c.valor_divida == valor_divida_unmask)
                    .Where(c => c.valor_custas == valor_custas_unmask)
                    .Where(c => c.valor_debito_total == valor_divida_unmask + valor_custas_unmask)
                    .Where(c => c.ano_exercicio == ano_exercicio)
                    .ToList().Count();
                }
                catch (Exception e)
                {
                    Response.Write("<script>alert('" + e.Message + "');</script>");
                };

                var errors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                    model2 = new Tbl_DebitosEPagamentos_Veiculo
                    {
                        id                      = (idDoCara > 0 ? idDoCara : 0), //Se não houver um id desse cara ele põe 0
                        chassi                  = chassiPesquisado,
                        renavam                 = renavamPesquisado,
                        placa                   = placaPesquisada,
                        dta_cobranca            = dta_cobranca,
                        dta_custas    = dta_custas,
                        uf_cobranca             = uf_cobranca,
                        tipo_cobranca           = tipo_cobranca,
                        valor_divida            = valor_divida_unmask,
                        ano_exercicio           = ano_exercicio,
                        cda                     = cda,
                        valor_custas            = valor_custas_unmask,
                        debito_protesto         = (debito_protesto == null ? false : true),
                        nome_cartorio           = nome_cartorio,
                        divida_ativa_serasa     = (divida_ativa_serasa == null ? false : true),
                        protesto_serasa         = (protesto_serasa == null ? false : true),
                        valor_debito_total      = valor_divida_unmask + valor_custas_unmask
                    };
                        
                    if (idDoCara != 0)
                    {
                        return View(GetContratosVeiculosViewModelErroRegistroDebito());
                    }
                    else
                    if (db.Entry(model2).State == EntityState.Detached)
                    {
                        try
                        {
                            db.Tbl_DebitosEPagamentos_Veiculo.Add(model2);
                            db.SaveChanges();
                            return View("RegistroDebitoIPVAManual", model);
                        }
                        catch (Exception e)
                        {
                            //if(e.GetType)
                            return View(GetErroDeEntrada(model.First()));
                        };
                    }
                }
                else
                {
                    return View(GetErroDeEntrada(null));
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
