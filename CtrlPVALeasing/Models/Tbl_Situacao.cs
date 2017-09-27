namespace CtrlPVALeasing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Situacao
    {
        public int id { get; set; }

        [StringLength(18)]
        public string cpf_cnpj_cliente { get; set; }

        public bool? ativa { get; set; }
        public bool? localizada { get; set; }
        public bool? outrasOper { get; set; }
    }
}
