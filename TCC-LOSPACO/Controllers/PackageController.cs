using System.Web.Mvc;
using TCC_LOSPACO.DAO;

namespace TCC_LOSPACO.Controllers {
    public class PackageController : Controller {
        [HttpPost]
        public ActionResult Get(string name) {
            return Json(new { package = PackageDAO.GetByName(name) });
        }
        [HttpPost]
        public ActionResult GetServices(string name) {
            return Json(new { services = PackageDAO.GetServicesFromPackage(name) });
        }
    }
}