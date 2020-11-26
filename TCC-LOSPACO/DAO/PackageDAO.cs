using System;
using System.Collections.Generic;
using System.Linq;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public abstract class PackageDAO {
        private static string GetQuery(int index, string category, int? startPrice, int? endPrice) {
            string cat = (category == null || category == "Tudo") ? "" : $"and CatName = '{category}'";
            startPrice = startPrice ?? 0;
            endPrice = endPrice ?? 99999;
            string defaultStr = $"select * from vw_Services where (PackPrice > {startPrice} and PackPrice < {endPrice}) {cat} order by PackPrice";
            string[] OrderingQueries = { $"{defaultStr}", $"{defaultStr}", $"{defaultStr} desc" };
            return OrderingQueries[index];
        }

        /*public static IEnumerable<Package> GetList(int orderIndex, string category, int? sp, int? ep) {
            var list = new List<Package>();
            string query = GetQuery(orderIndex, category, sp, ep);
            Database.ReaderRows(Database.ReturnCommand(query), row => {
                list.Add(new Package((ushort)row[0], (string)row[1], (string)row[2], (string)row[3], (decimal)row[4]));
            });
            return list;
        }*/

        public static IEnumerable<Package> GetList() {
            var list = new List<Package>();
            Database.ReaderRows(Database.ReturnCommand("select * from tbPackage"), row => {
                List<Service> services = GetServicesFromPackage((string)row[1]).Select(s => ServiceDAO.GetByName(s)).ToList();
                list.Add(new Package((ushort)row[0], (string)row[1], (string)row[2], (byte[])row[3], (decimal)row[4], services));
            });
            return list;
        }

        public static Package GetByName(string name) {
            object[] row = Database.ReaderRow(Database.ReturnCommand($"select * from tbPackage where PackName = '{name}'"));
            List<Service> services = GetServicesFromPackage((string)row[1]).Select(s => ServiceDAO.GetByName(s)).ToList();
            Package package = new Package((ushort)row[0], (string)row[1], (string)row[2], (byte[])row[3], (decimal)row[4], services);
            return package;
        }

        public static List<string> GetServicesFromPackage(string name) {
            var list = new List<string>();
            Database.ReaderRows(Database.ReturnCommand($"select * from vw_PackageItems where Nome_do_Pacote='{name}'"), row => {
                list.Add((string)row[1]);
            });
            return list;
        }

        /*public static string GetRating(string name) {
            object rating = Database.ReaderValue(Database.ReturnProcedure("sp_getAverageStarRatingService", name)) ?? "";
            return rating.ToString().Replace(",", ".");
        }*/

        public static int GetMaxPrice() {
            decimal price = (decimal)Database.ReaderValue(Database.ReturnCommand("select PackPrice from tbpackage order by PackPrice desc limit 1;"));
            return (int)Math.Truncate(price) + 5;
        }

        public static int GetMinPrice() {
            decimal price = (decimal)Database.ReaderValue(Database.ReturnCommand("select PackPrice from tbpackage order by PackPrice limit 1;"));
            return (int)Math.Truncate(price); ;
        }
    }
}