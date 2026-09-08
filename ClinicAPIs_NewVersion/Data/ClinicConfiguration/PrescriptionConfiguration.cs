using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using clinicAPIsSystem.Models;
namespace clinicAPIsSystem.Data.ClinicConfiguration
{
    public class PrescriptionConfiguration:IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.HasKey(p => p.Id);

            //set Properties
            builder.Property(p=>p.MedicalName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Dosage).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Frequency).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Duration).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Instructions).IsRequired(false).HasMaxLength(500);
            builder.Property(p=>p.Diagnosis).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.Date).IsRequired().HasColumnType("DateTime2");
            builder.Property(p => p.DoctorId).IsRequired();
            builder.Property(p => p.MedicalRecordId).IsRequired();

            //set relationships
            builder.HasOne(p => p.Doctor)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(p => p.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.MedicalRecord)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(p => p.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);


        }

    }
}
