using System.Web.Mvc;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Security;

namespace TCC_LOSPACO.Controllers {
    public class AccountController : Controller {
        // GET: Account
        public ActionResult Account() {
            if (Authentication.IsSigned()) return Redirect("/Home/Index");
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password, string remember_me) {
            if (Authentication.IsSigned()) return Redirect("/Home/Index");
            bool loginIsValid = AccountDAO.Login(email, password);
            if (loginIsValid) {
                Authentication.SignIn(AccountDAO.GetByEmail(email).Id);
                if (remember_me == "true") {
                    Authentication.RememberMe();
                }
            }
            return Json(new { isValid = loginIsValid, url = "/Home/Index", message = "Conta não existe", type = 1 });
        }

        [HttpPost]
        public ActionResult Signup(string email, string password) {
            if (Authentication.IsSigned()) return Redirect("/Home/Index");
            dynamic response = AccountDAO.Insert(email, password);
            return Json(new { message = response.Value, type = response.Type });
        }

        [HttpPost]
        public ActionResult Logout() {
            Authentication.SignOut();
            return RedirectToAction("Account");
        }
    }
}