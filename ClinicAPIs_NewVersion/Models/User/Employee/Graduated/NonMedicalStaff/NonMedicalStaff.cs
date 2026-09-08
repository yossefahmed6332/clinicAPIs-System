namespace clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff
{
    public abstract class NonMedicalStaff: Graduated
    {

        public NonMedicalStaff(string firstName, string lastName, string userName, string email, string phoneNumber, decimal salaryPerHour, int hoursWorked, TimeOnly shiftStart, TimeOnly shiftEnd, string degree, string university, int yearsOfExperience, int graduationYear, string license) : base(firstName, lastName, userName, email, phoneNumber, salaryPerHour, hoursWorked, shiftStart, shiftEnd, degree, university, yearsOfExperience, graduationYear, license)
        {
        }
        public NonMedicalStaff() { }

        public void UpdateNonMedicalStaffInfo(string firstName, string lastName, string userName, string email, string phoneNumber, decimal salaryPerHour, int hoursWorked, TimeOnly shiftStart, TimeOnly shiftEnd, string degree, string university, int yearsOfExperience, int graduationYear, string license)
        {
            UpdateGraduatedInfo(firstName, lastName, userName, email, phoneNumber, salaryPerHour, hoursWorked, shiftStart, shiftEnd, degree, university, yearsOfExperience, graduationYear, license);
        }
    }
}
