using System.Web.Mvc;
using TCC_LOSPACO.DAO;

namespace TCC_LOSPACO.Controllers {
    public class ScheduleController : Controller {
        [HttpPost]
        public ActionResult Get(uint id) {
            return Json(new { schedule = ScheduleDAO.GetById(id) });
        }
    }
}