using System.Web.Mvc;
using TCC_LOSPACO.DAO;

namespace TCC_LOSPACO.Controllers {
    public class EmployeeController : Controller {
        [HttpPost]
        public ActionResult Get(string email) {
            return Json(new { employee = EmployeeDAO.GetByEmail(email) });
        }
    }
}