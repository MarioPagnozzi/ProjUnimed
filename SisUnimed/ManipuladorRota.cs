using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SisUnimed
{
    public class MeuManipuladorDeRota : MvcRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var url = requestContext.HttpContext.Request.Path.TrimStart('/');          
                      
            if (!string.IsNullOrEmpty(url))
            {
                ItemDePagina item = GerenciadorDeRedirecionamento.ObterPaginaPorUrl(url);
                if (item != null)
                {
                    MontarRequisicao(item.Controller ?? "Login",
                        item.Action ?? "Login",
                        item.ConteudoId.ToString(),
                        requestContext);
                }               
            }
             

            return base.GetHttpHandler(requestContext);            
        }

        private static void MontarRequisicao(string controller, string action, string id, RequestContext requestContext)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            requestContext.RouteData.Values["controller"] = controller;
            //requestContext.RouteData.Values["action"] = action;
            //requestContext.RouteData.Values["id"] = id;
        }
    }
}