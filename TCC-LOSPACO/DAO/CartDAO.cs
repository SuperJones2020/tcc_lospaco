using System;
using System.Collections.Generic;

namespace TCC_LOSPACO.DAO {
    public abstract class CartDAO {
        private static Database db = new Database();

        public static List<dynamic> GetList() {
            List<dynamic> list = new List<dynamic>();
            db.ReaderRows(db.ReturnCommand($"select itemname, itemtype, ItemQnt, ItemImage, ItemPrice from vw_cart where LoginId='{Security.Authentication.GetUser().Account.Id}'"), row => list.Add(new { Name = row[0], Type = row[1], Qnt = row[2], Price = row[3], Image = row[4] }));
            return list;
        }

        public static object GetTotalPrice() => db.ReaderValue(db.ReturnProcedure("sp_SelectTotalValueCart", Security.Authentication.GetUser().Account.Id));
        public static void InsertItem(string name, byte quantity) => db.ExecuteProcedure("sp_insertItemCart", name, Security.Authentication.GetUser().Account.Id, quantity);
        public static object[] RemoveItem(string name) => db.ReaderAllValue(db.ReturnProcedure("sp_RemoveItemCart", Security.Authentication.GetUser().Account.Id, name));
        public static object[] UpdateQuantity(string name, byte qty) => db.ReaderAllValue(db.ReturnProcedure("sp_UpdateQntItemCart", Security.Authentication.GetUser().Account.Id, name, qty));
        public static object GetItemsCount() => db.ReaderValue(db.ReturnProcedure("sp_CountItemCart", Security.Authentication.GetUser().Account.Id));
        public static bool IsCartEmpty() => Convert.ToByte(GetItemsCount()) == 0;
        public static byte GetQuantity(string name) => (byte)db.ReaderValue(db.ReturnCommand($"select itemqnt from vw_cart where loginid='{Security.Authentication.GetUser().Account.Id}' and itemname = '{name}'"));
    }
}