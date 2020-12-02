using System.Web.Mvc;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Security;

namespace TCC_LOSPACO.Controllers {
    public class PackageController : Controller {
        Database db = new Database();
        [HttpPost]
        public ActionResult Get(ushort id) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            JsonResult json = Json(new { Object = PackageDAO.GetById(id) });
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        [HttpPost]
        public ActionResult GetServices(ushort id) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            JsonResult json = Json(new { services = PackageDAO.GetServicesFromPackage(id) });
            json.MaxJsonLength = int.MaxValue;
            return json; ;
        }

        [HttpPost]
        public ActionResult Update(ushort id, string column, string value) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            string query = $"update tbpackage set {column}='{value}' where packid='{id}'";
            db.ExecuteCommand(query);
            return Json(new { Success = "Success" });
        }

        [HttpPost]
        public ActionResult InsertService(ushort id, ushort servId) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            string query = $"insert into tbpackitem(servid, packid) values('{servId}', '{id}')";
            db.ExecuteCommand(query);
            return Json(new { Success = "Success" });
        }

        [HttpPost]
        public ActionResult RemoveService(ushort id, ushort servId) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            string query = $"delete from tbpackitem where packid='{id}' and servid='{servId}'";
            db.ExecuteCommand(query);
            return Json(new { Success = "Success" });
        }
    }
}