using System;
using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public abstract class PackageDAO {
        private static Database db = new Database();
        private static string GetQuery(int index, int? startPrice, int? endPrice) {
            startPrice = startPrice ?? 0;
            endPrice = endPrice ?? 99999;
            string defaultStr = $"select * from tbpackage where (PackPrice >= {startPrice} and PackPrice <= {endPrice}) order by PackPrice";
            string[] OrderingQueries = { $"{defaultStr}", $"{defaultStr}", $"{defaultStr} desc" };
            return OrderingQueries[index];
        }

        public static List<Package> GetList(int orderIndex, int? sp, int? ep) {
            var list = new List<Package>();
            string query = GetQuery(orderIndex, sp, ep);
            db.ReaderRows(db.ReturnCommand(query), row => {
                list.Add(new Package((ushort)row[0], (string)row[1], (string)row[2], (string)row[3], (byte[])row[4], (decimal)row[5], GetServicesFromPackage((ushort)row[0])));
            });
            return list;
        }

        public static List<Package> GetList() {
            var list = new List<Package>();
            db.ReaderRows(db.ReturnCommand("select * from tbPackage"), row => list.Add(new Package((ushort)row[0], (string)row[1], (string)row[2], (string)row[3], (byte[])row[4], (decimal)row[5], GetServicesFromPackage((ushort)row[0]))));
            return list;
        }

        public static Package GetById(ushort id) {
            object[] row = db.ReaderRow(db.ReturnCommand($"select * from tbPackage where PackId = '{id}'"));
            Package package = new Package((ushort)row[0], (string)row[1], (string)row[2], (string)row[3], (byte[])row[4], (decimal)row[5], GetServicesFromPackage((ushort)row[0]));
            return package;
        }

        public static Package GetByName(string name) {
            object[] row = db.ReaderRow(db.ReturnCommand($"select * from tbPackage where packname = '{name}'"));
            Package package = new Package((ushort)row[0], (string)row[1], (string)row[2], (string)row[3], (byte[])row[4], (decimal)row[5], GetServicesFromPackage((ushort)row[0]));
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

        public static dynamic GetCartPackageByName(string name) {
            string query = $"select ItemId, ItemName, ItemQnt, itemPrice, itemImage from vw_cart where ItemName='{name}' and Loginid = '{Security.Authentication.GetUser().Account.Id}'";
            object[] row = db.ReaderRow(db.ReturnCommand(query));
            if (row.Length == 0) return null;
            return new { Id = (ushort)row[0], Name = (string)row[1], Quantity = (byte)row[2], Price = (decimal)row[3], Image = (byte[])row[4], GetByName((string)row[1]).Services };
        }
    }
}