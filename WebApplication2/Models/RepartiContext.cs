using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication2.Models
{
    public partial class RepartiContext : DbContext
    {
        public RepartiContext()
        {
        }

        public RepartiContext(DbContextOptions<RepartiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Reparti> Reparti { get; set; }

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
            modelBuilder.Entity<Reparti>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("reparti");

                entity.Property(e => e.Jrdesc)
                    .IsRequired()
                    .HasColumnName("JRDESC")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Jrres)
                    .IsRequired()
                    .HasColumnName("JRRES")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
