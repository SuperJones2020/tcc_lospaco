using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public class CartServiceDAO {
        public static List<CartService> GetList() {
            List<CartService> list = new List<CartService>();
            Database.ReaderRows(Database.ReturnProcedure("sp_SelectCart", Security.Context.GetUser()), row => {
                list.Add(new CartService((string)row[0], (byte)row[1], (decimal)row[3], (byte[])row[4]));
            });
            return list;
        }

        public static CartService GetByName(string name) {
            string complement = $"and LoginEmail = '{Security.Context.GetUser()}'";
            object[] row = Database.ReaderRow(Database.ReturnCommand($"select ItemName, ItemQnt, itemPrice, itemImage from vw_cart where ItemName='{name}' {complement}"));
            if (row.Length == 0) return null;
            return new CartService((string)row[0], (byte)row[1], (decimal)row[2], (byte[])row[3]);
        }
    }
}