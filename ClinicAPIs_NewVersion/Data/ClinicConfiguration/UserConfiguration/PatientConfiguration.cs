using clinicAPIsSystem.Models.User; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace clinicAPIsSystem.Data.ClinicConfiguration.UserConfiguration
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {

            builder.Property(p => p.MedicalRecordId).IsRequired();
            builder.HasIndex(p => p.MedicalRecordId).IsUnique();
            builder.HasOne(p => p.MedicalRecord)
                .WithOne(m => m.Patient)
                .HasForeignKey<Patient>(p => p.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
