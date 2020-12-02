using System;
using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public abstract class PackageDAO {
        private static Database db = new Database();
        private static string GetQuery(int index, string category, int? startPrice, int? endPrice) {
            string cat = (category == null || category == "Tudo") ? "" : $"and CatName = '{category}'";
            startPrice = startPrice ?? 0;
            endPrice = endPrice ?? 99999;
            string defaultStr = $"select * from vw_Services where (PackPrice > {startPrice} and PackPrice < {endPrice}) {cat} order by PackPrice";
            string[] OrderingQueries = { $"{defaultStr}", $"{defaultStr}", $"{defaultStr} desc" };
            return OrderingQueries[index];
        }

        public static List<Package> GetList(int orderIndex, string category, int? sp, int? ep) {
            var list = new List<Package>();
            string query = GetQuery(orderIndex, category, sp, ep);
            db.ReaderRows(db.ReturnCommand(query), row => {
                //list.Add(new Package((ushort)row[0], (string)row[1], (string)row[2], (string)row[3], (decimal)row[4]));
            });
            return list;
        }

        public static List<Package> GetList() {
            var list = new List<Package>();
            db.ReaderRows(db.ReturnCommand("select * from tbPackage"), row => list.Add(new Package((ushort)row[0], (string)row[1], (string)row[2], (byte[])row[3], (decimal)row[4], GetServicesFromPackage((ushort)row[0]))));
            return list;
        }

        public static Package GetById(ushort id) {
            object[] row = db.ReaderRow(db.ReturnCommand($"select * from tbPackage where PackId = '{id}'"));
            Package package = new Package((ushort)row[0], (string)row[1], (string)row[2], (byte[])row[3], (decimal)row[4], GetServicesFromPackage((ushort)row[0]));
            return package;
        }

        public static List<Service> GetServicesFromPackage(ushort id) {
            var list = new List<Service>();
            db.ReaderRows(db.ReturnCommand($"select * from tbpackitem where packid='{id}'"), row => {
                list.Add(ServiceDAO.GetById((ushort)row[0]));
            });
            return list; ;
        }

        public static int GetMaxPrice() {
            object price = db.ReaderValue(db.ReturnCommand("select PackPrice from tbpackage order by PackPrice desc limit 1;"));
            return (int)Math.Truncate(Convert.ToDecimal(price)) + 5;
        }

        public static int GetMinPrice() {
            object price = db.ReaderValue(db.ReturnCommand("select PackPrice from tbpackage order by PackPrice limit 1;"));
            return (int)Math.Truncate(Convert.ToDecimal(price)); ;
        }
    }
}