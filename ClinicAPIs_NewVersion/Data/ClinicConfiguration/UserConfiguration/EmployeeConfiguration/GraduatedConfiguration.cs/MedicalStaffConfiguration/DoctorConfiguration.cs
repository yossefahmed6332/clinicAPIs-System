using clinicAPIsSystem.Models.User.Employee.Graduated.MedicalStaff;
using Microsoft.EntityFrameworkCore;

namespace clinicAPIsSystem.Data.ClinicConfiguration.UserConfiguration.EmployeeConfiguration.GraduatedConfiguration.MedicalStaffConfiguration
{
    public class DoctorConfiguration:IEntityTypeConfiguration<Doctor>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Doctor> builder)
        {

        }
    }
}
