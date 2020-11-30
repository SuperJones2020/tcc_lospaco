using System;
using System.Web;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.Security {
    public class Authentication {
        private static Database db = new Database();
        public static void SignIn() {
            if (!IsSigned()) {
                SJWT.GenerateToken();
                SJWT.GenerateSessionId();
                db.ExecuteCommand($"insert into tbsjwt(loginid, token, sessionid)values('{SJWT.User.Id}','{SJWT.Token}', '{SJWT.CurrentSessionId}')");
                HttpCookie cookie = new HttpCookie("sessionid", SJWT.CurrentSessionId + "") {
                    Expires = DateTime.MaxValue
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void SignOut() {
            if (IsSigned()) {
                db.ExecuteCommand($"delete from tbsjwt where sessionid = '{HttpContext.Current.Request.Cookies["sessionid"].Value}'");
                SJWT.ResetConfig();
                HttpCookie cookie = new HttpCookie("sessionid", null);
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
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

        public static bool IsSigned() {
            //return db.ReaderRow(db.ReturnCommand($"select * from tbsjwt where idlogin = '{id}'")).Length > 0;
            return HttpContext.Current.Request.Cookies["sessionid"] != null;
        }

        public static Account GetUser() {
            HttpCookie sessionid = HttpContext.Current.Request.Cookies["sessionid"];
            object loginId = db.ReaderValue(db.ReturnCommand($"select loginid from tbsjwt where sessionid = '{sessionid.Value}'"));
            if (loginId == null) return null;
            return AccountDAO.GetById(Convert.ToUInt32(loginId));
        }

        public static string GetToken() {
            return db.ReaderValue(db.ReturnCommand($"select token from tbsjwt where sessionid = '{HttpContext.Current.Request.Cookies["sessionid"].Value}'")) + "";
        }

        public static bool VerifyToken() {
            var headerToken = HttpContext.Current.Request.Headers["Authorization"];
            if (headerToken == null || headerToken == "null") return false;
            return headerToken.Split(' ')[1] == GetToken();
        }

        public static bool CanRequest() {
            return IsSigned() && VerifyToken();
        }
    }
}