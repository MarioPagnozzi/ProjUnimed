﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;

namespace SisUnimed.Controllers
{
    public class OperadoraController : Controller
    {
        //
        // GET: /Operadora/

        public ActionResult Operadora()
        {
            if (Session["usuariologadoId"] != null)
            {
                using (UnimedEntities1 up = new UnimedEntities1())
                {
                    int usuario_id = int.Parse(Session["usuariologadoId"].ToString());
                    var resultado = up.usuario_permissao.Where(a => a.id_usuario.Equals(usuario_id)).FirstOrDefault();

                    ViewData["usuario_permissao"] = resultado;
                }
                ViewBag.Titulo = "Cadastro de Operadora";

                return View();


            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

    }
}
