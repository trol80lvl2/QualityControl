using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication2.Models
{
    public partial class ExamplContext : DbContext
    {
        public ExamplContext()
        {
        }

        public ExamplContext(DbContextOptions<ExamplContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Example> Example { get; set; }
        public virtual DbSet<Colori> Colori { get; set; }
        public virtual DbSet<New> News { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Reparti> Repartis { get; set; }
        public virtual DbSet<Stagio> Stagio { get; set; }
        public DbSet<Check> Checks { get; set; }
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
            modelBuilder.Entity<Example>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Example");

                entity.Property(e => e.Artcon)
                    .HasColumnName("artcon")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Codice)
                    .HasColumnName("codice")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Codmas)
                    .HasColumnName("codmas")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Codnu1)
                    .HasColumnName("codnu1")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Codnum)
                    .HasColumnName("codnum")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Codsot)
                    .HasColumnName("codsot")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Codsta).HasColumnName("codsta");

                entity.Property(e => e.Confer)
                    .HasColumnName("confer")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Conseg)
                    .HasColumnName("conseg")
                    .HasColumnType("date");

                entity.Property(e => e.Datins)
                    .HasColumnName("datins")
                    .HasColumnType("date");

                entity.Property(e => e.Datsov)
                    .HasColumnName("datsov")
                    .HasColumnType("date");

                entity.Property(e => e.Fustel)
                    .HasColumnName("fustel")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Gimaga)
                    .HasColumnName("gimaga")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Mamaga)
                    .HasColumnName("mamaga")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Materi)
                    .HasColumnName("materi")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Precod)
                    .HasColumnName("precod")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Profon)
                    .HasColumnName("profon")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Proord).HasColumnName("proord");

                entity.Property(e => e.Prosot)
                    .HasColumnName("prosot")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Protom)
                    .HasColumnName("protom")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Qtaord).HasColumnName("qtaord");

                entity.Property(e => e.Rigord).HasColumnName("rigord");

                entity.Property(e => e.Sitprf)
                    .HasColumnName("sitprf")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Sitprs)
                    .HasColumnName("sitprs")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Sitprt)
                    .HasColumnName("sitprt")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Stato)
                    .HasColumnName("stato")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Tipo)
                    .HasColumnName("tipo")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Tipord)
                    .HasColumnName("tipord")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Trmaga)
                    .HasColumnName("trmaga")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Varcol)
                    .HasColumnName("varcol")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Varian)
                    .HasColumnName("varian")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();
            });
            modelBuilder.Entity<Colori>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Colori");

                entity.Property(e => e.Descrizione)
                    .HasColumnName("descrizione")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Valore)
                    .IsRequired()
                    .HasColumnName("valore")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });
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
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Client");

                entity.Property(e => e.Codclf)
                    .IsRequired()
                    .HasColumnName("codclf")
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Ragsoc)
                    .IsRequired()
                    .HasColumnName("ragsoc")
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .IsFixedLength();
            });
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

            modelBuilder.Entity<Stagio>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("stagio");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
