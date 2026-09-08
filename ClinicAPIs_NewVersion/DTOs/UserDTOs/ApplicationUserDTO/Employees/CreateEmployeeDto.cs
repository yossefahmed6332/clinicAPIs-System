using System.ComponentModel.DataAnnotations;

namespace clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees
{
    public abstract class CreateEmployeeDto: CreateApplicationUserDto
    {
        [Required,Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal SalaryPerHour { get;  set; }
        [Required,Range(0, int.MaxValue)]
        public int HoursWorked {  get;  set; }
        [Required]
        public TimeOnly ShiftStart {  get; set; }
        [Required]
        public TimeOnly ShiftEnd { get; set; }



    }
    
    
}
