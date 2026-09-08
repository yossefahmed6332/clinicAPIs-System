using clinicAPIsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;
namespace clinicAPIsSystem.Data.ClinicConfiguration
{
    public class PaymentOperationConfiguration:IEntityTypeConfiguration<PaymentOperation>
    {
        public void Configure(EntityTypeBuilder<PaymentOperation> builder)
        {
            builder.HasKey(x => x.Id);
            //properties
            builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Date).IsRequired().HasColumnType("DateTime2");
            builder.Property(x=>x.OperationType).IsRequired();
            builder.Property(x=>x.Status).IsRequired();
            builder.Property(x => x.PaymentMethod).IsRequired();

            //relationships
            builder.HasOne(x => x.Patient)
                .WithMany(x => x.PaymentOperations)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Accountant)
                .WithMany(x => x.PaymentOperations)
                .HasForeignKey(x => x.AccountantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
