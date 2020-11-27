﻿using System;
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
            var list = ServiceDAO.GetList(index, categoria, preco_inicial, preco_final);
            return View(list);
        }

        public ActionResult Profile() {
            if (!Security.Context.IsSigned()) return Redirect("/Account/Account");
            return View();
        }
    }
}