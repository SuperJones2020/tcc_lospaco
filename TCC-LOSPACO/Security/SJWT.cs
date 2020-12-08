using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;

namespace TCC_LOSPACO {
    public static class SJWT {
        public static string GenerateToken(uint id, string email, string password) {
            string header = "{\"alg\":\"SHA256\",\"typ\":\"SJWT\"}";
            string payload = "{\"id\":\"" + id + "\",\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";
            string Base64Header = ToBase64(GetBytes(header));
            string Base64Payload = ToBase64(GetBytes(payload));
            string Signature = ToBase64(GetBytes(HMAC_SHA256("5cnp0sod2rlt8bpi0y5g1925taileae67sp2uh6qhzncxgnm55ztfh", Base64Header + "." + Base64Payload)));
            return $"{Base64Header}.{Base64Payload}.{Signature}";
        }

        public static dynamic GetTokenData(string token) {
            string[] parts = token.Split('.');
            string header = FromBase64(parts[0]);
            string payload = FromBase64(parts[1]);
            string signature = parts[2];
            return new {
                Header = header,
                Payload = Json.Decode(payload),
                Signature = signature
            };
        }

        public static byte[] GetBytes(string str) => Encoding.ASCII.GetBytes(str);

        private static string GetHEX(byte[] bytes) {
            string finalHash = "";
            foreach (byte b in bytes) finalHash += b.ToString("X2").ToLower();
            return finalHash;
        }

        private static string ToBase64(byte[] bytes) {
            string base64 = Convert.ToBase64String(bytes);
            return RemoveAll(base64, '=');
        }

        private static string HMAC_SHA256(string key, string value) {
            HMACSHA256 hmacsha256 = new HMACSHA256(GetBytes(key));
            byte[] messageBytes = GetBytes(value);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return GetHEX(hashmessage).ToLower();
        }

        private static string RemoveAll(string str, params char[] chars) {
            List<char> array = str.ToCharArray().ToList();
            List<char> elements = chars.ToList();
            return new string(array.Where(x => !elements.Contains(x)).ToArray());
        }

        public static string FromBase64(string b) {
            while (b.Length % 4 != 0) b += "=";
            return System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(b));
        }
    }
}
