using System;
using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public abstract class ReviewDAO {
        public static IEnumerable<Review> GetList(string service) {
            var list = new List<Review>();
            Database.ReaderRows(Database.ReturnProcedure("sp_SelectCommentsService", service), row => {
                list.Add(new Review((decimal)row[0], (string)row[1], CustomerDAO.GetById(Convert.ToUInt32(row[2]))));
            });
            return list;
        }

        public static void Insert(string name, string review, float rating) {
            Database.ExecuteProcedure("sp_InsertComment", Security.Context.GetUser(), name, review, rating);
        }
    }
}