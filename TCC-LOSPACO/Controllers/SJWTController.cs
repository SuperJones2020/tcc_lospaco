using System.Web.Mvc;
using TCC_LOSPACO.Models;
using TCC_LOSPACO.Security;

namespace TCC_LOSPACO.Controllers {
    public class SJWTController : Controller {
        [HttpPost]
        public ActionResult GenerateSignature() {
            if (!Authentication.IsSigned()) return Json(new { Error = "Need to login" });
            Customer user = Authentication.GetUser();
            return Json(new { token = SJWT.GenerateToken(user.Account.Id, user.Account.Email, user.Account.Password) });
        }
    }
}