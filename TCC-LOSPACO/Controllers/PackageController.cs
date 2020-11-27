using System.Web.Mvc;
using TCC_LOSPACO.DAO;

namespace TCC_LOSPACO.Controllers {
    public class PackageController : Controller {
        [HttpPost]
        public ActionResult Get(ushort id) {
            return Json(new { package = PackageDAO.GetById(id) });
        }
        [HttpPost]
        public ActionResult GetServices(ushort id) {
            return Json(new { services = PackageDAO.GetServicesFromPackage(id) });
        }
    }
}