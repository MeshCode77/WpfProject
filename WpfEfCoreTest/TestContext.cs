using Microsoft.EntityFrameworkCore;
using WpfEfCoreTest.Model;

namespace WpfEfCoreTest
{
    public class TestContext : DbContext
    {
        //public TestContext(DbContextOptions<TestContext> options)
        //    : base(options)
        //{
        //}

        public virtual DbSet<Dgm> Dgms { get; set; }
        public virtual DbSet<F111> F111s { get; set; }
        public virtual DbSet<Formular> Formulars { get; set; }
        public virtual DbSet<Info> Infos { get; set; }
        public virtual DbSet<Komplect> Komplects { get; set; }
        public virtual DbSet<NameOborud> NameOboruds { get; set; }
        public virtual DbSet<OtchetFormular> OtchetFormulars { get; set; }
        public virtual DbSet<OtchetOborud> OtchetOboruds { get; set; }
        public virtual DbSet<OtchetRemont> OtchetRemonts { get; set; }

        public virtual DbSet<Podr> Podrs { get; set; }

        //public virtual DbSet<Remont> Remonts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Sklad> Sklads { get; set; }
        public virtual DbSet<Spisanie> Spisanies { get; set; }
        public virtual DbSet<SprDgm> SprDgms { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSy> UserSys { get; set; }
        public virtual DbSet<VSelectF111toNameOborud> VSelectF111toNameOboruds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(
                    "Data Source=DESKTOP-E4A1ROU;Initial Catalog=Test; User Id = sa; Password = 123456");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Dgm>(entity =>
            {
                entity.ToTable("dgm");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Gold)
                    .HasMaxLength(10)
                    .HasColumnName("gold");

                entity.Property(e => e.Idformular).HasColumnName("idformular");

                entity.Property(e => e.Mpg)
                    .HasMaxLength(10)
                    .HasColumnName("mpg");

                entity.Property(e => e.NumForm)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("numForm");

                entity.Property(e => e.Silver)
                    .HasMaxLength(10)
                    .HasColumnName("silver");

                entity.HasOne(d => d.IdformularNavigation)
                    .WithMany(p => p.Dgms)
                    .HasForeignKey(d => d.Idformular)
                    .HasConstraintName("FK_dgm_formular");
            });

            modelBuilder.Entity<F111>(entity =>
            {
                entity.ToTable("f111");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GtDate).HasColumnType("date");

                entity.Property(e => e.Podr).HasColumnName("Podr");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.IdnameOborud).HasColumnName("idnameOborud");

                entity.Property(e => e.InvNum)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.KartNum)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("model");

                entity.Property(e => e.NumForm)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("numForm");

                entity.Property(e => e.OutDate).HasColumnType("date");

                entity.Property(e => e.ZavodNum)
                    .HasMaxLength(50)
                    .HasColumnName("zavodNum");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.F111s)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_f111_users");

                entity.HasOne(d => d.IdnameOborudNavigation)
                    .WithMany(p => p.F111s)
                    .HasForeignKey(d => d.IdnameOborud)
                    .HasConstraintName("FK_f111_nameOborud");
            });

            modelBuilder.Entity<Formular>(entity =>
            {
                entity.ToTable("formular");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Count)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("count");

                entity.Property(e => e.DataTo)
                    .HasColumnType("date")
                    .HasColumnName("DataTO");

                entity.Property(e => e.DateIn).HasColumnType("date");

                entity.Property(e => e.DateOut).HasColumnType("date");

                entity.Property(e => e.GarantyTo)
                    .HasMaxLength(50)
                    .HasColumnName("garantyTo");

                entity.Property(e => e.IdKomplect).HasColumnName("idKomplect");

                entity.Property(e => e.Idf111).HasColumnName("idf111");

                entity.Property(e => e.InvNum)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Model)
                    .HasMaxLength(20)
                    .HasColumnName("model");

                entity.Property(e => e.NumAkt)
                    .HasMaxLength(50)
                    .HasColumnName("numAkt");

                entity.Property(e => e.NumForm)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("numForm");

                entity.Property(e => e.Serial)
                    .HasMaxLength(30)
                    .HasColumnName("serial");

                entity.Property(e => e.YearProd)
                    .HasMaxLength(50)
                    .HasColumnName("yearProd");

                entity.HasOne(d => d.IdKomplectNavigation)
                    .WithMany(p => p.Formulars)
                    .HasForeignKey(d => d.IdKomplect)
                    .HasConstraintName("FK_formular_Komplect");

                entity.HasOne(d => d.Idf111Navigation)
                    .WithMany(p => p.Formulars)
                    .HasForeignKey(d => d.Idf111)
                    .HasConstraintName("FK_formular_f111");
            });

            modelBuilder.Entity<Info>(entity =>
            {
                entity.ToTable("info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Doljnost)
                    .HasMaxLength(50)
                    .HasColumnName("doljnost");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Ip)
                    .HasMaxLength(50)
                    .HasColumnName("ip");

                entity.Property(e => e.Login)
                    .HasMaxLength(50)
                    .HasColumnName("login");

                entity.Property(e => e.Mac)
                    .HasMaxLength(50)
                    .HasColumnName("mac");

                entity.Property(e => e.NameComp).HasMaxLength(50);

                entity.Property(e => e.Pass)
                    .HasMaxLength(50)
                    .HasColumnName("pass");

                entity.Property(e => e.Vtel)
                    .HasMaxLength(30)
                    .HasColumnName("vtel");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Infos)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_info_users");
            });

            modelBuilder.Entity<Komplect>(entity =>
            {
                entity.ToTable("Komplect");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NameKompl)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<NameOborud>(entity =>
            {
                entity.ToTable("nameOborud");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NameOborud1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nameOborud");
            });

            modelBuilder.Entity<OtchetFormular>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("otchetFormular");

                entity.Property(e => e.Count)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("count");

                entity.Property(e => e.DataTo)
                    .HasColumnType("date")
                    .HasColumnName("DataTO");

                entity.Property(e => e.DateIn).HasColumnType("date");

                entity.Property(e => e.DateOut).HasColumnType("date");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.IdKomplect)
                    .HasMaxLength(50)
                    .HasColumnName("idKomplect");

                entity.Property(e => e.Idf111).HasColumnName("idf111");

                entity.Property(e => e.InvNum)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Model)
                    .HasMaxLength(20)
                    .HasColumnName("model");

                entity.Property(e => e.NumForm)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("numForm");

                entity.Property(e => e.Serial)
                    .HasMaxLength(30)
                    .HasColumnName("serial");
            });

            modelBuilder.Entity<OtchetOborud>(entity =>
            {
                entity.ToTable("otchetOborud");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.InvNum)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.NameOborud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nameOborud");

                entity.Property(e => e.NumForm)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("numForm");

                entity.Property(e => e.ZavodNum)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("zavodNum");
            });

            modelBuilder.Entity<OtchetRemont>(entity =>
            {
                entity.ToTable("OtchetRemont");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BeginDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Idf111).HasColumnName("idf111");

                entity.Property(e => e.NameOborud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nameOborud");

                entity.Property(e => e.NumForm)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("numForm");

                entity.Property(e => e.Podr)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("podr");

                entity.Property(e => e.Title).HasMaxLength(210);

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("user");

                entity.HasOne(d => d.Idf111Navigation)
                    .WithMany(p => p.OtchetRemonts)
                    .HasForeignKey(d => d.Idf111)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OtchetRemont_f111");
            });

            modelBuilder.Entity<Podr>(entity =>
            {
                entity.ToTable("Podr");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NamePodr)
                    .IsRequired()
                    .HasMaxLength(50);
            });


            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Role1)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("Role");
            });

            modelBuilder.Entity<Sklad>(entity =>
            {
                entity.ToTable("sklad");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateSpisania).HasColumnType("date");

                entity.Property(e => e.DateToSklad).HasColumnType("date");

                entity.Property(e => e.Idformular).HasColumnName("idformular");

                entity.Property(e => e.InvNum)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NumForm)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Text).HasMaxLength(250);
            });

            modelBuilder.Entity<Spisanie>(entity =>
            {
                entity.ToTable("Spisanie");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateSpisan).HasColumnType("date");

                entity.Property(e => e.Idformular).HasColumnName("idformular");

                entity.Property(e => e.InvNum)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NumAktTechSost)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NumForm)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("numForm");

                entity.Property(e => e.Text).HasMaxLength(150);

                entity.Property(e => e.ZavodNum)
                    .HasMaxLength(50)
                    .HasColumnName("zavodNum");

                entity.HasOne(d => d.IdformularNavigation)
                    .WithMany(p => p.Spisanies)
                    .HasForeignKey(d => d.Idformular)
                    .HasConstraintName("FK_Spisanie_formular");
            });

            modelBuilder.Entity<SprDgm>(entity =>
            {
                entity.ToTable("SprDgm");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Gold)
                    .HasMaxLength(10)
                    .HasColumnName("gold");

                entity.Property(e => e.IdNameOborudKomplekt).HasColumnName("idNameOborudKomplekt");

                entity.Property(e => e.Mpg)
                    .HasMaxLength(10)
                    .HasColumnName("mpg");

                entity.Property(e => e.NameOborud).HasMaxLength(50);

                entity.Property(e => e.Silver)
                    .HasMaxLength(10)
                    .HasColumnName("silver");

                entity.HasOne(d => d.IdNameOborudKomplektNavigation)
                    .WithMany(p => p.SprDgms)
                    .HasForeignKey(d => d.IdNameOborudKomplekt)
                    .HasConstraintName("FK_SprDgm_Komplect");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Fname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("FName");

                entity.Property(e => e.IdPodr).HasColumnName("idPodr");

                entity.Property(e => e.Lname)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Mname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("MName");

                entity.HasOne(d => d.IdPodrNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdPodr)
                    .HasConstraintName("FK_users_Podr");
            });

            modelBuilder.Entity<UserSy>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Fname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("FName");

                //entity.Property(e => e.IdRole).HasColumnName("idRole");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("login");

                entity.Property(e => e.Pass)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("pass");

                //entity.HasOne(d => d.IdRoleNavigation)
                //    .WithMany(p => p.UserSies)
                //    .HasForeignKey(d => d.IdRole)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_UserSys_Role");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UserSies)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_UserSys_users");
            });

            modelBuilder.Entity<VSelectF111toNameOborud>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vSelectF111ToNameOborud");

                entity.Property(e => e.GtDate).HasColumnType("date");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.IdnameOborud).HasColumnName("idnameOborud");

                entity.Property(e => e.InvNum)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.KartNum)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NameOborud)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nameOborud");

                entity.Property(e => e.NumForm)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("numForm");

                entity.Property(e => e.OutDate).HasColumnType("date");
            });

            //OnModelCreatingPartial(modelBuilder);
        }
    }
}