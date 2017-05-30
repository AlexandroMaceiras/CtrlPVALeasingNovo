using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CtrlPVALeasing.Models;

namespace CtrlPVALeasing.Controllers
{
    public class HomeController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        IEnumerable<TesteViewModel> model = null;
        IEnumerable<TesteViewModel> model1 = null;
        IEnumerable<TesteViewModel> model2 = null;

        public ActionResult Index()
        {
            model1 = (from a in db.Arm_LiquidadosEAtivos_Contrato
                      join b in db.Arm_Veiculos
                      on a.contrato equals b.contrato
                      where a.origem.Equals("B")
                      where !b.origem.Contains("RECIBO VEN")
                      group a by new { a.status } into g
                      select new
                      {
                          statusGoup = g.Key,
                          totalGoup = g.Count(c => c.status.HasValue)

                      }).AsEnumerable().Select(x => new TesteViewModel
                      {
                          status = x.statusGoup.status,
                          total = x.totalGoup
                      }).ToList();

            model2 = (from a in db.Arm_LiquidadosEAtivos_Contrato
                      join b in db.Arm_Veiculos
                      on a.contrato equals b.contrato
                      where a.origem.Equals("B")
                      where !b.origem.Contains("RECIBO VEN")
                      group a by new { b.status } into g
                      select new
                      {
                          statusGoup = g.Key,
                          totalGoup = g.Count(c => c.status.HasValue)

                      }).AsEnumerable().Select(x => new TesteViewModel
                      {
                          status2 = x.statusGoup.status,
                          total2 = x.totalGoup
                      }).ToList();

            return View(model1.Concat(model2));
        }
        public ActionResult Index_antigo()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "A página de descrição da sua aplicação.  ";

            return View();
        }
        public ActionResult Consulta()
        {
            ViewBag.Message = "A página de consulta.  ";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Sua página de contato.";

            return View();
        }

        public ActionResult Grafico()
        {
            model1 = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato
                     where a.origem.Equals("B")
                     where !b.origem.Contains("RECIBO VEN")
                     group a by new { a.status } into g
                     select new
                     {
                         statusGoup = g.Key,
                         totalGoup = g.Count(c => c.status.HasValue)

                     }).AsEnumerable().Select(x => new TesteViewModel
                     {
                         status = x.statusGoup.status,
                         total = x.totalGoup
                     }).ToList();

            model2 = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato
                     where a.origem.Equals("B")
                     where !b.origem.Contains("RECIBO VEN")
                     group a by new { b.status } into g
                     select new
                     {
                         statusGoup = g.Key,
                         totalGoup = g.Count(c => c.status.HasValue)

                     }).AsEnumerable().Select(x => new TesteViewModel
                     {
                         status2 = x.statusGoup.status,
                         total2 = x.totalGoup
                     }).ToList();

                return PartialView(model1.Concat(model2));
        }

        public ActionResult _graficoContratos()
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato
                     where a.origem.Equals("B")
                     where !b.origem.Contains("RECIBO VEN")
                     group a by new { a.status } into g
                     select new
                     {
                         statusGoup = g.Key,
                         totalGoup = g.Count(c => c.status.HasValue)

                     }).AsEnumerable().Select(x => new TesteViewModel
                     {
                         status = x.statusGoup.status,
                         total = x.totalGoup
                     });

            if (model.Count() == 0 || model == null)
            {
                return View();
            }

            if (model == null || model.Any() == false)
            {
                return View("_graficoContratos",model);
            }

            var myChart = new System.Web.Helpers.Chart(width: 230, height: 118, theme: System.Web.Helpers.ChartTheme.Vanilla3D)
                //.AddTitle("CONTRATOS")
                //.AddLegend("LEGENDA")
                //.Save(@"H:\Downloads\grafico.jpg")
                //.AddSeries(chartArea:"teste")
                .AddSeries("Default", chartType: "Pie", xValue: model.ToList(), xField: "status", yValues: model.ToList(), yFields: "total")
                .Write();

            return null;
        }

        public ActionResult _graficoVeiculos()
        {
            model = (from a in db.Arm_LiquidadosEAtivos_Contrato
                     join b in db.Arm_Veiculos
                     on a.contrato equals b.contrato
                     where a.origem.Equals("B")
                     where !b.origem.Contains("RECIBO VEN")
                     group a by new { b.status } into g
                     select new
                     {
                         statusGoup = g.Key,
                         totalGoup = g.Count(c => c.status.HasValue)

                     }).AsEnumerable().Select(x => new TesteViewModel
                     {
                         status = x.statusGoup.status,
                         total = x.totalGoup
                     });

            if (model.Count() == 0 || model == null)
            {
                return View();
            }

            if (model == null || model.Any() == false)
            {
                return View();
            }

            var myChart = new System.Web.Helpers.Chart(width: 230, height: 118, theme: System.Web.Helpers.ChartTheme.Vanilla3D)
                //.AddTitle("CONTRATOS")
                //.AddLegend("LEGENDA")
                //.Save(@"H:\Downloads\grafico.jpg")
                //.AddSeries(chartArea:"teste")
                .AddSeries("Default", chartType: "Pie", xValue: model.ToList(), xField: "status", yValues: model.ToList(), yFields: "total")
                .Write();

            return null;
        }
    }
}