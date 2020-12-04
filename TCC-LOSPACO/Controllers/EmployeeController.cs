using System.Web.Mvc;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Security;

namespace TCC_LOSPACO.Controllers {
    public class EmployeeController : Controller {
        Database db = new Database();
        [HttpPost]
        public ActionResult Get(uint id) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            JsonResult json = Json(new { Object = EmployeeDAO.GetById(id) });
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        [HttpPost]
        public ActionResult Update(string table, ushort id, string column, string value) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            string query = $"update {table} set {column}='{value}' where loginid='{id}'";
            db.ExecuteCommand(query);
            return Json(new { Success = "Success" });
        }

        [HttpPost]
        public ActionResult Insert(string fullname, string birth, string rg, string cpf, string salary, string genre, string image) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            //db.ExecuteProcedure(db.ReturnProcedure("sp_InsertEmployee", ));
            return Json(new { Success = "Success" });
        }
    }
}