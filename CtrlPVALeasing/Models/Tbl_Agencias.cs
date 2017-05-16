namespace CtrlPVALeasing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Agencias
    {
        public int id { get; set; }

        [StringLength(5)]
        public string agencia { get; set; }

        [StringLength(40)]
        public string descricao_agencia { get; set; }
    }
}
