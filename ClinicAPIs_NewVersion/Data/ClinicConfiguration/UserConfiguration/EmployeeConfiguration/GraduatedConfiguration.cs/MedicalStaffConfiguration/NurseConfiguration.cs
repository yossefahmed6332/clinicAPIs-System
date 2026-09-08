using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 
using Microsoft.EntityFrameworkCore;
namespace clinicAPIsSystem.Data.ClinicConfiguration.UserConfiguration.EmployeeConfiguration.GraduatedConfiguration.MedicalStaffConfiguration
{
    public class NurseConfiguration: IEntityTypeConfiguration<Nurse>
    {
        public void Configure(EntityTypeBuilder<Nurse> builder)
        {
        }
            
    }
}
