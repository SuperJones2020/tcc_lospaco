using System;
using System.IO;
using System.Web.Mvc;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Models;
using TCC_LOSPACO.Security;

namespace TCC_LOSPACO.Controllers {
    public class CategoryController : Controller {
        Database db = new Database();
        [HttpPost]
        public ActionResult Get(byte id) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            JsonResult json = Json(new { Object = CategoryDAO.GetById(id) });
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        [HttpPost]
        public ActionResult Update(ushort id, string column, string value) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            string query = $"update tbcategories set {column}='{value}' where categoryid='{id}'";
            db.ExecuteCommand(query);
            return Json(new { Success = "Success" });
        }


        [HttpPost]
        public ActionResult Insert(string name, System.Web.HttpPostedFileWrapper image) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            string base64Image = null;
            if (image != null) {
                BinaryReader br = new BinaryReader(image.InputStream);
                byte[] bytes = br.ReadBytes((Int32)image.InputStream.Length);
                base64Image = Convert.ToBase64String(bytes);
            }
            db.ExecuteCommand($"insert into tbcategories(catname, image)values('{name}', '{base64Image}')");
            Category category = CategoryDAO.GetByName(name);
            return Json(new { category.Id, Category = CustomHtmlHelper.CustomHtmlHelper.RenderPartialToString("Profile/TableItem/_Category", category, ControllerContext) });
        }
    }
}