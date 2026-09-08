using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace clinicAPIsSystem.Data.ClinicConfiguration.UserConfiguration.EmployeeConfiguration.GraduatedConfiguration.MedicalStaffConfiguration
{
    public class MedicalStaffConfiguration:IEntityTypeConfiguration<MedicalStaff>
    {
        public void Configure(EntityTypeBuilder<MedicalStaff> builder)
        {
        }
    }
}
