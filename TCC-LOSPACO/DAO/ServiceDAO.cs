using System;
using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public abstract class ServiceDAO {
        private static string GetQuery(int index, string category, int? startPrice, int? endPrice) {
            string cat = (category == null || category == "Tudo") ? "" : $"and CategoryId = '{category}'";
            startPrice = startPrice ?? 0;
            endPrice = endPrice ?? 99999;
            string defaultStr = $"select * from tbservices where (ServPrice > {startPrice} and ServPrice < {endPrice}) {cat} order by ServPrice";
            string[] OrderingQueries = { $"{defaultStr}", $"{defaultStr}", $"{defaultStr} desc" };
            return OrderingQueries[index];
        }

        public static List<Service> GetList(int orderIndex, string category, int? sp, int? ep) {
            var list = new List<Service>();
            string query = GetQuery(orderIndex, category, sp, ep);
            Database.ReaderRows(Database.ReturnCommand(query), row => {
                list.Add(new Service((ushort)row[0], (string)row[1], (decimal)row[2], (string)row[3], (string)row[4], CategoryDAO.GetById((byte)row[5]),
                     TimeSpan.Parse(row[6].ToString()), (byte[])row[7], (string)row[8]));
            });
            return list;
        }

        public static List<Service> GetList() {
            var list = new List<Service>();
            Database.ReaderRows(Database.ReturnCommand("select * from tbservices"), row => {
                list.Add(new Service((ushort)row[0], (string)row[1], (decimal)row[2], (string)row[3], (string)row[4], CategoryDAO.GetById((byte)row[5]),
                     TimeSpan.Parse(row[6].ToString()), (byte[])row[7], (string)row[8]));
            });
            return list;
        }

        public static Service GetById(ushort id) {
            object[] row = Database.ReaderRow(Database.ReturnCommand($"select * from tbservices where ServId = '{id}'"));
            Service service = new Service((ushort)row[0], (string)row[1], (decimal)row[2], (string)row[3], (string)row[4], CategoryDAO.GetById((byte)row[5]),
                     TimeSpan.Parse(row[6].ToString()), (byte[])row[7], (string)row[8]);
            return service;
        }

        public static Service GetByName(string name) {
            object[] row = Database.ReaderRow(Database.ReturnCommand($"select * from tbservices where ServName = '{name}'"));
            Service service = new Service((ushort)row[0], (string)row[1], (decimal)row[2], (string)row[3], (string)row[4], CategoryDAO.GetById((byte)row[5]),
                     TimeSpan.Parse(row[6].ToString()), (byte[])row[7], (string)row[8]);
            return service;
        }

        public static string GetRating(string name) {
            object rating = Database.ReaderValue(Database.ReturnProcedure("sp_getAverageStarRatingService", name)) ?? "";
            return rating.ToString().Replace(",", ".");
        }

        public static int GetMaxPrice() {
            decimal price = (decimal)Database.ReaderValue(Database.ReturnCommand("select ServPrice from vw_Services order by ServPrice desc limit 1;"));
            return (int)Math.Truncate(price) + 5;
        }

        public static int GetMinPrice() {
            decimal price = (decimal)Database.ReaderValue(Database.ReturnCommand("select ServPrice from vw_Services order by ServPrice limit 1;"));
            return (int)Math.Truncate(price); ;
        }
    }
}