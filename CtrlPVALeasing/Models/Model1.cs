namespace CtrlPVALeasing.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Arm__Veiculos> Arm__Veiculos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arm__Veiculos>()
                .Property(e => e.contrato)
                .IsUnicode(false);

            modelBuilder.Entity<Arm__Veiculos>()
                .Property(e => e.tipo_registro)
                .IsUnicode(false);

            modelBuilder.Entity<Arm__Veiculos>()
                .Property(e => e.marca)
                .IsUnicode(false);

            modelBuilder.Entity<Arm__Veiculos>()
                .Property(e => e.modelo)
                .IsUnicode(false);

            modelBuilder.Entity<Arm__Veiculos>()
                .Property(e => e.tipo)
                .IsUnicode(false);

            modelBuilder.Entity<Arm__Veiculos>()
                .Property(e => e.ano_fab)
                .IsUnicode(false);

            modelBuilder.Entity<Arm__Veiculos>()
                .Property(e => e.ano_mod)
                .IsUnicode(false);

            modelBuilder.Entity<Arm__Veiculos>()
                .Property(e => e.cor)
                .IsUnicode(false);

            modelBuilder.Entity<Arm__Veiculos>()
                .Property(e => e.renavam)
                .IsUnicode(false);

            modelBuilder.Entity<Arm__Veiculos>()
                .Property(e => e.chassi)
                .IsUnicode(false);

            modelBuilder.Entity<Arm__Veiculos>()
                .Property(e => e.placa)
                .IsUnicode(false);

            modelBuilder.Entity<Arm__Veiculos>()
                .Property(e => e.origem)
                .IsUnicode(false);
        }
    }
}
