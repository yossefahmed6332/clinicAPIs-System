using clinicAPIsSystem.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace clinicAPIsSystem.Data.ClinicConfiguration.UserConfiguration
{
    public class ApplicationUserConfiguration:IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            // Set properties
            builder.Property(a=>a.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(a => a.Email)
                .IsRequired();
            builder.Property(a => a.PhoneNumber)
                .IsRequired()
                .HasMaxLength(30);
            builder.Property(a=>a.UserName)
                .IsRequired()
                .HasMaxLength(50);

            // Set indexes 
            builder.HasIndex(a => a.Email)
                .IsUnique();
            builder.HasIndex(a => a.PhoneNumber)
                .IsUnique();




        }
    }
}
