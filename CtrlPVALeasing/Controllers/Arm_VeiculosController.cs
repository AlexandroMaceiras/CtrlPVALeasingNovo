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
    public class Arm_VeiculosController : Controller
    {
        private CtrlIPVALeasingContext db = new CtrlIPVALeasingContext();

        // GET: Arm_Veiculos
        public ActionResult Index()
        {
            return View(db.Arm_Veiculos.OrderByDescending(x => x.id).Take(100).ToList());
        }

        // GET: Arm_Veiculos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arm_Veiculos arm_Veiculos = db.Arm_Veiculos.Find(id);
            if (arm_Veiculos == null)
            {
                return HttpNotFound();
            }
            return View(arm_Veiculos);
        }

        // GET: Arm_Veiculos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Arm_Veiculos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,contrato,tipo_registro,marca,modelo,tipo,ano_fab,ano_mod,cor,renavam,chassi,placa,origem,status,comunicado_venda")] Arm_Veiculos arm_Veiculos)
        {
            if (ModelState.IsValid)
            {
                db.Arm_Veiculos.Add(arm_Veiculos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(arm_Veiculos);
        }

        // GET: Arm_Veiculos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arm_Veiculos arm_Veiculos = db.Arm_Veiculos.Find(id);
            if (arm_Veiculos == null)
            {
                return HttpNotFound();
            }
            return View(arm_Veiculos);
        }

        // POST: Arm_Veiculos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,contrato,tipo_registro,marca,modelo,tipo,ano_fab,ano_mod,cor,renavam,chassi,placa,origem,status,comunicado_venda")] Arm_Veiculos arm_Veiculos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arm_Veiculos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(arm_Veiculos);
        }

        // GET: Arm_Veiculos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arm_Veiculos arm_Veiculos = db.Arm_Veiculos.Find(id);
            if (arm_Veiculos == null)
            {
                return HttpNotFound();
            }
            return View(arm_Veiculos);
        }

        // POST: Arm_Veiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Arm_Veiculos arm_Veiculos = db.Arm_Veiculos.Find(id);
            db.Arm_Veiculos.Remove(arm_Veiculos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }












        // GET: Arm_LiquidadosEAtivos_Contrato/Create
        public ActionResult CriarVeiculo()
        {
            return View();
        }

        // POST: Arm_LiquidadosEAtivos_Contrato/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CriarVeiculo([Bind(Include = "id,contrato,tipo_registro,marca,modelo,tipo,ano_fab,ano_mod,cor,renavam,chassi,placa")] Arm_Veiculos arm_Veiculos)
        {
            try
            {
                // Controle de erros do ModelState
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

                if (arm_Veiculos.chassi == null)
                    arm_Veiculos.chassi = "ø";
                if (arm_Veiculos.renavam == null)
                    arm_Veiculos.renavam = "ø";
                if (arm_Veiculos.placa == null)
                    arm_Veiculos.placa = "ø";
                if (arm_Veiculos.chassi != "ø" || arm_Veiculos.renavam != "ø" || arm_Veiculos.placa != "ø")
                {
                    if (ModelState.IsValid)
                    {
                        bool valido1 = false;
                        bool valido2 = false;
                        bool valido3 = false;

                        ModelState.Clear();

                        if (db.Arm_Veiculos.Where(c => c.chassi == arm_Veiculos.chassi).Count() == 0)
                            valido1 = true;

                        if (db.Arm_Veiculos.Where(d => d.renavam == arm_Veiculos.renavam).Count() == 0)
                            valido2 = true;

                        if (db.Arm_Veiculos.Where(e => e.placa == arm_Veiculos.placa).Count() == 0)
                            valido3 = true;

                        if (valido1 && valido2 && valido3)
                        {
                            //Transforma tudo pra maiúsculas. 
                            if (arm_Veiculos.cor != null)
                                arm_Veiculos.cor = arm_Veiculos.cor.ToUpper();
                            if (arm_Veiculos.marca != null)
                                arm_Veiculos.marca = arm_Veiculos.marca.ToUpper();
                            if (arm_Veiculos.modelo != null)
                                arm_Veiculos.modelo = arm_Veiculos.modelo.ToUpper();
                            if (arm_Veiculos.tipo != null)
                                arm_Veiculos.tipo = arm_Veiculos.tipo.ToUpper();

                            if (arm_Veiculos.chassi != null && arm_Veiculos.chassi != "" && arm_Veiculos.chassi != "ø")
                                arm_Veiculos.chassi = arm_Veiculos.chassi.ToUpper();
                            else
                                arm_Veiculos.chassi = null;


                            if (arm_Veiculos.renavam != null && arm_Veiculos.renavam != "" && arm_Veiculos.renavam != "ø")
                                arm_Veiculos.renavam = arm_Veiculos.renavam.ToUpper();
                            else
                                arm_Veiculos.renavam = null;


                            if (arm_Veiculos.placa != null && arm_Veiculos.placa != "" && arm_Veiculos.placa != "ø")
                                arm_Veiculos.placa = arm_Veiculos.placa.ToUpper();
                            else
                                arm_Veiculos.placa = null;


                            arm_Veiculos.flag_manual = true;

                            db.Arm_Veiculos.Add(arm_Veiculos);
                            db.SaveChanges();
                            ViewBag.Message = "Incluido com Sucesso!";
                        }

                        if ((arm_Veiculos.chassi == "" && arm_Veiculos.renavam == "" && arm_Veiculos.placa == "")
                            || (arm_Veiculos.chassi == "ø" && arm_Veiculos.renavam == "ø" && arm_Veiculos.placa == "ø"))
                        {
                            ViewBag.Message = "Erro: O chassi o renavam ou a placa é obrigatório!";
                        }
                        else if (valido1 && valido2 && valido3)
                        {
                            ViewBag.Message = "Incluido com Sucesso!";
                        }
                        else
                        {
                            ViewBag.Message = "Erro: Este Veículo já está cadastrado!";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Erro: Algum campo está inválido!";
                    }
                }
                else
                {
                    ViewBag.Message = "Erro: O chassi o renavam ou a placa é obrigatório!";
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.InnerException.ToString();
            }
            return View();
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
