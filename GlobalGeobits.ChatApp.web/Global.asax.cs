using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GlobalGeobits.ChatApp.web
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

        void Application_EndRequest(object sender, System.EventArgs e)
        {
            // If the user is not authorised to see this page or access this function, send them to the error page.
            if (Response.StatusCode == 401)
            {
                Response.ClearContent();
              
                Response.RedirectToRoute("login", (RouteTable.Routes["login"] as Route).Defaults);
            }
        }

         void Session_End(Object sender, EventArgs e)
        {
            if (Session["user_id"] != null)
            {
                int id = int.Parse(Session["user_id"].ToString());
                using (Models.ChatAppDbContext db = new Models.ChatAppDbContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.UserID == id);
                    if (user != null)
                    {
                        user.Status = 0;
                        db.SaveChanges();
                        
                    }
                }
                Session.Abandon();
            }
        }
       

    }
}
