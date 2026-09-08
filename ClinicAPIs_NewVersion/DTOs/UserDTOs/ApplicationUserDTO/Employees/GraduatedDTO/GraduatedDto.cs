using System.ComponentModel.DataAnnotations;

namespace clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees.GraduatedDTO
{
    public abstract class GraduatedDto:EmployeeDto
    {
        public string Degree { get; set; } = null!; 
        public string University { get; set; } = null!; 
        public int YearsOfExperience { get;  set; }
        public int GraduationYear { get;  set; }
        public string License { get; set; } = null!; 

  
    }
}
