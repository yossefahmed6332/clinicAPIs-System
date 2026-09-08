using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using clinicAPIsSystem.Models;
using Microsoft.EntityFrameworkCore;
namespace clinicAPIsSystem.Data.ClinicConfiguration
{
    public class VitalsignsConfiguration: IEntityTypeConfiguration<VitalSigns>
    {
        public void Configure(EntityTypeBuilder<VitalSigns> builder)
        {
            builder.HasKey(v => v.Id);
            //set properties
            builder.Property(v => v.BloodPressureSystolic).IsRequired();
            builder.Property(v => v.BloodPressureDiastolic).IsRequired();
            builder.Property(v => v.HeartRate).IsRequired();
            builder.Property(v => v.Temperature).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(v => v.OxygenSaturation).IsRequired().HasPrecision(5,2);
            builder.Property(v => v.RecordedAt).IsRequired().HasColumnType("DateTime2");
            builder.Property(v => v.NurseId).IsRequired();
            builder.Property(v => v.MedicalRecordId).IsRequired();


            //set relationships
            builder.HasOne(v => v.Nurse)
                .WithMany(n => n.VitalSigns)
                .HasForeignKey(v => v.NurseId)
                .OnDelete(DeleteBehavior.Restrict);
                
            builder.HasOne(v => v.MedicalRecord)
                .WithMany(m => m.VitalSigns)
                .HasForeignKey(v => v.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
