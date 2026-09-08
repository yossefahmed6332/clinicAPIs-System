using clinicAPIsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 
namespace clinicAPIsSystem.Data.ClinicConfiguration
{
    public class ExaminationResultConfiguration:IEntityTypeConfiguration<ExaminationResult>
    {
        public void Configure(EntityTypeBuilder<ExaminationResult> builder)
        {
            //set properties
            builder.HasKey(e => e.Id);
            builder.Property(e => e.TestType).IsRequired().HasMaxLength(500);
            builder.Property(e => e.ResultValue).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Unit).IsRequired().HasMaxLength(50);
            builder.Property(e => e.NormalRange).IsRequired().HasMaxLength(100);
            builder.Property(e => e.RecordedAt).IsRequired().HasColumnType("DateTime2");
            builder.Property(e=>e.Notes).HasMaxLength(1000).IsRequired();
            builder.Property(e => e.NurseId).IsRequired();
            builder.Property(e=>e.MedicalRecordId).IsRequired();
            //set relationships 

            builder.HasOne(e => e.Nurse)
                .WithMany(e => e.ExaminationResults)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.MedicalRecord)
                .WithMany(e => e.ExaminationResults)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
