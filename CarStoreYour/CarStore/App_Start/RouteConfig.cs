using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            Route route = new Route(
              "{controller}/{action}",
              new RouteValueDictionary(new { Controller = "Account", Action = "Login" }),
              new MvcRouteHandler());
            routes.Add("Default", route);

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null,
                "",
                new
                {
                    controller = "Car",
                    action = "List",
                    category = (string)null,
                    page = 1
                }
            );

            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Car", action = "List", category = (string)null },
                constraints: new { page = @"\d+" }
            );

            routes.MapRoute(null,
                "{category}",
                new { controller = "Car", action = "List", page = 1 }
            );

            routes.MapRoute(null,
                "{category}/Page{page}",
                new { controller = "Car", action = "List" },
                new { page = @"\d+" }
            );

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
