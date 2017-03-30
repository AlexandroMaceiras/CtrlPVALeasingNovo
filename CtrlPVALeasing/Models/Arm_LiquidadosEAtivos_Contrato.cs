namespace CtrlPVALeasing.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Arm_LiquidadosEAtivos_Contrato
    {
        public int id { get; set; }

        [StringLength(9)]
        public string contrato { get; set; }

        [StringLength(4)]
        public string tipo { get; set; }

        [StringLength(5)]
        public string agencia { get; set; }

        [StringLength(8)]
        public string dta_inicio_contrato { get; set; }

        [StringLength(8)]
        public string dta_vecto_contrato { get; set; }

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

        [StringLength(8)]
        public string dta_ultimo_pagto { get; set; }

        [StringLength(1)]
        public string tipo_de_baixa { get; set; }

        [StringLength(8)]
        public string data_da_baixa { get; set; }

        [StringLength(2)]
        public string cod_empresa { get; set; }

        [StringLength(5)]
        public string num_end_cliente { get; set; }

        [StringLength(15)]
        public string comp_end_cliente { get; set; }
    }
}
