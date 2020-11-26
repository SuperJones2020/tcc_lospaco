using System.Web.Mvc;
using TCC_LOSPACO.DAO;

namespace TCC_LOSPACO.Controllers {
    public class CustomerController : Controller {
        [HttpPost]
        public ActionResult Get(string email) {
            return Json(new { customer = CustomerDAO.GetByEmail(email) });
        }
    }
}