using System.Web.Mvc;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Security;

namespace TCC_LOSPACO.Controllers {
    public class CustomerController : Controller {
        private static Database db = new Database();
        [HttpPost]
        public ActionResult Get(uint id) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            JsonResult json = Json(new { Object = CustomerDAO.GetById(id) });
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        [HttpPost]
        public ActionResult Update(ushort id, string column, string value) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            string query = $"update tbcustomer set {column}='{value}' where loginid='{id}'";
            db.ExecuteCommand(query);
            return Json(new { Success = "Success" });
        }
    }
}