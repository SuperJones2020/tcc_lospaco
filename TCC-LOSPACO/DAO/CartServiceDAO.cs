using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public class CartServiceDAO {
        public static List<CartService> GetList() {
            List<CartService> list = new List<CartService>();
            Database.ReaderRows(Database.ReturnProcedure("sp_SelectCart", Security.Authentication.GetUser()), row => list.Add(new CartService((ushort)row[0], (string)row[1], (byte)row[2], (decimal)row[4], (byte[])row[5])));
            return list;
        }

        public static CartService GetByName(string name) {
            string query = $"select ItemId, ItemName, ItemQnt, itemPrice, itemImage from vw_cart where ItemName='{name}' and LoginEmail = '{Security.Authentication.GetUser()}'";
            object[] row = Database.ReaderRow(Database.ReturnCommand(query));
            if (row.Length == 0) return null;
            return new CartService((ushort)row[0], (string)row[1], (byte)row[2], (decimal)row[3], (byte[])row[4]);
        }
    }
}