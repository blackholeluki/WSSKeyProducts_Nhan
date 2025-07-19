using System.Web;
using System.Web.Mvc;

namespace DoHoangNhan_2211110151_DeAnWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
