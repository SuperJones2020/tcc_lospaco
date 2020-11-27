using System.Web.Mvc;
using TCC_LOSPACO.DAO;

namespace TCC_LOSPACO.Controllers {
    public class CartController : Controller {
        [HttpPost]
        public ActionResult AddItemToCart(string name, byte quantity) {
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
            if (!containsInCart) view = CustomHtmlHelper.CustomHtmlHelper.RenderPartialToString("Cart/_Service", CartServiceDAO.GetByName(name), ControllerContext);

            return Json(new {
                contains = containsInCart,
                itemQuantity = CartDAO.GetQuantity(name),
                price = "R$ " + CartDAO.GetTotalPrice(),
                itemName = name,
                htmlItem = view,
                success = "Adicionado!"
            });
        }

        [HttpPost]
        public ActionResult RemoveCartItem(string name) {
            object[] obj = CartDAO.RemoveItem(name);
            return Json(new { type = obj[0], message = obj[1], name, price = "R$ " + CartDAO.GetTotalPrice() });
        }
        [HttpPost]
        public ActionResult UpdateCartItemQuantity(string name, byte qty) {
            object[] obj = CartDAO.UpdateQuantity(name, qty);
            return Json(new { type = obj[0], message = obj[1], price = "R$ " + CartDAO.GetTotalPrice() });
        }

        [HttpPost]
        public ActionResult IsCartEmpty() => Json(new { isEmpty = CartDAO.IsCartEmpty() });

    }
}