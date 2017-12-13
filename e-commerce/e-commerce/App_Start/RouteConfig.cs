using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace e_commerce
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ProductCreate",
                url: "Products/Create",
                defaults: new { controller = "Products", action = "Create"});

            routes.MapRoute(
                name: "ProductByCategoryByPage",
                url: "Products/{category}/Page{page}",
                defaults: new { controller = "Products", action = "Index" });

            routes.MapRoute(
                name: "ProductPages",
                url: "Products/Page{page}",
                defaults: new { controller = "Products", action = "Index" });

            routes.MapRoute(
                name: "ProductCategory",
                url: "Products/{category}",
                defaults: new { controller="Products", action = "Index", category = UrlParameter.Optional});

            routes.MapRoute(
                name: "ProductIndex",
                url: "Products",
                defaults: new { controller = "Products", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            
        }
    }
}
