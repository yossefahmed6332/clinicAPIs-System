using clinicAPIsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 
namespace clinicAPIsSystem.Data.ClinicConfiguration
{
    public class MedicalRecordConfiguration:IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            //set properties
            builder.HasKey(m => m.Id);
            builder.Property(m=>m.Height).IsRequired().HasPrecision(5,2);
            builder.Property(m => m.Weight).IsRequired().HasPrecision(5,2);
            builder.Property(m=>m.BloodType).IsRequired().HasMaxLength(10);
            builder.Property(m=>m.PatientId).IsRequired();

            //set relationships
            builder.HasOne(m => m.Patient)
                .WithOne(p => p.MedicalRecord)
                .HasForeignKey<MedicalRecord>(m => m.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
