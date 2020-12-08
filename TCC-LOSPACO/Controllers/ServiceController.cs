using System;
using System.Web.Mvc;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Security;

namespace TCC_LOSPACO.Controllers {
    public class ServiceController : Controller {
        private static Database db = new Database();
        [HttpPost]
        public ActionResult Get(uint id) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            JsonResult json = Json(new { Object = ServiceDAO.GetById(Convert.ToUInt16(id)) });
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult GetList() {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            JsonResult json = Json(new { Services = ServiceDAO.GetList() }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        [HttpPost]
        public ActionResult Update(ushort id, string column, string value) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            if (column == "servname") db.ExecuteCommand($"call sp_updateservicename('{id}','{value}')");
            string query = $"update tbservices set {column}='{value}' where servid='{id}'";
            db.ExecuteCommand(query);
            return Json(new { Success = "Success" });
        }

        [HttpPost]
        public ActionResult Insert(string name, string price, string minified_desc, string desc, uint category_id, string time, string image, string clothing) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            db.ExecuteProcedure("sp_InsertService", name, price, minified_desc, desc, category_id, time, image, clothing);
            return Json(new { Success = "Success" });
        }
    }
}