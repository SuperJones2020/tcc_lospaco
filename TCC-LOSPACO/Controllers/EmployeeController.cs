using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Models;
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
        public ActionResult InsertService(ushort id, ushort servId) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            EmployeeDAO.InsertService(id, servId);
            return Json(new { Success = "Success" });
        }

        [HttpPost]
        public ActionResult RemoveService(ushort id, ushort servId) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            EmployeeDAO.RemoveService(id, servId);
            return Json(new { Success = "Success" });
        }

        [HttpPost]
        public ActionResult Insert(string full_name, string username, string email, string password, string number, string birth, string rg, string cpf, string salary, string genre, System.Web.HttpPostedFileWrapper image, string services) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            string base64Image = null;
            if (image != null) {
                BinaryReader br = new BinaryReader(image.InputStream);
                byte[] bytes = br.ReadBytes((Int32)image.InputStream.Length);
                base64Image = Convert.ToBase64String(bytes);
            }
            EmployeeDAO.Insert(full_name, username, email, password, number, birth, rg, cpf, salary, genre, base64Image, services);
            Employee e = EmployeeDAO.GetByRG(rg);
            return Json(new { e.Customer.Account.Id, Employee = CustomHtmlHelper.CustomHtmlHelper.RenderPartialToString("Profile/TableItem/_Employee", e, ControllerContext) });
        }

        [HttpPost]
        public ActionResult GetEmpsAvaible(string datetime, string servname) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            Service s = ServiceDAO.GetByName(servname);
            List<Employee> emps = EmployeeDAO.GetEmployeesAvaible(datetime, s.Id);
            return Json(new { CarouselItems = CustomHtmlHelper.CustomHtmlHelper.RenderPartialToString("Profile/_EmployeeCarousel", emps, ControllerContext) });
        }
    }
}