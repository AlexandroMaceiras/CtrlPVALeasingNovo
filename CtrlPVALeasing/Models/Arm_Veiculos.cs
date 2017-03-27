namespace CtrlPVALeasing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Arm_Veiculos
    {
        public int id { get; set; }

        [StringLength(9)]
        public string contrato { get; set; }

        [StringLength(4)]
        public string tipo_registro { get; set; }

        [StringLength(20)]
        public string marca { get; set; }

        [StringLength(20)]
        public string modelo { get; set; }

        [StringLength(20)]
        public string tipo { get; set; }

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
        public string origem { get; set; }
    }
}
