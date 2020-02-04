using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vidly
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // \note old way to adding custom route
            // will match http://localhost:62444/movies/released/2018/09 
            // http://localhost:62444/movies/released?year=2018&month=09
            //routes.MapRoute(
            //    name: "MoviesByReleaseDate  ",
            //    url: "movies/released/{year}/{month}",
            //    defaults: new { controller = "Movies", action = "ByReleaseDate", year = UrlParameter.Optional, month = UrlParameter.Optional }
            //    //, new { year = @"\d{4}", month = @"\d{2}" } // to enable constraint remove the "optional" above but note that query string will not match in that case
            //);

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
