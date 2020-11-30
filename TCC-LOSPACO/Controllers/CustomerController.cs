using System.Web.Mvc;
using TCC_LOSPACO.DAO;

namespace TCC_LOSPACO.Controllers {
    public class CustomerController : Controller {
        private static Database db = new Database();
        [HttpPost]
        public ActionResult Get(uint id) {
            return Json(new { customer = CustomerDAO.GetById(id) });
        }

        [HttpPost]
        public ActionResult GetTableColumns() {
            return Json(new { columns = db.ReaderColumns("tbcustomer") });
        }
    }
}