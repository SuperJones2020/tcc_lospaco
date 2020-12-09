using System.Web.Mvc;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Security;

namespace TCC_LOSPACO.Controllers {
    public class SaleController : Controller {
        private static Database db = new Database();
        [HttpPost]
        public ActionResult Get(uint id) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            JsonResult json = Json(new { Object = SaleDAO.GetItemSaleById(id) });
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        [HttpPost]
        public ActionResult GetSales() {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            JsonResult json = Json(new { Object = SaleDAO.GetList() });
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        [HttpPost]
        public ActionResult BuyItems() {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            SaleDAO.BuyItemsByCart();
            return Json(new { Success = "Success" });
        }

        /*public ActionResult GetList() {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            JsonResult json = Json(new { Services = ServiceDAO.GetList() }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }*/

        /*[HttpPost]
         public ActionResult Update(ushort id, string column, string value) {
             if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
             if (column == "servname") {
                 db.ExecuteProcedure("sp_updateservicename", id, value);
                 return Json(new { Success = "Success" });
             }
             string query = $"update tbservices set {column}='{value}' where servid='{id}'";
             db.ExecuteCommand(query);
             return Json(new { Success = "Success" });
         }*/

    }
}