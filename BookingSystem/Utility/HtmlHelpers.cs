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
        public static string TestimonialStatus(this IHtmlHelper html, string status)
        {
            return status.Equals(SD.Testimonial_Pending) ? "table-warning" : status.Equals(SD.Testimonial_Approved) ? "table-success" : "table-danger";
        }
        public static string BookingStatus(this IHtmlHelper html, string status)
        {
            return status.Equals(SD.BookingStatus_Confirmed) ?
                "badge badge-success" :
                status.Equals(SD.BookingStatus_Cancelled) ?
                "badge badge-danger" :
                status.Equals(SD.BookingStatus_Pending) ?
                "badge badge-warning" :
                status.Equals(SD.BookingStatus_CheckedOut) || status.Equals(SD.BookingStatus_CheckedIn) ?
                "badge badge-info":"";
        }
    }

}
