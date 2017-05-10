namespace CtrlPVALeasing.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CtrlIPVALeasingContext : DbContext
    {
        public CtrlIPVALeasingContext()
            : base("name=CTRL_IPVA_LEASING_REAL")
        {
        }

        public virtual DbSet<Arm_LiquidadosEAtivos_Contrato> Arm_LiquidadosEAtivos_Contrato { get; set; }
        public virtual DbSet<Arm_Veiculos> Arm_Veiculos { get; set; }
        public DbSet<Tbl_DebitosEPagamentos_Veiculo> Tbl_DebitosEPagamentos_Veiculo { get; set; }
        public DbSet<CtrlPVALeasing.Models.Tbl_DadosDaVenda> Tbl_DadosDaVenda { get; set; }
        public DbSet<CtrlPVALeasing.Models.Tbl_Bens> Tbl_Bens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.contrato)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.tipo)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.agencia)
                .IsUnicode(false);

            //modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
            //    .Property(e => e.dta_inicio_contrato)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
            //    .Property(e => e.dta_vecto_contrato)
            //    .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.origem)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.cpf_cnpj_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.nome_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.ddd_cliente_particular)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.fone_cliente_particular)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.rml_cliente_particular)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.end_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.bairro_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.cidade_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.uf_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.cep_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.filler)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.ddd_cliente_cml)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.fone_cliente_cml)
                .IsUnicode(false);

            //modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
            //    .Property(e => e.dta_ultimo_pagto)
            //    .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.tipo_de_baixa)
                .IsUnicode(false);

            //modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
            //    .Property(e => e.data_da_baixa)
            //    .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.cod_empresa)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.num_end_cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_LiquidadosEAtivos_Contrato>()
                .Property(e => e.comp_end_cliente)
                .IsUnicode(false);





            modelBuilder.Entity<Arm_Veiculos>()
                .Property(e => e.contrato)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_Veiculos>()
                .Property(e => e.tipo_registro)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_Veiculos>()
                .Property(e => e.marca)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_Veiculos>()
                .Property(e => e.modelo)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_Veiculos>()
                .Property(e => e.tipo)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_Veiculos>()
                .Property(e => e.ano_fab)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_Veiculos>()
                .Property(e => e.ano_mod)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_Veiculos>()
                .Property(e => e.cor)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_Veiculos>()
                .Property(e => e.renavam)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_Veiculos>()
                .Property(e => e.chassi)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_Veiculos>()
                .Property(e => e.placa)
                .IsUnicode(false);

            modelBuilder.Entity<Arm_Veiculos>()
                .Property(e => e.origem)
                .IsUnicode(false);



            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.chassi)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.renavam)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.placa)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.uf_cobranca)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.tipo_cobranca)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.ano_exercicio)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.cda)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.nome_cartorio)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.uf_pagamento)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.forma_pagamento_divida)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.forma_pagamento_custas)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.numero_miro_divida)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.numero_miro_custa)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.obs_pagamento)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.pci_credito)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.pci_debito_custa)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_DebitosEPagamentos_Veiculo>()
                .Property(e => e.pci_debito_divida)
                .IsUnicode(false);
        }

        public System.Data.Entity.DbSet<CtrlPVALeasing.Models.Tbl_Dut> Tbl_Dut { get; set; }

        public System.Data.Entity.DbSet<CtrlPVALeasing.Models.Tbl_CCL> Tbl_CCL { get; set; }

        public System.Data.Entity.DbSet<CtrlPVALeasing.Models.Tbl_SCC> Tbl_SCC { get; set; }
    }
}
