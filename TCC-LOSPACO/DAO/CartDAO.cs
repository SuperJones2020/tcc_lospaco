using System.Collections.Generic;

namespace TCC_LOSPACO.DAO {
    public abstract class CartDAO {
        private static Database db = new Database();

        public static List<dynamic> GetList(ushort id) {
            List<dynamic> list = new List<dynamic>();
            db.ReaderRows(db.ReturnCommand($"select itemname, itemtype, ItemQnt, ItemImage, ItemPrice from vw_cart where LoginId='{id}'"), row => list.Add(new { Name = row[0], Type = row[1], Qnt = row[2], Price = row[3], Image = row[4], }));
            return list;
        }

        public static object GetTotalPrice() => db.ReaderValue(db.ReturnProcedure("sp_SelectTotalValueCart", Security.Authentication.GetUser().Id));
        public static void InsertItem(string name, byte quantity) => db.ExecuteProcedure("sp_insertItemCart", name, Security.Authentication.GetUser().Id, quantity);
        public static object[] RemoveItem(string name) => db.ReaderAllValue(db.ReturnProcedure("sp_RemoveItemCart", Security.Authentication.GetUser().Id, name));
        public static object[] UpdateQuantity(string name, byte qty) => db.ReaderAllValue(db.ReturnProcedure("sp_UpdateQntItemCart", Security.Authentication.GetUser().Id, name, qty));
        public static bool IsCartEmpty() => db.ReaderValue(db.ReturnProcedure("sp_selectCart", Security.Authentication.GetUser().Id)) == null;
        public static byte GetQuantity(string name) {
            object v = db.ReaderValue(db.ReturnCommand($"select itemqnt from vw_cart where loginid='{Security.Authentication.GetUser().Id}' and itemname = '{name}'"));
            return (byte)v;
        }
    }
}