using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TCC_LOSPACO.DAO;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO {
    public static class SJWT {
        public static Account User { get; private set; }
        public static string CurrentSessionId { get; private set; }
        public static string Token { get; private set; }
        public static string Secret { get; private set; }
        public static string Header { get; private set; }
        public static string Payload { get; private set; }

        public static void SJWTConfig(string secret, dynamic data) {
            Header = "{\"alg\":\"" + data.Alg + "\",\"typ\":\"" + data.Typ + "\"}";
            Payload = "{\"id\":" + data.Id + ",\"email\":\"" + data.Email + "\"}\"";
            Secret = secret;
            User = AccountDAO.GetById(Convert.ToUInt16(data.Id + ""));
        }

        public static void ResetConfig() {
            CurrentSessionId = null;
            User = null;
            Token = null;
            Secret = null;
            Header = null;
            Payload = null;
        }

        public static void GenerateToken() {
            string Base64Header = Base64(GetBytes(Header));
            string Base64Payload = Base64(GetBytes(Payload));
            string Signature = Base64(GetBytes(HMAC_SHA256(Secret, Base64Header + "." + Base64Payload)));
            Token = $"{Base64Header}.{Base64Payload}.{Signature}";
        }

        private static byte[] GetBytes(string str) => Encoding.ASCII.GetBytes(str);

        private static string GetHEX(byte[] bytes) {
            string finalHash = "";
            foreach (byte b in bytes) finalHash += b.ToString("X2").ToLower();
            return finalHash;
        }

        private static string Base64(byte[] bytes) {
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

        public static void GenerateSessionId() {
            CurrentSessionId = Guid.NewGuid().ToString();
        }
    }
}
