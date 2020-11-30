using System.Web.Mvc;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Models;
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
            Account account = AccountDAO.GetByEmail(email);
            if (Authentication.IsSigned()) return Redirect("/Home/Index");
            bool loginIsValid = AccountDAO.Login(email, password);
            if (loginIsValid) {

                SJWT.SJWTConfig("5cnp0sod2rlt8bpi0y5g1925taileae67sp2uh6qhzncxgnm55ztfh", new {
                    Alg = "SHA256",
                    Typ = "SJWT",
                    account.Id,
                    account.Email
                });
                Authentication.SignIn();

                //if (remember_me == "true") {
                //    Authentication.RememberMe();
                //}
            }
            return Json(new { isValid = loginIsValid, url = "/Home/Index", message = "Conta não existe", type = 1 });
        }

        [HttpPost]
        public ActionResult Signup(string email, string password) {
            Account account = AccountDAO.GetByEmail(email);
            if (Authentication.IsSigned()) return Redirect("/Home/Index");
            dynamic response = AccountDAO.Insert(email, password);
            return Json(new { message = response.Value, type = response.Type });
        }

        public ActionResult Logout() {
            Authentication.SignOut();
            return RedirectToAction("Account");
        }
    }
}