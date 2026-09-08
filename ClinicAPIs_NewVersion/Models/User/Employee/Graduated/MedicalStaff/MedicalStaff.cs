namespace clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff
{
    public abstract class MedicalStaff:Graduated
    {
        public ICollection<Appointment> Appointments { get; private set; } = new HashSet<Appointment>();


        public MedicalStaff(string firstName, string lastName, string userName, string email,string phoneNumber, decimal salaryPerHour, int hoursWorked, TimeOnly shiftStart, TimeOnly shiftEnd, string degree, string university, int yearsOfExperience, int graduationYear, string license) : base(firstName, lastName, userName, email, phoneNumber, salaryPerHour, hoursWorked, shiftStart, shiftEnd, degree, university, yearsOfExperience, graduationYear, license)
        {
        }
        public MedicalStaff() { }

        public void UpdateMedicalStaffInfo(string firstName, string lastName, string userName, string email, string phoneNumber, decimal salaryPerHour, int hoursWorked, TimeOnly shiftStart, TimeOnly shiftEnd, string degree, string university, int yearsOfExperience, int graduationYear, string license)
        {
            UpdateGraduatedInfo(firstName, lastName, userName, email, phoneNumber, salaryPerHour, hoursWorked, shiftStart, shiftEnd, degree, university, yearsOfExperience, graduationYear, license);
        }

    }
}
