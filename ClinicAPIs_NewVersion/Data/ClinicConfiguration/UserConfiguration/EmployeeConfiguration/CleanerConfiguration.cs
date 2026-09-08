using clinicAPIsSystem.Models.User.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace clinicAPIsSystem.Data.ClinicConfiguration.UserConfiguration.EmployeeConfiguration
{
    public class CleanerConfiguration:IEntityTypeConfiguration<Cleaner>
    {
        public void Configure(EntityTypeBuilder<Cleaner> builder)
        {
        }
    }
}
