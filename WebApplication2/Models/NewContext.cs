using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication2.Models
{
    public partial class NewContext : DbContext
    {
        public NewContext()
        {
        }

        public NewContext(DbContextOptions<NewContext> options)
            : base(options)
        {
        }

        public virtual DbSet<New> New { get; set; }

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
            modelBuilder.Entity<New>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("New");

                entity.Property(e => e.Artcli)
                    .HasColumnName("artcli")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Articolo)
                    .IsRequired()
                    .HasColumnName("articolo")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Bollone)
                    .HasColumnName("bollone")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ciclo)
                    .IsRequired()
                    .HasColumnName("ciclo")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Codice)
                    .HasColumnName("codice")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Codmas)
                    .HasColumnName("codmas")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Codprg)
                    .HasColumnName("codprg")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Codsot)
                    .HasColumnName("codsot")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Codsta)
                    .HasColumnName("codsta")
                    .HasColumnType("datetime");

                entity.Property(e => e.Colore)
                    .HasColumnName("colore")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ColoreDesc)
                    .HasColumnName("colore_desc")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Commessa)
                    .IsRequired()
                    .HasColumnName("commessa")
                    .HasMaxLength(19)
                    .IsUnicode(false);

                entity.Property(e => e.ComponenteSuccessivo)
                    .HasColumnName("componente_successivo")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.DataInizio)
                    .HasColumnName("data_inizio")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataProduzione)
                    .HasColumnName("data_produzione")
                    .HasColumnType("datetime");

                entity.Property(e => e.Datsca)
                    .HasColumnName("datsca")
                    .HasColumnType("date");

                entity.Property(e => e.Datsov)
                    .HasColumnName("datsov")
                    .HasColumnType("date");

                entity.Property(e => e.Ditta)
                    .IsRequired()
                    .HasColumnName("ditta")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fase)
                    .IsRequired()
                    .HasColumnName("fase")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Fustella)
                    .HasColumnName("fustella")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Linea)
                    .HasColumnName("linea")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.LineaComponente)
                    .HasColumnName("linea_componente")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Lotto)
                    .HasColumnName("lotto")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Matcod)
                    .HasColumnName("matcod")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Materiale)
                    .HasColumnName("materiale")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Numord).HasColumnName("numord");

                entity.Property(e => e.Proord)
                    .HasColumnName("proord")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.QtaContata).HasColumnName("qta_contata");

                entity.Property(e => e.QtaOrd)
                    .HasColumnName("qta_ord")
                    .HasColumnType("decimal(11, 2)");

                entity.Property(e => e.QtaProdotta).HasColumnName("qta_prodotta");

                entity.Property(e => e.QtaScarti).HasColumnName("qta_scarti");

                entity.Property(e => e.QtaTaglia)
                    .HasColumnName("qta_taglia")
                    .HasColumnType("decimal(11, 3)");

                entity.Property(e => e.Reparto)
                    .HasColumnName("reparto")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RigaCicloStart).HasColumnName("riga_ciclo_start");

                entity.Property(e => e.Rigord).HasColumnName("rigord");

                entity.Property(e => e.RismachCodStart)
                    .HasColumnName("rismach_cod_start")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Seqgps).HasColumnName("seqgps");

                entity.Property(e => e.Sequenza).HasColumnName("sequenza");

                entity.Property(e => e.StatoOperazione)
                    .HasColumnName("stato_operazione")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.StatoProdix)
                    .HasColumnName("stato_prodix")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StatoRiga)
                    .IsRequired()
                    .HasColumnName("stato_riga")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Taglia)
                    .HasColumnName("taglia")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TempoPaio).HasColumnName("tempo_paio");

                entity.Property(e => e.TestCiclo)
                    .HasColumnName("test_ciclo")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.TestDistinta)
                    .HasColumnName("test_distinta")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.TipoMacchina)
                    .HasColumnName("tipo_macchina")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
