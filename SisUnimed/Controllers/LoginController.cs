using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SisUnimed.Models;


namespace SisUnimed.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/        
        public ActionResult Login()
        {            
            Npgsql.NpgsqlConnection.ClearAllPools();        
            return View();
        }
        [HttpPost]        
        [ValidateAntiForgeryToken]
        public ActionResult Login(usuario u)
        {
            
            
            if (ModelState.IsValidField("email_usuario") && ModelState.IsValidField("senha_usuario"))
            {
                using (UnimedEntities1 ul = new UnimedEntities1())
                {
                    var v = ul.usuarios.Where(a => a.email_usuario.Equals(u.email_usuario) && a.senha_usuario.Equals(u.senha_usuario)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["usuariologadoid"] = v.id.ToString();
                        Session["usuariologadonome"] = v.nome_usuario.ToString();
                        Session["horalogado"] = DateTime.Now.ToString("HH:mm:ss");
                        Session["datalogado"] = DateTime.Today.ToString("dd/MM/yyyy");
                        return RedirectToAction("Index","Home");
                    }
                    ViewBag.Message = "Usuário e/ou Senha Inválido";
                    
                }
                
            }
            
            return View(u);
        }

    }
}
