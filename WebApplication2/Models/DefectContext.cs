using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication2.Models
{
    public partial class DefectContext : DbContext
    {
        public DefectContext()
        {
        }

        public DefectContext(DbContextOptions<DefectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Defects> Defects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=192.168.204.41\\Bucket;Initial Catalog=UA_Stage;Persist Security Info=True;User ID=swstage;Password=@UAStage2019.");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Defects>(entity =>
            {
                entity.Property(e => e.DefectId).HasColumnName("DefectId");

                entity.Property(e => e.Nameen).HasColumnName("nameen");

                entity.Property(e => e.Nameit).HasColumnName("nameit");

                entity.Property(e => e.Nameua).HasColumnName("nameua");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
