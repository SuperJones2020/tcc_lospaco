using System.Web.Mvc;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Security;

namespace TCC_LOSPACO.Controllers {
    public class ScheduleController : Controller {
        Database db = new Database();
        [HttpPost]
        public ActionResult Get(uint id) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            JsonResult json = Json(new { Object = ScheduleDAO.GetById(id) });
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        [HttpPost]
        public ActionResult Update(ushort id, string column, string value) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            string query = $"update vw_schedulescustomer set {column}='{value}' where schedid='{id}'";
            db.ExecuteCommand(query);
            return Json(new { Success = "Success" });
        }

        [HttpPost]
        public ActionResult Insert(string customer_cpf, uint service, uint employee, string date, string time) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            db.ExecuteProcedure("sp_InsertShed", CustomerDAO.GetByCPF(customer_cpf), employee, service, $"{date} {time}");
            return Json(new { Success = "Success" });
        }
    }
}