using System.Web.Mvc;

namespace Obads2.Areas.Doctor
{
    public class DoctorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Doctor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Doctor_default",
                "Doctor/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "Obads2.Areas.Doctor.Controllers" }
            );
        }
    }
}