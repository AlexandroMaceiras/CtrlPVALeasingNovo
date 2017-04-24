namespace CtrlPVALeasing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_DadosDaVenda
    {
        public int id { get; set; }

        [StringLength(20)]
        public string renavam { get; set; }

        [StringLength(20)]
        public string chassi { get; set; }

        [StringLength(9)]
        public string placa { get; set; }

        [StringLength(40)]
        public string nome_comprador { get; set; }

        [StringLength(18)]
        public string cpf_cnpj_comprador { get; set; }

        [StringLength(18)]
        public string rg_comprador { get; set; }

        [StringLength(80)]
        public string local_comprador { get; set; }

        [StringLength(80)]
        public string end_comprador { get; set; }

        public DateTime? dta_da_compra { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? valor_da_compra { get; set; }
    }
}
