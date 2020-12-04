using System;
using System.Linq;
using System.Web.Mvc;
using TCC_LOSPACO.DAO;

namespace TCC_LOSPACO.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public ActionResult Service(ushort? id) {
            if (id == null) return RedirectToAction("Services");
            return View(ServiceDAO.GetById(Convert.ToUInt16(id)));
        }

        public ActionResult Services(string categoria, string ordenar_por, int? preco_inicial, int? preco_final) {
            ordenar_por = ordenar_por ?? "Relevancia";
            string[] orderByString = { "Relevancia", "Menor Para Maior", "Maior Para Menor" };
            int index = orderByString.ToList().IndexOf(orderByString.ToList().Where(i => i == ordenar_por).First());
            return View(ServiceDAO.GetList(index, categoria, preco_inicial, preco_final));
        }

        public ActionResult Package(ushort? id) {
            if (id == null) return RedirectToAction("Packages");
            return View(PackageDAO.GetById(Convert.ToUInt16(id)));
        }

        public ActionResult Packages(string ordenar_por, int? preco_inicial, int? preco_final) {
            ordenar_por = ordenar_por ?? "Relevancia";
            string[] orderByString = { "Relevancia", "Menor Para Maior", "Maior Para Menor" };
            int index = orderByString.ToList().IndexOf(orderByString.ToList().Where(i => i == ordenar_por).First());
            return View(PackageDAO.GetList(index, preco_inicial, preco_final));
        }

        public ActionResult Error() {
            return View();
        }

        public ActionResult Profile() {
            if (!Security.Authentication.IsSigned()) return Redirect("/Account/Account");
            return View();
        }
    }
}