namespace clinicAPIsSystem.Models.User.Employee
{
    public class Cleaner:Employee
    {
        public Cleaner(string firstName, string lastName, string userName, string email,string phoneNumber,decimal salaryPerHour,int hoursWorked,TimeOnly shiftStart,TimeOnly shiftEnd) : base(firstName, lastName, userName, email, phoneNumber, salaryPerHour, hoursWorked, shiftStart, shiftEnd)
        {
        }
        public Cleaner()
        {
        }

        public void UpdateCleanerInfo(string firstName, string lastName, string userName, string email, string phoneNumber, decimal salaryPerHour, int hoursWorked, TimeOnly shiftStart, TimeOnly shiftEnd)
        {
            UpdateEmployeeInfo(firstName, lastName, userName, email, phoneNumber, salaryPerHour, hoursWorked, shiftStart, shiftEnd);
        }
    }
}
