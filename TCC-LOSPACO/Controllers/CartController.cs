using System;
using System.Web.Mvc;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Security;

namespace TCC_LOSPACO.Controllers {
    public class CartController : Controller {
        [HttpPost]
        public ActionResult AddItemToCart(string id, byte quantity, string type) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            string name = type == "SERVICO" ? ServiceDAO.GetById(Convert.ToUInt16(id)).Name : PackageDAO.GetById(Convert.ToUInt16(id)).Name;
            var data = CartDAO.GetList();
            bool containsInCart = false;
            string view = null;
            foreach (dynamic d in data) {
                if (d.Name == name) {
                    containsInCart = true;
                    break;
                }
            }
            CartDAO.InsertItem(name, quantity);
            if (!containsInCart) {
                if (type == "SERVICO") view = CustomHtmlHelper.CustomHtmlHelper.RenderPartialToString("Cart/_Service", new { Object = ServiceDAO.GetCartServiceByName(name) }, ControllerContext);
                else view = CustomHtmlHelper.CustomHtmlHelper.RenderPartialToString("Cart/_Package", new { Object = PackageDAO.GetCartPackageByName(name) }, ControllerContext);
            }

            JsonResult json = Json(new {
                contains = containsInCart,
                itemQuantity = CartDAO.GetQuantity(name),
                price = "R$ " + CartDAO.GetTotalPrice(),
                itemName = name,
                htmlItem = view,
                success = "Adicionado!"
            });
            json.MaxJsonLength = int.MaxValue;

            return json;
        }

        [HttpPost]
        public ActionResult RemoveCartItem(string name) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            object[] obj = CartDAO.RemoveItem(name);
            return Json(new { type = obj[0], message = obj[1], name, price = "R$ " + CartDAO.GetTotalPrice() });
        }
        [HttpPost]
        public ActionResult UpdateCartItemQuantity(string name, byte qty) {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            object[] obj = CartDAO.UpdateQuantity(name, qty);
            return Json(new { type = obj[0], message = obj[1], price = "R$ " + CartDAO.GetTotalPrice() });
        }

        [HttpPost]
        public ActionResult IsCartEmpty() {
            if (!Authentication.IsValid()) return Json(new { Error = "Not Authenticated" });
            return Json(new { isEmpty = CartDAO.IsCartEmpty() });
        }

    }
}