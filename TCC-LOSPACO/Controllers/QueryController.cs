using System.Web.Mvc;
using TCC_LOSPACO.DAO;

namespace TCC_LOSPACO.Controllers {
    public class QueryController : Controller {
        [HttpPost]
        public ActionResult AddReview(string name, string review, byte rating) {
            //ReviewDAO.AddReview(name, review, rating);
            return Json("");
        }
        [HttpPost]
        public ActionResult UpdatePassword(string currentPassword, string newPassword) {
            dynamic obj = AccountDAO.UpdatePassword(currentPassword, newPassword);
            return Json(new { obj.message, obj.index });
        }
    }
}