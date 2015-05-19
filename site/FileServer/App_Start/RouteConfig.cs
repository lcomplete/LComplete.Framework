using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FileServer
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Img",
                url: "img/{uniqueKey}/{thumb}",
                defaults: new { controller = "File", action = "img", thumb = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "static",
                url: "static/{uniqueKey}",
                defaults: new { controller = "File", action = "static" }
            );
        }
    }
}