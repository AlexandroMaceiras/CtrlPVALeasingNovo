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

        /// <summary>
        /// Model LINQ para o Relatório e para o CSV do RelatorioBensApreendidosDebtoIPVA
        /// </summary>
        private void ModelRelatorioBensApreendidosDebtoIPVA()
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato

                     from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv =>
                     (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa))

                     from d in db.Tbl_Bens.Where(bens =>
                     (b.chassi == bens.chassi) || (b.renavam == bens.renavam) || (b.placa == bens.placa))

                     join e in db.Tbl_CCL
                     on a.cpf_cnpj_cliente equals e.cpf_cnpj_cliente
                     into j1
                     from e in j1.DefaultIfEmpty() //Isto é um LEFT JOIN pra trazer quem esta na CCL e quem não está também.

                     where (c.pagamento_efet_banco == false || c.pagamento_efet_banco == null)

                     where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
                     select new
                     {
                         contrato = a.contrato,
                         status = a.status,
                         nome_cliente = a.nome_cliente,
                         cpf_cnpj_cliente = a.cpf_cnpj_cliente,
                         marca = e.marca,
                         chassi = b.chassi,
                         renavam = b.renavam,
                         placa = b.placa,
                         dta_cobranca = c.dta_cobranca,
                         uf_cobranca = c.uf_cobranca,
                         tipo_cobranca = c.tipo_cobranca,
                         valor_divida = c.valor_divida,
                         ano_exercicio = c.ano_exercicio,
                         pagamento_efet_banco = c.pagamento_efet_banco

                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         contrato = x.contrato,
                         status = x.status,
                         nome_cliente = x.nome_cliente,
                         cpf_cnpj_cliente = x.cpf_cnpj_cliente,
                         marca = x.marca,
                         chassi = x.chassi,
                         renavam = x.renavam,
                         placa = x.placa,
                         dta_cobranca = x.dta_cobranca,
                         uf_cobranca = x.uf_cobranca,
                         tipo_cobranca = x.tipo_cobranca,
                         valor_divida = x.valor_divida,
                         ano_exercicio = x.ano_exercicio,
                         pagamento_efet_banco = x.pagamento_efet_banco

                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);
        }

        public ActionResult RelatorioBensApreendidosDebtoIPVA()
        {
            ModelRelatorioBensApreendidosDebtoIPVA();
            return View("", model);
        }

        public ActionResult CSVdoRelatorioBensApreendidosDebtoIPVA()
        {
            //Gera Resultado em CSV.
            StringWriter sw = new StringWriter();

            sw.WriteLine("Nº Contrato; Status do Contrato; Nome do Cliente; CPF/CNPJ; Marca; Chassi; Renavam; Placa; Data da Cobrança; UF da Cobrança; Tipo de Cobrança; Valor da Dívida; Ano de Exercício");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=RelatorioBensApreendidosDebtoIPVA.csv");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.Default;

            var users = db.Tbl_DadosDaVenda.ToList();

            ModelRelatorioBensApreendidosDebtoIPVA();
            foreach (var elemento in model)
            {
                sw.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};",

                elemento.contrato,
                elemento.status,
                elemento.nome_cliente,
                elemento.cpf_cnpj_cliente,
                elemento.marca,
                elemento.chassi,
                elemento.renavam,
                elemento.placa,
                elemento.dta_cobranca,
                elemento.uf_cobranca,
                elemento.tipo_cobranca,
                elemento.valor_divida,
                elemento.ano_exercicio
                ));
            }
            Response.Write(sw.ToString());
            Response.End();

            return new EmptyResult();
        }

        /// <summary>
        /// Model LINQ para o Relatório e para o CSV do RelatorioCobrancasSimplesParaPagamento
        /// </summary>
        private void ModelRelatorioCobrancasSimplesParaPagamento()
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato

                     from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv =>
                     (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa))

                     from d in db.Tbl_Bens.Where(bens =>
                     (b.chassi == bens.chassi) || (b.renavam == bens.renavam) || (b.placa == bens.placa)).DefaultIfEmpty() //Isto é um LEFT JOIN
                     //join d in db.Tbl_Bens
                     //on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                     //into j0
                     //from d in j0.DefaultIfEmpty() //Isto é um LEFT JOIN

                     join e in db.Tbl_CCL
                     on a.cpf_cnpj_cliente equals e.cpf_cnpj_cliente
                     into j1
                     from e in j1.DefaultIfEmpty() //Isto é um LEFT JOIN pra trazer quem esta na CCL e quem não está também.

                     where (d.chassi == null || d.renavam == null || d.placa == null)

                     where (c.pagamento_efet_banco == false || c.pagamento_efet_banco == null)

                     where (e.marca.Equals("JUR") || e.marca == null)

                     where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
                     select new
                     {
                         contrato = a.contrato,
                         status = a.status,
                         nome_cliente = a.nome_cliente,
                         cpf_cnpj_cliente = a.cpf_cnpj_cliente,
                         marca = e.marca,
                         chassi = b.chassi,
                         renavam = b.renavam,
                         placa = b.placa,
                         dta_cobranca = c.dta_cobranca,
                         uf_cobranca = c.uf_cobranca,
                         tipo_cobranca = c.tipo_cobranca,
                         valor_divida = c.valor_divida,
                         ano_exercicio = c.ano_exercicio,
                         pagamento_efet_banco = c.pagamento_efet_banco

                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         contrato = x.contrato,
                         status = x.status,
                         nome_cliente = x.nome_cliente,
                         cpf_cnpj_cliente = x.cpf_cnpj_cliente,
                         marca = x.marca,
                         chassi = x.chassi,
                         renavam = x.renavam,
                         placa = x.placa,
                         dta_cobranca = x.dta_cobranca,
                         uf_cobranca = x.uf_cobranca,
                         tipo_cobranca = x.tipo_cobranca,
                         valor_divida = x.valor_divida,
                         ano_exercicio = x.ano_exercicio,
                         pagamento_efet_banco = x.pagamento_efet_banco

                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);
        }

        public ActionResult RelatorioCobrancasSimplesParaPagamento()
        {
            ModelRelatorioCobrancasSimplesParaPagamento();
            return View("", model);
        }

        public ActionResult CSVdoRelatorioCobrancasSimplesParaPagamento()
        {
            //Gera Resultado em CSV.
            StringWriter sw = new StringWriter();

            sw.WriteLine("Nº Contrato; Status do Contrato; Nome do Cliente; CPF/CNPJ; Marca; Chassi; Renavam; Placa; Data da Cobrança; UF da Cobrança; Tipo de Cobrança; Valor da Dívida; Ano de Exercício");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=RelatorioCobrancasSimplesParaPagamento.csv");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.Default;

            var users = db.Tbl_DadosDaVenda.ToList();

            ModelRelatorioCobrancasSimplesParaPagamento();
            foreach (var elemento in model)
            {
                sw.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};",

                elemento.contrato,
                elemento.status,
                elemento.nome_cliente,
                elemento.cpf_cnpj_cliente,
                elemento.marca,
                elemento.chassi,
                elemento.renavam,
                elemento.placa,
                elemento.dta_cobranca,
                elemento.uf_cobranca,
                elemento.tipo_cobranca,
                elemento.valor_divida,
                elemento.ano_exercicio
                ));
            }
            Response.Write(sw.ToString());
            Response.End();

            return new EmptyResult();
        }

        /// <summary>
        /// Model LINQ para o Relatório e para o CSV do RelatorioCobrancaIPVAAreaBens
        /// </summary>
        private void ModelRelatorioCobrancaIPVAAreaBens()
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato

                     from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv =>
                     (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa))

                     from d in db.Tbl_Bens.Where(bens =>
                     (b.chassi == bens.chassi) || (b.renavam == bens.renavam) || (b.placa == bens.placa)).DefaultIfEmpty() //Isto é um LEFT JOIN

                     join e in db.Tbl_CCL
                     on a.cpf_cnpj_cliente equals e.cpf_cnpj_cliente
                     into j1
                     from e in j1.DefaultIfEmpty() //Isto é um LEFT JOIN pra trazer quem esta na CCL e quem não está também.

                     where c.pagamento_efet_banco == true
                     
                     where (c.dta_recuperacao == null || c.valor_recuperado == null || c.valor_total_recuperado == null)
                     
                     where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)

                     select new
                     {
                         contrato = a.contrato,
                         status = a.status,
                         nome_cliente = a.nome_cliente,
                         cpf_cnpj_cliente = a.cpf_cnpj_cliente,
                         marca = e.marca,
                         chassi = b.chassi,
                         renavam = b.renavam,
                         placa = b.placa,
                         valor_divida = c.valor_divida,
                         valor_custas = c.valor_custas,
                         valor_debito_total = c.valor_debito_total,
                         ano_exercicio = c.ano_exercicio,
                         valor_pago_total = c.valor_divida + c.valor_custas


                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         contrato = x.contrato,
                         status = x.status,
                         nome_cliente = x.nome_cliente,
                         cpf_cnpj_cliente = x.cpf_cnpj_cliente,
                         marca = x.marca,
                         chassi = x.chassi,
                         renavam = x.renavam,
                         placa = x.placa,
                         valor_divida = x.valor_divida,
                         valor_custas = x.valor_custas,
                         valor_debito_total = x.valor_debito_total,
                         ano_exercicio = x.ano_exercicio,
                         valor_pago_total = x.valor_divida + x.valor_custas

                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);
        }

        public ActionResult RelatorioCobrancaIPVAAreaBens()
        {
            ModelRelatorioCobrancaIPVAAreaBens();
            return View("", model);
        }
        public ActionResult CSVdoRelatorioCobrancaIPVAAreaBens()
        {
            //Gera Resultado em CSV.
            StringWriter sw = new StringWriter();

            sw.WriteLine("Nº Contrato; Status do Contrato; Nome do Cliente; CPF/CNPJ; Marca; Chassi; Renavam; Placa; Valor da Dívida; Valor das Custas; Valor Débito Total; Ano de Exercício; Valor Pago Total");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=RelatorioCobrancaIPVAAreaBens.csv");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.Default;

            var users = db.Tbl_DadosDaVenda.ToList();

            ModelRelatorioCobrancaIPVAAreaBens();
            foreach (var elemento in model)
            {
                sw.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};",

                elemento.contrato,
                elemento.status,
                elemento.nome_cliente,
                elemento.cpf_cnpj_cliente,
                elemento.marca,
                elemento.chassi,
                elemento.renavam,
                elemento.placa,
                elemento.valor_divida,
                elemento.valor_custas,
                elemento.valor_debito_total,
                elemento.ano_exercicio,
                elemento.valor_pago_total
                ));
            }
            Response.Write(sw.ToString());
            Response.End();

            return new EmptyResult();
        }

        /// <summary>
        /// Model LINQ para o Relatório e para o CSV do RelatorioDebitoEmContaCorrente
        /// </summary>
        private void ModelRelatorioDebitoEmContaCorrente()
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato

                     from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv =>
                     (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa))

                     from d in db.Tbl_Bens.Where(bens =>
                     (b.chassi == bens.chassi) || (b.renavam == bens.renavam) || (b.placa == bens.placa)).DefaultIfEmpty() //Isto é um LEFT JOIN
                     //join d in db.Tbl_Bens
                     //on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                     //into j2
                     //from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN pra trazer quem esta na Bens e quem não está também.

                     join f in db.Tbl_SCC
                     on a.cpf_cnpj_cliente equals f.cpf_cnpj_cliente

                     join e in db.Tbl_CCL
                     on a.cpf_cnpj_cliente equals e.cpf_cnpj_cliente
                     into j1
                     from e in j1.DefaultIfEmpty() //Isto é um LEFT JOIN pra trazer quem esta na CCL e quem não está também.


                     where (c.dta_recuperacao == null || c.valor_recuperado == null || c.valor_total_recuperado == null)

                     where (d.chassi == null || d.renavam == null || d.placa == null)

                     where (c.pagamento_efet_banco == true || c.pagamento_efet_banco != null)

                     where (!e.marca.Contains("JUR") || e.marca == null)

                     where (f.perm_debito == true)

                     where (f.sinal == "+")

                     where (f.saldo >= c.valor_debito_total)

                     where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)

                     select new
                     {
                         contrato = a.contrato,
                         status = a.status,
                         nome_cliente = a.nome_cliente,
                         cpf_cnpj_cliente = a.cpf_cnpj_cliente,
                         chassi = b.chassi,
                         agencia = a.agencia,
                         conta = f.conta,
                         valor_divida = c.valor_divida,
                         valor_custas = c.valor_custas,
                         valor_debito_total = c.valor_debito_total,
                         ano_exercicio = c.ano_exercicio,
                         valor_pago_total = c.valor_divida + c.valor_custas

                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         contrato = x.contrato,
                         status = x.status,
                         nome_cliente = x.nome_cliente,
                         cpf_cnpj_cliente = x.cpf_cnpj_cliente,
                         chassi = x.chassi,
                         agencia = x.agencia,
                         conta = x.conta,
                         valor_divida = x.valor_divida,
                         valor_custas = x.valor_custas,
                         valor_debito_total = x.valor_debito_total,
                         ano_exercicio = x.ano_exercicio,
                         valor_pago_total = x.valor_divida + x.valor_custas

                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);
        }

        public ActionResult RelatorioDebitoEmContaCorrente()
        {
            ModelRelatorioDebitoEmContaCorrente();
            return View("", model);
        }

        public ActionResult CSVdoRelatorioDebitoEmContaCorrente()
        {
            //Gera Resultado em CSV.
            StringWriter sw = new StringWriter();

            sw.WriteLine("Nº Contrato; Status do Contrato; Nome do Cliente; CPF/CNPJ; Chassi; Agência; Conta; Valor da Dívida; Valor das Custas; Valor Débito Total; Ano de Exercício; Valor Pago Total; Valor À Ser Debitado");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=RelatorioDebitoEmContaCorrente.csv");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.Default;

            var users = db.Tbl_DadosDaVenda.ToList();

            ModelRelatorioDebitoEmContaCorrente();
            foreach (var elemento in model)
            {
                sw.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};",

                elemento.contrato,
                elemento.status,
                elemento.nome_cliente,
                " - " + elemento.cpf_cnpj_cliente.ToString() + " - ",
                elemento.chassi,
                elemento.agencia,
                elemento.conta,
                elemento.valor_divida,
                elemento.valor_custas,
                elemento.valor_debito_total,
                elemento.ano_exercicio,
                elemento.valor_pago_total,
                elemento.valor_pago_total
                ));
            }
            Response.Write(sw.ToString());
            Response.End();

            return new EmptyResult();
        }

        /// <summary>
        /// Model LINQ para o Relatório e para o CSV do RelatorioParaIncorporacaoDeDividaJur
        /// </summary>
        private void ModelRelatorioParaIncorporacaoDeDividaJur()
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato

                     from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv =>
                     (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa))

                     from d in db.Tbl_Bens.Where(bens =>
                     (b.chassi == bens.chassi) || (b.renavam == bens.renavam) || (b.placa == bens.placa)).DefaultIfEmpty() //Isto é um LEFT JOIN
                     //join d in db.Tbl_Bens
                     //on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                     //into j2
                     //from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN pra trazer quem esta na Bens e quem não está também.

                     join e in db.Tbl_CCL
                     on a.cpf_cnpj_cliente equals e.cpf_cnpj_cliente
                     into j1
                     from e in j1.DefaultIfEmpty() //Isto é um LEFT JOIN pra trazer quem esta na CCL e quem não está também.

                     where (c.dta_recuperacao == null || c.valor_recuperado == null || c.valor_total_recuperado == null)

                     where (d.chassi == null || d.renavam == null || d.placa == null)

                     where (c.pagamento_efet_banco == true || c.pagamento_efet_banco != null)

                     where e.marca.Equals("JUR")

                     where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)

                     select new
                     {
                         contrato = a.contrato,
                         status = a.status,
                         nome_cliente = a.nome_cliente,
                         cpf_cnpj_cliente = a.cpf_cnpj_cliente,
                         marca = e.marca,
                         chassi = b.chassi,
                         renavam = b.renavam,
                         placa = b.placa,
                         valor_divida = c.valor_divida,
                         valor_custas = c.valor_custas,
                         valor_debito_total = c.valor_debito_total,
                         ano_exercicio = c.ano_exercicio,
                         valor_pago_total = c.valor_divida + c.valor_custas

                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         contrato = x.contrato,
                         status = x.status,
                         nome_cliente = x.nome_cliente,
                         cpf_cnpj_cliente = x.cpf_cnpj_cliente,
                         marca = x.marca,
                         chassi = x.chassi,
                         renavam = x.renavam,
                         placa = x.placa,
                         valor_divida = x.valor_divida,
                         valor_custas = x.valor_custas,
                         valor_debito_total = x.valor_debito_total,
                         ano_exercicio = x.ano_exercicio,
                         valor_pago_total = x.valor_divida + x.valor_custas

                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);
        }

        /// <summary>
        /// Model LINQ para o Relatório e para o CSV do RelatorioParaIncorporacaoDeDividaRat
        /// </summary>
        private void ModelRelatorioParaIncorporacaoDeDividaRat()
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato

                     from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv =>
                     (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa))

                     from d in db.Tbl_Bens.Where(bens =>
                     (b.chassi == bens.chassi) || (b.renavam == bens.renavam) || (b.placa == bens.placa)).DefaultIfEmpty() //Isto é um LEFT JOIN
                     //join d in db.Tbl_Bens
                     //on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                     //into j2
                     //from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN pra trazer quem esta na Bens e quem não está também.

                     join e in db.Tbl_CCL
                     on a.cpf_cnpj_cliente equals e.cpf_cnpj_cliente
                     into j1
                     from e in j1.DefaultIfEmpty() //Isto é um LEFT JOIN pra trazer quem esta na CCL e quem não está também.

                     where (c.dta_recuperacao == null || c.valor_recuperado == null || c.valor_total_recuperado == null)

                     where (d.chassi == null || d.renavam == null || d.placa == null)

                     where (c.pagamento_efet_banco == true || c.pagamento_efet_banco != null)

                     where e.marca != "JUR"

                     where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
                     select new
                    {
                        contrato = a.contrato,
                        status = a.status,
                        nome_cliente = a.nome_cliente,
                        cpf_cnpj_cliente = a.cpf_cnpj_cliente,
                        marca = e.marca,
                        chassi = b.chassi,
                        renavam = b.renavam,
                        placa = b.placa,
                        valor_divida = c.valor_divida,
                        valor_custas = c.valor_custas,
                        valor_debito_total = c.valor_debito_total,
                        ano_exercicio = c.ano_exercicio,
                        valor_pago_total = c.valor_divida + c.valor_custas

                    }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                    {
                        contrato = x.contrato,
                        status = x.status,
                        nome_cliente = x.nome_cliente,
                        cpf_cnpj_cliente = x.cpf_cnpj_cliente,
                        marca = x.marca,
                        chassi = x.chassi,
                        renavam = x.renavam,
                        placa = x.placa,
                        valor_divida = x.valor_divida,
                        valor_custas = x.valor_custas,
                        valor_debito_total = x.valor_debito_total,
                        ano_exercicio = x.ano_exercicio,
                        valor_pago_total = x.valor_divida + x.valor_custas

                    }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);
        }

        public ActionResult RelatorioParaIncorporacaoDeDividaJurRat(string RAT)
        {
            if (RAT != "true")
            {
                ModelRelatorioParaIncorporacaoDeDividaJur();
            }
            else
            {
                ModelRelatorioParaIncorporacaoDeDividaRat();
            }

            return View("", model);
        }

        public ActionResult CSVdoRelatorioParaIncorporacaoDeDividaJurRat(string RAT)
        {
            //Gera Resultado em CSV.
            StringWriter sw = new StringWriter();

            sw.WriteLine("Nº Contrato; Status do Contrato; Nome do Cliente; CPF/CNPJ; Marca; Chassi; Renavam; Placa; Valor da Dívida; Valor das Custas; Valor Débito Total; Ano de Exercício; Valor Total Pago; Valor Para Incorporar");

            Response.ClearContent();
            if (RAT != "true")
            {
                ModelRelatorioParaIncorporacaoDeDividaJur();
                Response.AddHeader("content-disposition", "attachment;filename=RelatorioParaIncorporacaoDeDividaJur.csv");
            }
            else
            {
                ModelRelatorioParaIncorporacaoDeDividaRat();
                Response.AddHeader("content-disposition", "attachment;filename=RelatorioParaIncorporacaoDeDividaRat.csv");
            }

            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.Default;

            var users = db.Tbl_DadosDaVenda.ToList();

            foreach (var elemento in model)
            {
                sw.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};",

                elemento.contrato,
                elemento.status,
                elemento.nome_cliente,
                elemento.cpf_cnpj_cliente,
                elemento.marca,
                elemento.chassi,
                elemento.renavam,
                elemento.placa,
                elemento.valor_divida,
                elemento.valor_custas,
                elemento.valor_debito_total,
                elemento.ano_exercicio,
                elemento.valor_pago_total,
                elemento.valor_pago_total
                ));
            }
            Response.Write(sw.ToString());
            Response.End();

            return new EmptyResult();
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

                     from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv =>
                     (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa))

                     join f in db.Tbl_Agencias
                     on a.agencia equals f.agencia
                     into j1
                     from f in j1.DefaultIfEmpty()

                     where c.dta_pagamento >= dataInicio
                     where c.dta_pagamento <= dataFim
                     where a.origem.Equals("B")
                     where (!b.origem.Contains("RECIBO VEN") || b.origem == null)
                     select new
                     {
                         chassi             = b.chassi,
                         contrato           = a.contrato,
                         grupo_safra        = c.grupo_safra,

                         descricao_agencia  = f.descricao_agencia,
                         agencia            = a.agencia,

                         c.dta_pagamento,
                         c.valor_divida,
                         c.pci_debito_divida,
                         c.dta_cobranca,
                         c.dta_contabil_divida


                     }).AsEnumerable().Select(x => new ContratosVeiculosViewModel
                     {
                         chassi             = x.chassi,
                         contrato           = x.contrato,
                         grupo_safra        = x.grupo_safra,

                         descricao_agencia  = x.descricao_agencia,
                         agencia            = x.agencia,

                         dta_pagamento          = x.dta_pagamento,
                         valor_divida           = x.valor_divida,
                         pci_debito_divida      = x.pci_debito_divida,
                         dta_cobranca           = x.dta_cobranca,
                         dta_contabil_divida    = x.dta_contabil_divida

                     }).OrderByDescending(x => x.ano_exercicio).OrderByDescending(x => x.dta_cobranca);

            if (escolha)
            {
                //Gera Resultado em CSV.
                StringWriter sw = new StringWriter();

                string cabecalho = "";
                cabecalho += "BO_LEASING" + "_" + dataInicio.ToString().Substring(6,4) + ";";
                cabecalho += "Descrição;";
                cabecalho += "No Contrato/Processo;";
                cabecalho += "Nome Empresa;";
                cabecalho += "Cód;";
                cabecalho += "Área Onde Ocorreu a Perda;";
                cabecalho += "Cód;";
                cabecalho += "Tipo do Evento - 1º Nível;";
                cabecalho += "Cód;";
                cabecalho += "Tipo do Evento - 2º Nível;";
                cabecalho += "Cód;";
                cabecalho += "Tipo do Evento - 3º Nível;";
                cabecalho += "Cód;";
                cabecalho += "Fator de Risco;";
                cabecalho += "Cód;";
                cabecalho += "Subfator de Risco;";
                cabecalho += "Cód;";
                cabecalho += "Causa da Perda;";
                cabecalho += "Id Evento Perdas Raiz;";
                cabecalho += "Sistema Envolvido;";
                cabecalho += "Cód;";
                cabecalho += "Produto;";
                cabecalho += "Cód;";
                cabecalho += "Canal de Distribuição;";
                cabecalho += "Cód;";
                cabecalho += "Tipo de Evento;";
                cabecalho += "Data de Incidente;";
                cabecalho += "Data da Descoberta;";
                cabecalho += "Tipo de Risco Vinculado;";
                cabecalho += "Cód;";
                cabecalho += "Área que Cadastrou a Perda;";
                cabecalho += "Cód;";
                cabecalho += "Linha de Negócio;";
                cabecalho += "Cód;";
                cabecalho += "Segmento Comercial;";
                cabecalho += "Cód;";
                cabecalho += "Tipo Financeiro;";
                cabecalho += "Tipo de Lançamento;";
                cabecalho += "Cód;";
                cabecalho += "Data Contábil;";
                cabecalho += "Valor;";
                cabecalho += "PCI / PCU";

                sw.WriteLine(cabecalho);

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=RelatorioPerdasOperacionais.csv");
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = System.Text.Encoding.Default;

                var users = db.Tbl_DadosDaVenda.ToList();

                int contador = 0;

                foreach (var elemento in model)
                {
                    contador++;
                    sw.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25};{26};{27};{28};{29};{30};{31};{32};{33};{34};{35};{36};{37};{38};{39};{40};{41};",

                    "BO_LEASING_" + dataInicio.ToString().Substring(0, 10) + "_" + dataFim.ToString().Substring(0, 10) + "_" + contador,
                    "PAG IPVA, CHASSI:" + elemento.chassi,
                    elemento.contrato,
                    (elemento.grupo_safra == "0514" ? "BANCO SAFRA S/A" : (elemento.grupo_safra == "0658" ? "BANCO J. SAFRA S/A" : (elemento.grupo_safra == "0521" ? "SAFRA LEASING S/A ARRENDAMENTO MERCANTIL" : (elemento.grupo_safra == "0000" ? "JS ADM RECURSOS" : "")))),
                    elemento.grupo_safra,
                    elemento.descricao_agencia,
                    elemento.agencia,
                    "7 - Práticas Inadequadas relativas a contrapartes, clientes, produtos e serviços",
                    "7",
                    "7_01 - Perdas na relação de negócios com clientes",
                    "7_01",
                    "7_01_03 - Outras perdas decorrentes da relação de negócios com clientes",
                    "7_01_03",
                    "Eventos Externos",
                    "EVEX",
                    "Cliente",
                    "CLIE",
                    "Não pagamento de IPVA contratos Leasing",
                    "",
                    "Arrendamento Mercantil - Sicred",
                    "ARM",
                    "LEASING",
                    "104010000",
                    "Agências",
                    "1",
                    "Perda Efetiva",
                    elemento.dta_pagamento.HasValue ? elemento.dta_pagamento.ToString().Substring(0,10) : "",
                    elemento.dta_cobranca.HasValue ? elemento.dta_cobranca.ToString().Substring(0, 10) : "",
                    "Crédito",
                    "C",
                    "BO LEASING PESADOSPOA",
                    "37470",
                    "Banco Comercial",
                    "BCOCOM",
                    "Bancos",
                    "BANCOS",
                    "PERDA_DIRETA",
                    "Perda Financeira Direta",
                    "PEPD",
                    elemento.dta_contabil_divida.HasValue ? elemento.dta_contabil_divida.ToString().Substring(0, 10) : "",
                    "R$ " + elemento.valor_divida,
                    elemento.pci_debito_divida
                    ));
                }
                Response.Write(sw.ToString());
                Response.End();
            }
            else
            {
                //Gera Relatório impresso em Landscape para poder caber todas as 27 colunas.
                return View("RelatorioPerdasOperacionais", model);
            }

            return View(GetContratosVeiculosViewModelPrimeira());
        }


        public ActionResult NotificacaoDeDebitosDeIPVAParaClientes()
        {

            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato

                     from c in db.Tbl_DebitosEPagamentos_Veiculo.Where(depv =>
                     (b.chassi == depv.chassi) || (b.renavam == depv.renavam) || (b.placa == depv.placa))

                     from d in db.Tbl_Bens.Where(bens =>
                     (b.chassi == bens.chassi) || (b.renavam == bens.renavam) || (b.placa == bens.placa)).DefaultIfEmpty() //Isto é um LEFT JOIN
                     //join d in db.Tbl_Bens
                     //on new { b.chassi, b.renavam, b.placa } equals new { d.chassi, d.renavam, d.placa }
                     //into j2
                     //from d in j2.DefaultIfEmpty() //Isto é um LEFT JOIN pra trazer quem esta na Bens e quem não está também.

                     //join e in db.Tbl_CCL
                     //on a.cpf_cnpj_cliente equals e.cpf_cnpj_cliente

                     //where (c.pagamento_efet_banco == false || c.pagamento_efet_banco == null)

                     //where a.origem.Equals("B")
                     //where (!b.origem.Contains("RECIBO VEN") || b.origem == null)

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
