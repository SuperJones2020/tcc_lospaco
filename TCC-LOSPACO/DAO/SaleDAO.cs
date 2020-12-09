using System.Collections.Generic;
using TCC_LOSPACO.Models;
using TCC_LOSPACO.Security;

namespace TCC_LOSPACO.DAO {
    public abstract class SaleDAO {
        private static Database db = new Database();
        public static List<Sale> GetList() {
            var sales = new List<Sale>();
            db.ReaderRows(db.ReturnCommand($"select * from tbsale where loginid = '{Authentication.GetUser().Account.Id}'"), row => {
                var items = new List<ItemSale>();
                db.ReaderRows(db.ReturnCommand($"select * from tbitemsale where SaleId = '{(uint)row[0]}'"), data => items.Add(new ItemSale((uint)data[0], (string)data[1], (decimal)data[3])));
                sales.Add(new Sale((uint)row[0], row[1] + "", CustomerDAO.GetById((uint)row[2]), items));
            });
            return sales;
        }

        public static ItemSale GetItemSaleById(uint id) {
            object[] row = db.ReaderRow(db.ReturnCommand($"select * from tbitemsale where itemsaleid = '{id}'"));
            return new ItemSale((uint)row[0], (string)row[1], (decimal)row[3]);
        }

        public static void BuyItemsByCart() {
            Customer c = Authentication.GetUser();
            string qInsertSale = $"insert into tbsale(loginid, saledatetime) values('{c.Account.Id}', now())";
            db.ExecuteCommand(qInsertSale);

            var list = CartDAO.GetList();
            list.ForEach(x => {
                string itemname = $"'{x.Name}'";
                string itemprice = $"'{x.Price}'";
                string saleid = $"(select saleId from tbsale where loginid = '{c.Account.Id}' order by saleId desc limit 1)";

                for (int i = 0; i < x.Qnt; i++) {
                    string qInsertItemSale = $"insert into tbitemsale(itemname, itemprice, saleid) values({itemname}, {itemprice}, {saleid})";
                    db.ExecuteCommand(qInsertItemSale);
                }
            });
            db.ExecuteProcedure("sp_DeleteCart", c.Account.Id);
            //db.ExecuteProcedure("sp_InsertSaleByCart", Authentication.GetUser().Account.Id);
        }
    }
}