namespace clinicAPIsSystem.Models.User.Employee.Graduated
{
    public abstract class Graduated:Employee
    {
        public string? Degree { get; protected set; } 
        public string? University { get; protected set; } 
        public int YearsOfExperience { get; protected set; }
        public int GraduationYear { get; protected set; }
        public string? License { get; protected set; }

        public Graduated(string firstName, string lastName, string userName, string email,string phoneNumber, decimal salaryPerHour, int hoursWorked, TimeOnly shiftStart, TimeOnly shiftEnd, string degree, string university, int yearsOfExperience, int graduationYear, string license) : base(firstName, lastName, userName, email, phoneNumber, salaryPerHour, hoursWorked, shiftStart, shiftEnd)
        {
            Degree = degree;
            University = university;
            YearsOfExperience = yearsOfExperience;
            GraduationYear = graduationYear;
            License = license;
        }
        public Graduated()
        {
        }

        public void UpdateGraduatedInfo(string firstName, string lastName, string userName, string email, string phoneNumber, decimal salaryPerHour, int hoursWorked, TimeOnly shiftStart, TimeOnly shiftEnd, string degree, string university, int yearsOfExperience, int graduationYear, string license)
        {
            UpdateEmployeeInfo(firstName, lastName, userName, email, phoneNumber, salaryPerHour, hoursWorked, shiftStart, shiftEnd);
            Degree = degree;
            University = university;
            YearsOfExperience = yearsOfExperience;
            GraduationYear = graduationYear;
            License = license;
        }


    }
}
