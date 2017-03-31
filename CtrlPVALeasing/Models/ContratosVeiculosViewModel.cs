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


    }
}