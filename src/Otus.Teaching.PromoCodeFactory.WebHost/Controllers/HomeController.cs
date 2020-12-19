using Microsoft.AspNetCore.Mvc;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    public class HomeController : ControllerBase
    {
        [Route(""), HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public RedirectResult RedirectToSwaggerUi()
        {
            return Redirect("/swagger/");
        }
    }
}