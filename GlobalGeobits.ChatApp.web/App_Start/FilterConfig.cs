using System.Web;
using System.Web.Mvc;

namespace GlobalGeobits.ChatApp.web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
