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

        private void ModelGeral()
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
        }

        public ActionResult Index()
        {
            ModelGeral();
            return View(model1.Concat(model2));
        }
        public ActionResult Index_antigo()
        {
            return View();
        }

        public ActionResult Sobre()
        {
            ViewBag.Message = "Sistema de Controle de IPVA em Leasing de Veículos do Banco SAFRA.";

            return View();
        }

        public ActionResult Contato()
        {
            ViewBag.Message = "Entre em contato para suporte e ampliação do sistema.";

            return View();
        }

        /// <summary>
        /// Este só serve para testar o Gráfico chamando ele por Home/Grafico.
        /// </summary>
        /// <returns></returns>
        public ActionResult Grafico()
        {
            ModelGeral();
            return PartialView(model1.Concat(model2));
        }

        public const string Vanilla3D3 = "<Chart BackColor=\"#555\" BackGradientStyle=\"TopBottom\" BackSecondaryColor=\"White\" BorderColor=\"26, 59, 105\" BorderWidth=\"20\" BorderlineDashStyle=\"Solid\" Palette=\"BrightPastel\" AntiAliasing=\"All\">\r\n    <ChartAreas>\r\n        <ChartArea Name=\"Default\" _Template_=\"All\" BackColor=\"Transparent\" BackSecondaryColor=\"White\" BorderColor=\"121, 163, 196\" BorderDashStyle=\"Solid\" ShadowColor=\"Transparent\">\r\n            <Area3DStyle LightStyle=\"Simplistic\" Enable3D=\"True\" Inclination=\"55\" IsClustered=\"False\" IsRightAngleAxes=\"False\" Perspective=\"10\" Rotation=\"-30\" WallWidth=\"0\" />\r\n        </ChartArea>\r\n    </ChartAreas>\r\n</Chart>";
        public const string Vanilla3D2 = "<Chart BackColor=\"#D3DFF0\" BackGradientStyle=\"TopBottom\" BackSecondaryColor=\"White\" BorderColor=\"26, 59, 105\" BorderWidth=\"20\" BorderlineDashStyle=\"Solid\" Palette=\"BrightPastel\" AntiAliasing=\"All\">\r\n    <ChartAreas>\r\n        <ChartArea Name=\"Default\" _Template_=\"All\" BackColor=\"Transparent\" BackSecondaryColor=\"White\" BorderColor=\"121, 163, 196\" BorderDashStyle=\"Solid\" ShadowColor=\"Transparent\">\r\n            <Area3DStyle LightStyle=\"Simplistic\" Enable3D=\"True\" Inclination=\"55\" IsClustered=\"False\" IsRightAngleAxes=\"False\" Perspective=\"10\" Rotation=\"-30\" WallWidth=\"0\" />\r\n        </ChartArea>\r\n    </ChartAreas>\r\n</Chart>";

        public const string Blue =      "<Chart BackColor=\"#D3DFF0\" BackGradientStyle=\"TopBottom\" BackSecondaryColor=\"White\" BorderColor=\"26, 59, 105\" BorderlineDashStyle=\"Solid\" BorderWidth=\"2\" Palette=\"BrightPastel\">\r\n    <ChartAreas>\r\n        <ChartArea Name=\"Default\" _Template_=\"All\" BackColor=\"64, 165, 191, 228\" BackGradientStyle=\"TopBottom\" BackSecondaryColor=\"White\" BorderColor=\"64, 64, 64, 64\" BorderDashStyle=\"Solid\" ShadowColor=\"Transparent\" /> \r\n    </ChartAreas>\r\n    <Legends>\r\n        <Legend _Template_=\"All\" BackColor=\"Transparent\" Font=\"Trebuchet MS, 8.25pt, style=Bold\" IsTextAutoFit=\"False\" /> \r\n    </Legends>\r\n    <BorderSkin SkinStyle=\"Emboss\" /> \r\n  </Chart>";
        public const string Vanilla3D = "<Chart BackColor=\"#555\" BackGradientStyle=\"TopBottom\" BorderColor=\"181, 64, 1\" BorderWidth=\"2\" BorderlineDashStyle=\"Solid\" Palette=\"SemiTransparent\" AntiAliasing=\"All\">\r\n    <ChartAreas>\r\n        <ChartArea Name=\"Default\" _Template_=\"All\" BackColor=\"Transparent\" BackSecondaryColor=\"White\" BorderColor=\"64, 64, 64, 64\" BorderDashStyle=\"Solid\" ShadowColor=\"Transparent\">\r\n            <Area3DStyle LightStyle=\"Simplistic\" Enable3D=\"True\" Inclination=\"30\" IsClustered=\"False\" IsRightAngleAxes=\"False\" Perspective=\"10\" Rotation=\"-30\" WallWidth=\"0\" />\r\n        </ChartArea>\r\n    </ChartAreas>\r\n</Chart>";
        public const string Green = "<Chart BackColor=\"#C9DC87\" BackGradientStyle=\"TopBottom\" BorderColor=\"181, 64, 1\" BorderWidth=\"2\" BorderlineDashStyle=\"Solid\" Palette=\"BrightPastel\">\r\n  <ChartAreas>\r\n    <ChartArea Name=\"Default\" _Template_=\"All\" BackColor=\"Transparent\" BackSecondaryColor=\"White\" BorderColor=\"64, 64, 64, 64\" BorderDashStyle=\"Solid\" ShadowColor=\"Transparent\">\r\n      <AxisY LineColor=\"64, 64, 64, 64\">\r\n        <MajorGrid Interval=\"Auto\" LineColor=\"64, 64, 64, 64\" />\r\n        <LabelStyle Font=\"Trebuchet MS, 8.25pt, style=Bold\" />\r\n      </AxisY>\r\n      <AxisX LineColor=\"64, 64, 64, 64\">\r\n        <MajorGrid LineColor=\"64, 64, 64, 64\" />\r\n        <LabelStyle Font=\"Trebuchet MS, 8.25pt, style=Bold\" />\r\n      </AxisX>\r\n      <Area3DStyle Inclination=\"15\" IsClustered=\"False\" IsRightAngleAxes=\"False\" Perspective=\"10\" Rotation=\"10\" WallWidth=\"0\" />\r\n    </ChartArea>\r\n  </ChartAreas>\r\n  <Legends>\r\n    <Legend _Template_=\"All\" Alignment=\"Center\" BackColor=\"Transparent\" Docking=\"Bottom\" Font=\"Trebuchet MS, 8.25pt, style=Bold\" IsTextAutoFit =\"False\" LegendStyle=\"Row\">\r\n    </Legend>\r\n  </Legends>\r\n  <BorderSkin SkinStyle=\"Emboss\" />\r\n</Chart>";
        public const string Vanilla = "<Chart Palette=\"SemiTransparent\" BorderColor=\"#000\" BorderWidth=\"2\" BorderlineDashStyle=\"Solid\">\r\n<ChartAreas>\r\n    <ChartArea _Template_=\"All\" Name=\"Default\">\r\n            <AxisX>\r\n                <MinorGrid Enabled=\"False\" />\r\n                <MajorGrid Enabled=\"False\" />\r\n            </AxisX>\r\n            <AxisY>\r\n                <MajorGrid Enabled=\"False\" />\r\n                <MinorGrid Enabled=\"False\" />\r\n            </AxisY>\r\n    </ChartArea>\r\n</ChartAreas>\r\n</Chart>";
        public const string Yellow = "<Chart BackColor=\"#FADA5E\" BackGradientStyle=\"TopBottom\" BorderColor=\"#B8860B\" BorderWidth=\"2\" BorderlineDashStyle=\"Solid\" Palette=\"EarthTones\">\r\n  <ChartAreas>\r\n    <ChartArea Name=\"Default\" _Template_=\"All\" BackColor=\"Transparent\" BackSecondaryColor=\"White\" BorderColor=\"64, 64, 64, 64\" BorderDashStyle=\"Solid\" ShadowColor=\"Transparent\">\r\n      <AxisY>\r\n        <LabelStyle Font=\"Trebuchet MS, 8.25pt, style=Bold\" />\r\n      </AxisY>\r\n      <AxisX LineColor=\"64, 64, 64, 64\">\r\n        <LabelStyle Font=\"Trebuchet MS, 8.25pt, style=Bold\" />\r\n      </AxisX>\r\n    </ChartArea>\r\n  </ChartAreas>\r\n  <Legends>\r\n    <Legend _Template_=\"All\" BackColor=\"Transparent\" Docking=\"Bottom\" Font=\"Trebuchet MS, 8.25pt, style=Bold\" LegendStyle=\"Row\">\r\n    </Legend>\r\n  </Legends>\r\n  <BorderSkin SkinStyle=\"Emboss\" />\r\n</Chart>";


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

            var myChart = new System.Web.Helpers.Chart(width: 230, height: 118, theme: Vanilla3D3)
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

            var myChart = new System.Web.Helpers.Chart(width: 230, height: 118, theme: Vanilla3D3)//System.Web.Helpers.ChartTheme.Vanilla3D)
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