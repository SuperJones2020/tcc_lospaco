using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public abstract class AccountDAO {
        public static Account GetByEmail(string email) {
            var row = Database.ReaderRow(Database.ReturnCommand($"select * from tbLogin where LoginEmail = '{email}'"));
            Account account = new Account((uint)row[0], (string)row[1], (string)row[2], RoleDAO.GetById((byte)row[3]));
            return account;
        }

        public static Account GetById(uint id) {
            var row = Database.ReaderRow(Database.ReturnCommand($"select * from tbLogin where LoginId = '{id}'"));
            Account account = new Account((uint)row[0], (string)row[1], (string)row[2], RoleDAO.GetById((byte)row[3]));
            return account;
        }

        public static dynamic Insert(string email, string password) {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password, 12);
            object[] data = Database.ReaderAllValue(Database.ReturnProcedure("sp_InsertLogin", email, passwordHash));
            return new { Type = data[0], Value = data[1] };
        }

        public static bool Login(string email, string password) {
            string query = $"select LoginPass from vw_Login where LoginEmail = '{email}'";
            string currentPassword = (string)Database.ReaderValue(Database.ReturnCommand(query));
            if (currentPassword == null) return false;
            return BCrypt.Net.BCrypt.Verify(password, currentPassword);
        }

        public static dynamic UpdatePassword(string currentPassword, string newPassword) {
            string query = $"select LoginPass from vw_Login where LoginEmail = '{Security.Authentication.GetUser()}'";
            string currentPasswordHashFromDB = (string)Database.ReaderValue(Database.ReturnCommand(query));
            bool passwordMatches = BCrypt.Net.BCrypt.Verify(currentPassword, currentPasswordHashFromDB);
            if (passwordMatches) {
                string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword, 12);
                Database.ExecuteCommand($"update tbLogin set LoginPass='{newPasswordHash}' where LoginEmail='{Security.Authentication.GetUser()}'");
                return new { message = "Senha atualizada com sucesso!", index = 0 };
            } else {
                return new { message = "Senha atual incorreta!", index = 2 };
            }
        }
    }
}