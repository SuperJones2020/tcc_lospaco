using System;
using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public abstract class ReviewDAO {
        private static Database db = new Database();
        public static List<Review> GetList(ushort serviceId) {
            var list = new List<Review>();
            db.ReaderRows(db.ReturnProcedure("sp_SelectCommentsService", serviceId), row => {
                list.Add(new Review((decimal)row[0], (string)row[1], CustomerDAO.GetById(Convert.ToUInt32(row[2]))));
            });
            return list;
        }

        public static void Insert(string name, string review, float rating) => db.ExecuteProcedure("sp_InsertComment", Security.Authentication.GetUser().Account.Email, name, review, rating);

    }
}