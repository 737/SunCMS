using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace Sun.API
{
    public class APIHelper : HttpConfiguration
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultAPI",
                routeTemplate: "api/{controller}/{sKey}",
                defaults: new { sKey = RouteParameter.Optional }
            );
        }

    }
}
