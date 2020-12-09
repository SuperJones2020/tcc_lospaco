using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public abstract class SaleDAO {
        private static Database db = new Database();
        public static Sale GetById(uint id) {
            var row = db.ReaderRow(db.ReturnCommand($"select * from tbsale where loginid = '{id}'"));
            var sales = new List<ItemSale>();
            db.ReaderRows(db.ReturnCommand($"select * from tbitemsale where SaleId = '{(uint)row[0]}'"), data => sales.Add(new ItemSale((uint)data[0], (string)data[1], (decimal)data[3])));
            return new Sale((uint)row[0], row[1] + "", CustomerDAO.GetById((uint)row[2]), sales);
        }

        public static void BuyItemsByCart(uint id) {
            db.ExecuteProcedure("sp_InsertSaleByCart", id);
        }
    }
}