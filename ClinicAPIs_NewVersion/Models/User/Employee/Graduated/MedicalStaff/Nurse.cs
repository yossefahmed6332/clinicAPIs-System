using clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO.MedicalStaffDTO.Nurse;

namespace clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff
{
    public class Nurse:MedicalStaff
    {
        public Nurse(string firstName, string lastName, string userName, string email, string phoneNumber, decimal salaryPerHour, int hoursWorked, TimeOnly shiftStart, TimeOnly shiftEnd, string degree, string university, int yearsOfExperience, int graduationYear, string license) : base(firstName, lastName, userName, email, phoneNumber, salaryPerHour, hoursWorked, shiftStart, shiftEnd, degree, university, yearsOfExperience, graduationYear, license)
        {
        }
        public Nurse() { }

        public ICollection<VitalSigns> VitalSigns { get;private set; } = new HashSet<VitalSigns>();
        public ICollection<ExaminationResult> ExaminationResults { get; private set; } = new HashSet<ExaminationResult>();

        public void UpdateNurseInfo(string firstName, string lastName, string userName, string email, string phoneNumber, decimal salaryPerHour, int hoursWorked, TimeOnly shiftStart, TimeOnly shiftEnd, string degree, string university, int yearsOfExperience, int graduationYear, string license)
        {
            UpdateMedicalStaffInfo(firstName, lastName, userName, email, phoneNumber, salaryPerHour, hoursWorked, shiftStart, shiftEnd, degree, university, yearsOfExperience, graduationYear, license);
        }

    }
}
