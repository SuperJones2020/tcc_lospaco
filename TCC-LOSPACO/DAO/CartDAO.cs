using System.Collections.Generic;

namespace TCC_LOSPACO.DAO {
    public abstract class CartDAO {
        public static List<dynamic> GetList() {
            List<dynamic> list = new List<dynamic>();
            Database.ReaderRows(Database.ReturnCommand($"select itemname, itemtype from vw_cart where loginemail='{Security.Authentication.GetUser()}'"), row => list.Add(new { Name = row[0], Type = row[1] }));
            return list;
        }

        public static object GetTotalPrice() => Database.ReaderValue(Database.ReturnProcedure("sp_SelectTotalValueCart", Security.Authentication.GetUser()));
        public static object[] GetByName(string name) => Database.ReaderRow(Database.ReturnCommand($"select ItemName, ItemQnt, ItemType from vw_Cart where LoginEmail='{Security.Authentication.GetUser()}' and ItemName = '{name}'"));
        public static void InsertItem(string name, byte quantity) => Database.ExecuteProcedure("sp_insertItemCart", name, Security.Authentication.GetUser(), quantity);
        public static object[] RemoveItem(string name) => Database.ReaderAllValue(Database.ReturnProcedure("sp_RemoveItemCart", Security.Authentication.GetUser(), name));
        public static object[] UpdateQuantity(string name, byte qty) => Database.ReaderAllValue(Database.ReturnProcedure("sp_UpdateQntItemCart", Security.Authentication.GetUser(), name, qty));
        public static bool IsCartEmpty() => Database.ReaderValue(Database.ReturnProcedure("sp_selectCart", Security.Authentication.GetUser())) == null;
        public static byte GetQuantity(string name) => (byte)Database.ReaderValue(Database.ReturnCommand($"select itemqnt from vw_cart where loginemail='{Security.Authentication.GetUser()}' and itemname = '{name}'"));
    }
}