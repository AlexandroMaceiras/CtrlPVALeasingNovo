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
    public class RelatorioController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        IEnumerable<ContratosVeiculosViewModel> model = null;

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
