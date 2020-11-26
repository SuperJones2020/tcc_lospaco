using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace TCC_LOSPACO.CustomHtmlHelper {
    public static class CustomHtmlHelper {
        public static IHtmlString DropdownItem(string item, string value, string name) {
            string htmlItem = $"<label for='{value}' data-option='{value}' data-anim-to-black class='bg-white p-3'>{item}</label><input id='{value}' type='radio' class='d-none' name='{name}' value='{value}' />";
            return new MvcHtmlString(htmlItem);
        }

        public static string RenderPartialToString(string viewName, object model, ControllerContext ControllerContext) {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            ViewDataDictionary ViewData = new ViewDataDictionary();
            TempDataDictionary TempData = new TempDataDictionary();
            ViewData.Model = model;
            using (StringWriter sw = new StringWriter()) {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }

        }

        public static IHtmlString SendRequest(string txt, string url, dynamic data, dynamic info, string htmlClass) {
            string attrOnSuccess = Global.GetValue(info, "OnSuccess") != null ? $"data-on-success = '{Global.GetValue(info, "OnSuccess")}'" : "";
            string attrOnFailure = Global.GetValue(info, "OnFailure") != null ? $"data-on-failure = '{Global.GetValue(info, "OnFailure")}'" : "";
            string attrLoader = Global.GetValue(info, "Loader") != null ? $"data-loader = '{Global.GetValue(info, "Loader")}'" : "";
            string htmlItem = $"<a action='{url}' data-params='{JObject.FromObject(data)}' {attrOnSuccess} {attrOnFailure} {attrLoader} class='{htmlClass}' data-element-request-sender='false' >{txt}</a>";
            return new MvcHtmlString(htmlItem);
        }

        public static string ImageSource(byte[] image) {
            string value = System.Text.Encoding.UTF8.GetString(image);
            return $"data:image/png;base64,{value}";
        }

    }
}