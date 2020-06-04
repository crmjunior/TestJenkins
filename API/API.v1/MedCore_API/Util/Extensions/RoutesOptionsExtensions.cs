using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace MedCoreAPI.Util.Extensions
{
    public static class RoutesOptionsExtensions
    {
        public static void UseGeneralRoutePrefix(this MvcOptions mvcOptions, IRouteTemplateProvider routeAttribute)
        {
            mvcOptions.Conventions.Add(new RoutePrefixConvention(routeAttribute));
        }

        public static void UseGeneralRoutePrefix(this MvcOptions mvcOptions, string prefix)
        {
            mvcOptions.UseGeneralRoutePrefix(new RouteAttribute(prefix));
        }
    }
}