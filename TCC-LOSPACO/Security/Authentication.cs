using System;
using System.Web;

namespace TCC_LOSPACO.Security {
    public static class Authentication {
        public static void SignIn(object value) {
            if (!IsSigned()) {
                HttpCookie cookie = new HttpCookie("user", value + "") {
                    Expires = DateTime.MaxValue
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void SignOut() {
            if (IsSigned()) {
                HttpCookie cookie = new HttpCookie("user", null);
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            if (HttpContext.Current.Request.Cookies["remember_me"] != null) {
                HttpCookie cookie = new HttpCookie("remember_me", null);
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void RememberMe() {
            if (HttpContext.Current.Request.Cookies["remember_me"] == null) {
                HttpCookie cookie = new HttpCookie("remember_me", "true") {
                    Expires = DateTime.MaxValue
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static bool IsSigned() {
            return HttpContext.Current.Request.Cookies["user"] != null;
        }

        public static string GetUser() {
            HttpCookie user = HttpContext.Current.Request.Cookies["user"];
            string value = user == null ? "" : user.Value;
            return value;
        }
    }
}