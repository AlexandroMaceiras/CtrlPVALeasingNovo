namespace CtrlPVALeasing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Impressao
    {
        public int id { get; set; }

        [StringLength(20)]
        public string renavam { get; set; }

        [StringLength(20)]
        public string chassi { get; set; }

        [StringLength(9)]
        public string placa { get; set; }

        [Required]
        [StringLength(1)]
        public string tipo_impressao { get; set; }
    }
}
