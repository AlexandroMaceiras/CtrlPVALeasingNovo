namespace CtrlPVALeasing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_DebitosEPagamentos_Veiculo
    {
        public int id { get; set; }

        [StringLength(20)]
        public string chassi { get; set; }

        [StringLength(20)]
        public string renavam { get; set; }

        [StringLength(9)]
        public string placa { get; set; }

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

        public DateTime? dta_pagamento_divida { get; set; }

        public DateTime? dta_recuperacao { get; set; }

        [StringLength(15)]
        public string pci_debito_divida { get; set; }

        [StringLength(15)]
        public string pci_debito_custa { get; set; }

        [StringLength(15)]
        public string pci_credito { get; set; }

        [StringLength(6)]
        public string grupo_safra { get; set; }

        public DateTime? dta_custas { get; set; }

        [StringLength(13)]
        public string status_recuperacao { get; set; }

        public DateTime? dta_contabil_divida { get; set; }

        public DateTime? dta_contabil_custa { get; set; }

    }
}
