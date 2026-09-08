using clinicAPIsSystem.Models.User.Employee.Graduated.NonMedicalStaff;
using Microsoft.EntityFrameworkCore;

namespace clinicAPIsSystem.Data.ClinicConfiguration.UserConfiguration.EmployeeConfiguration.GraduatedConfiguration.NonMedicalStaffConfiguration
{
    public class ReceptionistConfiguration:IEntityTypeConfiguration<Receptionist>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Receptionist> builder)
        {

        }
    }
}
