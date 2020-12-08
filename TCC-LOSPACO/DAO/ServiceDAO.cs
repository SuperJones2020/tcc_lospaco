using System;
using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public abstract class ServiceDAO {
        private static Database db = new Database();
        private static string GetQuery(int index, string category, int? startPrice, int? endPrice) {
            string cat = (category == null || category == "Tudo") ? "" : $"and CategoryId = '{category}'";
            startPrice = startPrice ?? 0;
            endPrice = endPrice ?? 99999;
            string defaultStr = $"select * from vw_services where (ServPrice >= {startPrice} and ServPrice <= {endPrice}) {cat} order by ";
            string[] OrderingQueries = { $"{defaultStr} salecount", $"{defaultStr} servprice", $"{defaultStr} servprice desc" };
            return OrderingQueries[index];
        }

        public static List<Service> GetList(int orderIndex, string category, int? sp, int? ep) {
            var list = new List<Service>();
            string query = GetQuery(orderIndex, category, sp, ep);
            db.ReaderRows(db.ReturnCommand(query), row => {
                uint salesCount = Convert.ToUInt32((decimal)row[9]);
                decimal? starRating = row[10] != DBNull.Value ? Convert.ToDecimal(row[10].ToString().Replace('.', ',') + "") : (decimal?)null;
                list.Add(new Service((ushort)row[0], (string)row[1], (decimal)row[2], (string)row[3], (string)row[4], CategoryDAO.GetById((byte)row[5]),
                     TimeSpan.Parse(row[6].ToString()), (byte[])row[7], (string)row[8], salesCount, starRating));
            });
            return list;
        }

        public static List<Service> GetList() {
            var list = new List<Service>();
            db.ReaderRows(db.ReturnCommand("select * from vw_services"), row => {
                uint salesCount = Convert.ToUInt32((decimal)row[9]);
                decimal? starRating = row[10] != DBNull.Value ? Convert.ToDecimal(row[10].ToString().Replace('.', ',') + "") : (decimal?)null;
                list.Add(new Service((ushort)row[0], (string)row[1], (decimal)row[2], (string)row[3], (string)row[4], CategoryDAO.GetById((byte)row[5]),
                     TimeSpan.Parse(row[6].ToString()), (byte[])row[7], (string)row[8], salesCount, starRating));
            });
            return list;
        }

        public static Service GetById(ushort id) {
            object[] row = db.ReaderRow(db.ReturnCommand($"select * from vw_services where ServId = '{id}'"));
            uint salesCount = Convert.ToUInt32((decimal)row[9]);
            decimal? starRating = row[10] != DBNull.Value ? Convert.ToDecimal(row[10].ToString().Replace('.', ',') + "") : (decimal?)null;
            Service service = new Service((ushort)row[0], (string)row[1], (decimal)row[2], (string)row[3], (string)row[4], CategoryDAO.GetById((byte)row[5]),
                     TimeSpan.Parse(row[6].ToString()), (byte[])row[7], (string)row[8], salesCount, starRating);
            return service;
        }

        public static Service GetByName(string name) {
            object[] row = db.ReaderRow(db.ReturnCommand($"select * from vw_services where ServName = '{name}'"));
            uint salesCount = Convert.ToUInt32((decimal)row[9]);
            decimal? starRating = row[10] != DBNull.Value ? Convert.ToDecimal(row[10].ToString().Replace('.', ',') + "") : (decimal?)null;
            Service service = new Service((ushort)row[0], (string)row[1], (decimal)row[2], (string)row[3], (string)row[4], CategoryDAO.GetById((byte)row[5]),
                     TimeSpan.Parse(row[6].ToString()), (byte[])row[7], (string)row[8], salesCount, starRating);
            return service;
        }

        public static int GetMaxPrice() {
            object price = db.ReaderValue(db.ReturnCommand("select ServPrice from vw_services order by ServPrice desc limit 1;"));
            return (int)Math.Truncate(Convert.ToDecimal(price)) + 5;
        }

        public static int GetMinPrice() {
            object price = db.ReaderValue(db.ReturnCommand("select ServPrice from vw_services order by ServPrice limit 1;"));
            return (int)Math.Truncate(Convert.ToDecimal(price)); ;
        }

        public static dynamic GetCartServiceByName(string name) {
            string query = $"select ItemId, ItemName, ItemQnt, itemPrice, itemImage from vw_cart where ItemName='{name}' and Loginid = '{Security.Authentication.GetUser().Account.Id}'";
            object[] row = db.ReaderRow(db.ReturnCommand(query));
            if (row.Length == 0) return null;
            return new { Id = (ushort)row[0], Name = (string)row[1], Quantity = (byte)row[2], Price = (decimal)row[3], Image = (byte[])row[4] };
        }
    }
}