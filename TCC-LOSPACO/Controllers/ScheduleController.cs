using System.Web.Mvc;
using TCC_LOSPACO.DAO;

namespace TCC_LOSPACO.Controllers {
    public class ScheduleController : Controller {
        [HttpPost]
        public ActionResult Get(string date) {
            return Json(new { schedule = ScheduleDAO.GetByDate(date) });
        }
    }
}