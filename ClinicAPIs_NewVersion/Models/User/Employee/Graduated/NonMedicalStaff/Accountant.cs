using Microsoft.VisualBasic;

namespace clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff
{
    public class Accountant:NonMedicalStaff
    {
        public ICollection<PaymentOperation> PaymentOperations { get; private set; } = new HashSet<PaymentOperation>();

        public Accountant(string firstName, string lastName, string userName, string email, string phoneNumber, decimal salaryPerHour, int hoursWorked, TimeOnly shiftStart, TimeOnly shiftEnd, string degree, string university, int yearsOfExperience, int graduationYear, string license) : base(firstName, lastName, userName, email, phoneNumber, salaryPerHour, hoursWorked, shiftStart, shiftEnd, degree, university, yearsOfExperience, graduationYear, license)
        {
        }

        public Accountant() { }

        public void UpdateAccountantInfo(string firstName, string lastName, string userName, string email, string phoneNumber, decimal salaryPerHour, int hoursWorked, TimeOnly shiftStart, TimeOnly shiftEnd, string degree, string university, int yearsOfExperience, int graduationYear, string license)
        {
            UpdateNonMedicalStaffInfo(firstName, lastName, userName, email, phoneNumber, salaryPerHour, hoursWorked, shiftStart, shiftEnd, degree, university, yearsOfExperience, graduationYear, license);
        }

    }
}
