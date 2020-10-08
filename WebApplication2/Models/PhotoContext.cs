﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication2.Models
{
    public partial class PhotoContext : DbContext
    {
        public PhotoContext()
        {
        }

        public PhotoContext(DbContextOptions<PhotoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Photos> Photos { get; set; }

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
            modelBuilder.Entity<Photos>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.IdOrd).HasColumnName("Id_ord");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}