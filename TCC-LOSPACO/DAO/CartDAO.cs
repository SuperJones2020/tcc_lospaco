using System.Collections.Generic;

namespace TCC_LOSPACO.DAO {
    public abstract class CartDAO {
        public static List<dynamic> GetList() {
            List<dynamic> list = new List<dynamic>();
            Database.ReaderRows(Database.ReturnCommand($"select itemname, itemtype from vw_cart where loginemail='{Security.Context.GetUser()}'"), row => list.Add(new { Name = row[0], Type = row[1] }));
            return list;
        }

        public static object GetTotalPrice() => Database.ReaderValue(Database.ReturnProcedure("sp_SelectTotalValueCart", Security.Context.GetUser()));
        public static object[] GetByName(string name) => Database.ReaderRow(Database.ReturnCommand($"select ItemName, ItemQnt, ItemType from vw_Cart where LoginEmail='{Security.Context.GetUser()}' and ItemName = '{name}'"));
        public static void InsertItem(string name, byte quantity) => Database.ExecuteProcedure("sp_insertItemCart", name, Security.Context.GetUser(), quantity);
        public static object[] RemoveItem(string name) => Database.ReaderAllValue(Database.ReturnProcedure("sp_RemoveItemCart", Security.Context.GetUser(), name));
        public static object[] UpdateQuantity(string name, byte qty) => Database.ReaderAllValue(Database.ReturnProcedure("sp_UpdateQntItemCart", Security.Context.GetUser(), name, qty));
        public static bool IsCartEmpty() => Database.ReaderValue(Database.ReturnProcedure("sp_selectCart", Security.Context.GetUser())) == null;
        public static byte GetQuantity(string name) => (byte)Database.ReaderValue(Database.ReturnCommand($"select itemqnt from vw_cart where loginemail='{Security.Context.GetUser()}' and itemname = '{name}'"));
    }
}