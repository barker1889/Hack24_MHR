﻿using System.Web.Mvc;
using System.Web.Routing;

namespace DataDisplay
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "AddFeedback",
                "AddFeedback",
                new { controller = "Home", action = "Feedback" }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );        
        }
    }
}
