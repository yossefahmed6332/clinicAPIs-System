using clinicAPIsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace clinicAPIsSystem.Data.ClinicConfiguration
{
    public class AppointmentConfiguration:IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);
            //set properties for the appointment entity
            builder.Property(a=>a.StartDate).IsRequired().HasColumnType("DateTime2");
            builder.Property(a => a.EndDate).IsRequired().HasColumnType("DateTime2"); 
            builder.Property(a=>a.PatientId).IsRequired();
            builder.Property(a => a.DoctorId).IsRequired();
            builder.Property(a=> a.NurseId).IsRequired();
            //set relationships for the appointment entity
            builder.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.Nurse)
                .WithMany(n => n.Appointments)
                .HasForeignKey(a => a.NurseId)
                .OnDelete(DeleteBehavior.Restrict);
           





        }
    }
}
