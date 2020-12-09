using System;
using System.IO;
using System.Web.Mvc;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Models;
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
            if (column == "packname") {
                db.ExecuteProcedure("sp_updatepackagename", id, value);
                return Json(new { Success = "Success" });
            }
            string query = $"update tbpackage set {column}='{value}' where packid='{id}'";
            db.ExecuteCommand(query);
            return Json(new { Success = "Success" });
        }

        [HttpPost]
        public ActionResult InsertService(ushort id, ushort servId) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            PackageDAO.InsertService(id, servId);
            return Json(new { Success = "Success" });
        }

        [HttpPost]
        public ActionResult RemoveService(ushort id, ushort servId) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            PackageDAO.RemoveService(id, servId);
            return Json(new { Success = "Success" });
        }

        [HttpPost]
        public ActionResult Insert(string name, string minified_desc, string desc, System.Web.HttpPostedFileWrapper image, string price, string services) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            string base64Image = null;
            if (image != null) {
                BinaryReader br = new BinaryReader(image.InputStream);
                byte[] bytes = br.ReadBytes((Int32)image.InputStream.Length);
                base64Image = Convert.ToBase64String(bytes);
            }
            PackageDAO.Insert(name, minified_desc, desc, base64Image, price, services);
            Package p = PackageDAO.GetByName(name);
            return Json(new { p.Id, Package = CustomHtmlHelper.CustomHtmlHelper.RenderPartialToString("Profile/TableItem/_Package", p, ControllerContext) });
        }
    }
}