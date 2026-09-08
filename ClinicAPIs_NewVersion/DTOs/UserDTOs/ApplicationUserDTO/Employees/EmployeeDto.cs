namespace clinicAPIsSystem.DTOs.UserDTOs.ApplicationUserDTO.Employees
{
    public abstract class EmployeeDto: ApplicationUserDto
    {
        public decimal SalaryPerHour { get;  set; }
        public int HoursWorked { get;  set; }
        public TimeOnly ShiftStart { get;  set; }
        public TimeOnly ShiftEnd { get;  set; }
 
    
    }
}
