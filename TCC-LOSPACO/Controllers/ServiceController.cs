using System;
using System.Web.Mvc;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Security;

namespace TCC_LOSPACO.Controllers {
    public class ServiceController : Controller {
        private static Database db = new Database();
        [HttpPost]
        public ActionResult Get(uint id) {
            return Json(new { service = ServiceDAO.GetById(Convert.ToUInt16(id)) });
        }

        /*[HttpPost]
        public void Update(ushort id, string name, decimal price, string minifiedDesc, string completeDesc, string category, TimeSpan time, string image, string propperClothing) {
            var data = image.Split(',')[1];
            string p = price.ToString().Replace(',', '.');
            Database.ExecuteProcedure("sp_UpdateService", id, name, p, minifiedDesc, completeDesc, category, time, data, propperClothing);
        }*/

        [HttpPost]
        public ActionResult GetTableColumns() {
            return Json(new { columns = db.ReaderColumns("tbservices") });
        }

        [HttpPost]
        public ActionResult Update(ushort id, string column, string value) {
            if (!Authentication.VerifyToken()) return Json(new { Error = "Not Authenticated" });
            string query = $"update tbservices set {column}='{value}' where servid='{id}'";
            db.ExecuteCommand(query);
            return Json(new { Success = "Success" });
        }
    }
}