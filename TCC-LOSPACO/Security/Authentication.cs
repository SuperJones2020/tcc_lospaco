using System;
using System.Web;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.Security {
    public class Authentication {
        private static Database db = new Database();
        public static void SignIn(uint id, string email, string password) {
            if (!IsSigned()) {
                HttpCookie cookie = new HttpCookie("sjwt", SJWT.GenerateToken(id, email, password)) {
                    Expires = DateTime.MaxValue
                };
                cookie.HttpOnly = true;
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        public static void SignOut() {
            if (IsSigned()) {
                HttpCookie cookie = new HttpCookie("sjwt", null) {
                    Expires = DateTime.Now.AddDays(-1)
                };
                HttpContext.Current.Response.AppendCookie(cookie);
            }
            /*if (HttpContext.Current.Request.Cookies["remember_me"] != null) {
                HttpCookie cookie = new HttpCookie("remember_me", null);
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }*/
        }

        /*public static void RememberMe() {
            if (HttpContext.Current.Request.Cookies["remember_me"] == null) {
                HttpCookie cookie = new HttpCookie("remember_me", "true") {
                    Expires = DateTime.MaxValue
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }*/


        public static Customer GetUser() {
            HttpCookie token = GetToken();
            if (token == null) return null;
            dynamic data = SJWT.GetTokenData(token.Value);
            dynamic payload = data.Payload;
            object id = payload.id;
            return CustomerDAO.GetById(Convert.ToUInt32(id));
        }

        public static bool IsSigned() => GetToken() != null;
        public static HttpCookie GetToken() => HttpContext.Current.Request.Cookies["sjwt"];

        public static bool VerifyToken() {
            var headerToken = HttpContext.Current.Request.Headers["Authorization"];
            if (headerToken == null || headerToken == "null") return false;
            string token = headerToken.Split(' ')[1];
            return token == GetToken().Value;
        }

        public static bool VerifyTokenMobile() {
            var headerToken = HttpContext.Current.Request.Headers["Authorization"];
            if (headerToken == null || headerToken == "null") return false;
            string token = headerToken.Split(' ')[1];
            dynamic data = SJWT.GetTokenData(token);
            Customer c = CustomerDAO.GetById(Convert.ToUInt32(data.Payload.id));
            string newToken = SJWT.GenerateToken(c.Account.Id, c.Account.Email, c.Account.Password);
            return token == newToken;
        }

        public static bool IsValid() {
            return IsSigned() && VerifyToken();
        }
    }
}