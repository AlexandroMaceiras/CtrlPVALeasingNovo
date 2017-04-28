namespace CtrlPVALeasing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_Bens
    {
        public int id { get; set; }

        [StringLength(20)]
        public string renavam { get; set; }

        [StringLength(18)]
        public string chassi { get; set; }

        [StringLength(9)]
        public string placa { get; set; }
    }
}
