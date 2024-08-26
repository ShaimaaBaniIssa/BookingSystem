using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingSystem.Utility
{
    public static class HtmlHelpers
    {
        public static string IsActive(this IHtmlHelper html, string controller, string action)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = routeData.Values["action"]?.ToString();
            var routeController = routeData.Values["controller"]?.ToString();

            return (controller == routeController && action == routeAction) ? "active" : "";
        }
    }

}
