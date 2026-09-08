using clinicAPIsSystem.Models.User.Employee.Graduated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace clinicAPIsSystem.Data.ClinicConfiguration.UserConfiguration.EmployeeConfiguration.GraduatedConfiguration
{
    public class GraduatedConfiguration:IEntityTypeConfiguration<Graduated>
    {
        public void Configure(EntityTypeBuilder<Graduated> builder)
        {
            builder.Property(a=>a.University).IsRequired().HasMaxLength(100);
            builder.Property(a => a.YearsOfExperience).IsRequired();
            builder.Property(a => a.GraduationYear).IsRequired();
            builder.Property(a=> a.Degree).IsRequired().HasMaxLength(100);
            builder.Property(a => a.License).IsRequired().HasMaxLength(100);

        }
    
    }
}
