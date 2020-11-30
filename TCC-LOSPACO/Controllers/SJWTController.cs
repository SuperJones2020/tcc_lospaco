using System.Web.Mvc;

namespace TCC_LOSPACO.Controllers {
    public class SJWTController : Controller {
        [HttpPost]
        public ActionResult Get() {
            return Json(new { token = Security.Authentication.GetToken() });
        }


    }
}