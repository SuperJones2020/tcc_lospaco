using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public abstract class RoleDAO {
        private static Database db = new Database();
        public static Role GetById(byte id) {
            var row = db.ReaderRow(db.ReturnCommand($"select * from tblevelaccess where levelid='{id}'"));
            return new Role((byte)row[0], (string)row[1]);
        }
    }
}