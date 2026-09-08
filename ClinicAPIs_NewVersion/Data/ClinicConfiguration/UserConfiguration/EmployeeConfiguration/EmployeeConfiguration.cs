using Microsoft.EntityFrameworkCore;
using clinicAPIsSystem.Models.User.Employee;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace clinicAPIsSystem.Data.ClinicConfiguration.UserConfiguration.EmployeeConfiguration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            //set properties 
            builder.Property(a => a.SalaryPerHour).IsRequired().HasPrecision(10,2);
            builder.Property(a => a.HoursWorked).IsRequired();
            builder.Property(a => a.ShiftStart).IsRequired().HasColumnType("Time");
            builder.Property(a => a.ShiftEnd).IsRequired().HasColumnType("Time");




        }
    }
}
