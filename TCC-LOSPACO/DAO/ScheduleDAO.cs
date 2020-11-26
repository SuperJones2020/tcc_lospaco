using System.Collections.Generic;
using TCC_LOSPACO.Models;

namespace TCC_LOSPACO.DAO {
    public class ScheduleDAO {
        public static List<Schedule> GetList() {
            var list = new List<Schedule>();
            Database.ReaderRows(Database.ReturnCommand("select * from vw_SchedulesCustomer"), row => {
                list.Add(new Schedule(EmployeeDAO.GetByEmail((string)row[0]), CustomerDAO.GetByEmail((string)row[1]), ServiceDAO.GetByName((string)row[2]), row[3] + ""));
            });
            return list;
        }

        public static Schedule GetByDate(string date) {
            object[] row = Database.ReaderRow(Database.ReturnCommand($"select * from vw_SchedulesCustomer where SchedDateTime = '{date}'"));
            Schedule schedule = new Schedule(EmployeeDAO.GetByEmail((string)row[0]), CustomerDAO.GetByEmail((string)row[1]), ServiceDAO.GetByName((string)row[2]), row[3] + "");
            return schedule;
        }
    }
}