namespace CtrlPVALeasing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Arm_LiquidadosEAtivos_Contrato",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        contrato = c.String(maxLength: 9, unicode: false),
                        tipo = c.String(maxLength: 4, unicode: false),
                        agencia = c.String(maxLength: 5, unicode: false),
                        dta_inicio_contrato = c.DateTime(storeType: "date"),
                        dta_vecto_contrato = c.DateTime(storeType: "date"),
                        origem = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        cpf_cnpj_cliente = c.String(maxLength: 18, unicode: false),
                        nome_cliente = c.String(maxLength: 40, unicode: false),
                        ddd_cliente_particular = c.String(maxLength: 4, unicode: false),
                        fone_cliente_particular = c.String(maxLength: 8, unicode: false),
                        rml_cliente_particular = c.String(maxLength: 4, unicode: false),
                        end_cliente = c.String(maxLength: 30, unicode: false),
                        bairro_cliente = c.String(maxLength: 20, unicode: false),
                        cidade_cliente = c.String(maxLength: 30, unicode: false),
                        uf_cliente = c.String(maxLength: 2, unicode: false),
                        cep_cliente = c.String(maxLength: 8, unicode: false),
                        filler = c.String(maxLength: 1, unicode: false),
                        ddd_cliente_cml = c.String(maxLength: 4, unicode: false),
                        fone_cliente_cml = c.String(maxLength: 8, unicode: false),
                        dta_ultimo_pagto = c.DateTime(storeType: "date"),
                        tipo_de_baixa = c.String(maxLength: 1, unicode: false),
                        data_da_baixa = c.DateTime(storeType: "date"),
                        cod_empresa = c.String(maxLength: 2, unicode: false),
                        num_end_cliente = c.String(maxLength: 5, unicode: false),
                        comp_end_cliente = c.String(maxLength: 15, unicode: false),
                        status = c.Boolean(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Arm_Veiculos",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        contrato = c.String(maxLength: 9, unicode: false),
                        tipo_registro = c.String(maxLength: 4, unicode: false),
                        marca = c.String(maxLength: 20, unicode: false),
                        modelo = c.String(maxLength: 20, unicode: false),
                        tipo = c.String(maxLength: 20, unicode: false),
                        ano_fab = c.String(maxLength: 4, unicode: false),
                        ano_mod = c.String(maxLength: 4, unicode: false),
                        cor = c.String(maxLength: 10, unicode: false),
                        renavam = c.String(maxLength: 20, unicode: false),
                        chassi = c.String(maxLength: 20, unicode: false),
                        placa = c.String(maxLength: 9, unicode: false),
                        origem = c.String(maxLength: 92, unicode: false),
                        status = c.Boolean(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Tbl_DebitosEPagamentos_Veiculo",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        chassi = c.String(maxLength: 20, unicode: false),
                        renavam = c.String(maxLength: 20, unicode: false),
                        placa = c.String(maxLength: 9, unicode: false),
                        dta_cobranca = c.DateTime(),
                        uf_cobranca = c.String(maxLength: 2, unicode: false),
                        tipo_cobranca = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        valor_divida = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        ano_exercicio = c.String(maxLength: 4, unicode: false),
                        cda = c.String(maxLength: 10, unicode: false),
                        valor_custas = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        debito_protesto = c.Boolean(),
                        nome_cartotio = c.String(maxLength: 40, unicode: false),
                        divida_ativa_serasa = c.Boolean(),
                        protesto_serasa = c.Boolean(),
                        valor_debito_total = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        pagamento_efet_banco = c.Boolean(),
                        dta_pagamento = c.DateTime(),
                        uf_pagamento = c.String(maxLength: 2, unicode: false),
                        tipo_pagamento = c.String(maxLength: 10, unicode: false),
                        valor_pago_divida = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        numero_miro = c.String(maxLength: 10, unicode: false),
                        obs_pagamento = c.String(maxLength: 250, unicode: false),
                        valor_pago_custas = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        valor_pago_total = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        valor_recuperado = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                        valor_total_recuperado = c.Decimal(precision: 18, scale: 2, storeType: "numeric"),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_DebitosEPagamentos_Veiculo");
            DropTable("dbo.Arm_Veiculos");
            DropTable("dbo.Arm_LiquidadosEAtivos_Contrato");
        }
    }
}
