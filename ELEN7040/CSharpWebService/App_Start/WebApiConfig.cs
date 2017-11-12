using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CSharpWebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ActionAPI",
                routeTemplate: "api/{controller}/{action}/{recordLimit}/{threads}",
                defaults: new { controller = "Home", action = "Index", recordLimit = 1, threads = 1 }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
