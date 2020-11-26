using System.Web.Mvc;
using TCC_LOSPACO.DAO;

namespace TCC_LOSPACO.Controllers {
    public class AccountController : Controller {
        // GET: Account
        public ActionResult Account() {
            if (Security.Context.IsSigned()) return Redirect("/Home/Index");
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password, string remember_me) {
            if (Security.Context.IsSigned()) return Redirect("/Home/Index");
            bool loginIsValid = AccountDAO.Login(email, password);
            if (loginIsValid) {
                Security.Context.SignIn(email);
                if (remember_me == "true") {
                    Security.Context.RememberMe();
                }
            }
            return Json(new { isValid = loginIsValid, url = "/Home/Index", message = "Conta não existe", type = 1 });
        }

        [HttpPost]
        public ActionResult Signup(string email, string password) {
            if (Security.Context.IsSigned()) return Redirect("/Home/Index");
            dynamic response = AccountDAO.Insert(email, password);
            return Json(new { message = response.Value, type = response.Type });
        }

        public ActionResult Logout() {
            Security.Context.SignOut();
            return RedirectToAction("Account");
        }
    }
}