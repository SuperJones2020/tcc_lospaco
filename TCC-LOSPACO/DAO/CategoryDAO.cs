using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public abstract class CategoryDAO {
        private static Database db = new Database();
        public static List<Category> GetList() {
            var list = new List<Category>();
            db.ReaderRows(db.ReturnCommand("select * from tbCategories"), row => {
                list.Add(new Category((byte)row[0], (string)row[1]));
            });
            return list;
        }

        public static Category GetByName(string name) {
            object[] row = db.ReaderRow(db.ReturnCommand($"select * from tbCategories where catname = '{name}'"));
            return new Category((byte)row[0], (string)row[1]);
        }

        public static Category GetById(byte id) {
            object[] row = db.ReaderRow(db.ReturnCommand($"select * from tbCategories where categoryid = '{id}'"));
            return new Category((byte)row[0], (string)row[1]);
        }
    }
}