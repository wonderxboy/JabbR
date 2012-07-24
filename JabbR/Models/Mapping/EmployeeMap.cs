using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace JabbR.Models.Mapping
{
    public class EmployeeMap : EntityTypeConfiguration<Employee>
    {
        public EmployeeMap()
        {
            // Primary Key
            this.HasKey(t => t.DirectoryId);

            // Properties
            this.Property(t => t.EmployeeNumber)
                .IsFixedLength()
                .HasMaxLength(9);

            this.Property(t => t.UserId)
                .IsFixedLength()
                .HasMaxLength(9);

            this.Property(t => t.EmployeeStatus)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.RecordType)
                .HasMaxLength(20);

            this.Property(t => t.LastName)
                .HasMaxLength(27);

            this.Property(t => t.FirstName)
                .HasMaxLength(15);

            this.Property(t => t.JobItemNo)
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.JobTitle)
                .HasMaxLength(30);

            this.Property(t => t.WorkPhone)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.WorkExtension)
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.EngineerNumber)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.DivisionDistrictIndicator)
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.CrewNumber)
                .IsFixedLength()
                .HasMaxLength(5);

            this.Property(t => t.Location)
                .HasMaxLength(30);

            this.Property(t => t.Email)
                .HasMaxLength(319);

            this.Property(t => t.LastChangeUser)
                .IsRequired()
                .HasMaxLength(9);

            // Table & Column Mappings
            this.ToTable("Directory", "UsrProv");
            this.Property(t => t.DirectoryId).HasColumnName("DirectoryId");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.EmployeeNumber).HasColumnName("EmployeeNumber");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.EmployeeStatus).HasColumnName("EmployeeStatus");
            this.Property(t => t.RecordType).HasColumnName("RecordType");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.BudgetId).HasColumnName("BudgetId");
            this.Property(t => t.JobItemNo).HasColumnName("JobItemNo");
            this.Property(t => t.JobTitle).HasColumnName("JobTitle");
            this.Property(t => t.WorkPhone).HasColumnName("WorkPhone");
            this.Property(t => t.WorkExtension).HasColumnName("WorkExtension");
            this.Property(t => t.EngineerNumber).HasColumnName("EngineerNumber");
            this.Property(t => t.SectionId).HasColumnName("SectionId");
            this.Property(t => t.DivisionDistrictIndicator).HasColumnName("DivisionDistrictIndicator");
            this.Property(t => t.CrewNumber).HasColumnName("CrewNumber");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.ReportsToId).HasColumnName("ReportsToId");
            this.Property(t => t.DeleteFlag).HasColumnName("DeleteFlag");
            this.Property(t => t.DeleteFlagDate).HasColumnName("DeleteFlagDate");
            this.Property(t => t.TerminalServices).HasColumnName("TerminalServices");
            this.Property(t => t.Enabled).HasColumnName("Enabled");
            this.Property(t => t.DateEnabled).HasColumnName("DateEnabled");
            this.Property(t => t.DateDisabled).HasColumnName("DateDisabled");
            this.Property(t => t.LastChangeDate).HasColumnName("LastChangeDate");
            this.Property(t => t.LastChangeUser).HasColumnName("LastChangeUser");

            // Relationships
            //this.HasOptional(t => t.Budget)
            //    .WithMany(t => t.Directories)
            //    .HasForeignKey(d => d.BudgetId);
            //this.HasOptional(t => t.Section)
            //    .WithMany(t => t.Directories)
            //    .HasForeignKey(d => d.SectionId);
            this.HasOptional(t => t.ReportsTo)
                .WithMany(t => t.Employees)
                .HasForeignKey(d => d.ReportsToId);

        }
    }
}
