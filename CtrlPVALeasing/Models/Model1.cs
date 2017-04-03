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

        public virtual DbSet<Arm_AleTeste> Arm_AleTeste { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.contrato)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.tipo)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.agencia)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.origem)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.cpf_cnpj_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.nome_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.ddd_cliente_particular)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.fone_cliente_particular)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.rml_cliente_particular)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.end_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.bairro_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.cidade_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.uf_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.cep_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.filler)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.ddd_cliente_cml)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.fone_cliente_cml)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.tipo_de_baixa)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.cod_empresa)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.num_end_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.comp_end_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_AleTeste>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
