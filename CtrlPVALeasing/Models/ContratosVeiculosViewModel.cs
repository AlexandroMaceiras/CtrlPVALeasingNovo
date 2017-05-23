using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CtrlPVALeasing.Models
{
    public class ContratosVeiculosViewModel
    {
        public int id { get; set; }

        [StringLength(9)]
        public string contrato { get; set; }

        [StringLength(4)]
        public string tipo { get; set; }

        [StringLength(5)]
        public string agencia { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime? dta_inicio_contrato { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime? dta_vecto_contrato { get; set; }

        [StringLength(1)]
        public string origem { get; set; }

        [StringLength(18)]
        public string cpf_cnpj_cliente { get; set; }

        [StringLength(40)]
        public string nome_cliente { get; set; }

        [StringLength(4)]
        public string ddd_cliente_particular { get; set; }

        [StringLength(8)]
        public string fone_cliente_particular { get; set; }

        [StringLength(4)]
        public string rml_cliente_particular { get; set; }

        [StringLength(30)]
        public string end_cliente { get; set; }

        [StringLength(20)]
        public string bairro_cliente { get; set; }

        [StringLength(30)]
        public string cidade_cliente { get; set; }

        [StringLength(2)]
        public string uf_cliente { get; set; }

        [StringLength(8)]
        public string cep_cliente { get; set; }

        [StringLength(1)]
        public string filler { get; set; }

        [StringLength(4)]
        public string ddd_cliente_cml { get; set; }

        [StringLength(8)]
        public string fone_cliente_cml { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime? dta_ultimo_pagto { get; set; }

        [StringLength(1)]
        public string tipo_de_baixa { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime? data_da_baixa { get; set; }

        [StringLength(2)]
        public string cod_empresa { get; set; }

        [StringLength(5)]
        public string num_end_cliente { get; set; }

        [StringLength(15)]
        public string comp_end_cliente { get; set; }

        public bool? status { get; set; }



        //public int id { get; set; }

        [StringLength(9)]
        public string contrato_v { get; set; }

        [StringLength(4)]
        public string tipo_registro { get; set; }

        [StringLength(20)]
        public string marca { get; set; }

        [StringLength(20)]
        public string modelo { get; set; }

        [StringLength(20)]
        public string tipo_v { get; set; }

        [StringLength(4)]
        public string ano_fab { get; set; }

        [StringLength(4)]
        public string ano_mod { get; set; }

        [StringLength(10)]
        public string cor { get; set; }

        [StringLength(20)]
        public string renavam { get; set; }

        [StringLength(20)]
        public string chassi { get; set; }

        [StringLength(9)]
        public string placa { get; set; }

        [StringLength(92)]
        public string origem_v { get; set; }

        public bool? status_v { get; set; }
        public bool? comunicado_venda { get; set; }



        public int id_debito { get; set; }

        public DateTime? dta_cobranca { get; set; }

        [StringLength(2)]
        public string uf_cobranca { get; set; }

        [StringLength(1)]
        public string tipo_cobranca { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? valor_divida { get; set; }

        [StringLength(4)]
        public string ano_exercicio { get; set; }

        [StringLength(10)]
        public string cda { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? valor_custas { get; set; }

        public bool? debito_protesto { get; set; }

        [StringLength(40)]
        public string nome_cartorio { get; set; }

        public bool? divida_ativa_serasa { get; set; }

        public bool? protesto_serasa { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? valor_debito_total { get; set; }

        public bool? pagamento_efet_banco { get; set; }

        public DateTime? dta_pagamento { get; set; }

        [StringLength(2)]
        public string uf_pagamento { get; set; }

        [StringLength(10)]
        public string forma_pagamento_divida { get; set; }

        [StringLength(10)]
        public string forma_pagamento_custas { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? valor_pago_divida { get; set; }

        [StringLength(10)]
        public string numero_miro_divida { get; set; }

        [StringLength(10)]
        public string numero_miro_custa { get; set; }

        [StringLength(10)]
        public string numero_miro { get; set; }

        [StringLength(250)]
        public string obs_pagamento { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? valor_pago_custas { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? valor_pago_total { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? valor_recuperado { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? valor_total_recuperado { get; set; }

        public DateTime? dta_pagamento_custas { get; set; }

        public DateTime? dta_recuperacao { get; set; }

        [StringLength(15)]
        public string pci_debito_divida { get; set; }

        [StringLength(15)]
        public string pci_debito_custa { get; set; }

        [StringLength(15)]
        public string pci_credito { get; set; }

        [StringLength(6)]
        public string grupo_safra { get; set; }







        public int? id_comprador { get; set; }

        [StringLength(40)]
        public string nome_comprador { get; set; }

        [StringLength(18)]
        public string cpf_cnpj_comprador { get; set; }

        [StringLength(11)]
        public string rg_comprador { get; set; }

        [StringLength(40)]
        public string local_comprador { get; set; }

        [StringLength(80)]
        public string end_comprador { get; set; }

        public DateTime? dta_da_compra { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? valor_da_compra { get; set; }




        [StringLength(20)]
        public string renavam_bens { get; set; }

        [StringLength(20)]
        public string chassi_bens { get; set; }

        [StringLength(9)]
        public string placa_bens { get; set; }




        
        public bool? perm_debito { get; set; }

        [StringLength(1)]
        public string sinal { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? saldo { get; set; }

        [StringLength(10)]
        public string conta { get; set; }





        [StringLength(40)]
        public string descricao_agencia { get; set; }




        [StringLength(1)]
        public string tipo_impressao { get; set; }
        [StringLength(20)]
        public string renavam_dut { get; set; }

        [StringLength(20)]
        public string chassi_dut { get; set; }

        [StringLength(9)]
        public string placa_dut { get; set; }


        public TimeSpan diferenca { get; set; }


        public string listaSelecionados { get; set; }
    }
}