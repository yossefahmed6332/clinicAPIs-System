using clinicAPIsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace clinicAPIsSystem.Data.ClinicConfiguration
{
    public class FinancialReportConfiguration:IEntityTypeConfiguration<FinancialReport>
    {
        public void Configure(EntityTypeBuilder<FinancialReport> builder)
        {
            builder.HasKey(f => f.Id);
            //set properties
            builder.Property(f => f.MonthlyExpenses).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(f => f.MonthlyRevenue).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(f => f.NetProfit).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(f => f.Date).IsRequired().HasColumnType("datetime2");




        }
    }
    }

