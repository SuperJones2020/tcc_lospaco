using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public abstract class RoleDAO {
        public static Role GetById(byte id) {
            var row = Database.ReaderRow(Database.ReturnCommand($"select * from tblevelaccess where levelid='{id}'"));
            return new Role((byte)row[0], (string)row[1]);
        }
    }
}