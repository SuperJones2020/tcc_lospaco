namespace TCC_LOSPACO.Models {
    public class Schedule {
        public uint Id { get; private set; }
        public Employee Employee { get; private set; }
        public Customer Customer { get; private set; }
        public Service Service { get; private set; }
        public string Date { get; private set; }
        public bool IsPerformed { get; private set; }

        public Schedule(uint id, Employee employee, Customer customer, Service service, string date, bool isPerformed) {
            Id = id;
            Employee = employee;
            Customer = customer;
            Service = service;
            Date = date;
            IsPerformed = isPerformed;
        }

        public Schedule() { }
    }
}