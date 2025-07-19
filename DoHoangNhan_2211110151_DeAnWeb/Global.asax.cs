using DoHoangNhan_2211110151_DeAnWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DoHoangNhan_2211110151_DeAnWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start()
        {
            giohang giohang = new giohang();
            Session["GIOHANG"] = giohang;
            Session["SOLUONGMATHANG"] = 0;
        }
        protected void Session_End()
        {
            Session.Remove("GIOHANG");
            Session.Remove("SOLUONGMATHANG");
        }
    }
}
