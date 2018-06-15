using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http;
using System.Collections.Generic;
using System.Web.Http.Cors;

namespace XFifaFantasy
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web



            // Rutas de API web
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id= RouteParameter.Optional}
                );

            
        
        }
        /*
        public static void AddRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "Post",
                routeTemplate: "api/{controller}/post/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
            config.Routes.MapHttpRoute(
                name: "Update",
                routeTemplate: "api/{controller}/update/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
           

        }
        */


      
    }
}
