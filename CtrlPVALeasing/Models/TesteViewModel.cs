using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CtrlPVALeasing.Models
{
    public class TesteViewModel
    {
        public bool? status { get; set; }

        public int? total { get; set; }

        public bool? status2 { get; set; }

        public int? total2 { get; set; }
    }
}