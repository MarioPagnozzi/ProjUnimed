//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SisUnimed.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class importacao_xls
    {
        public long id { get; set; }
        public System.DateTime d_importacao { get; set; }
        public string c_numero { get; set; }
        public string c_fabricante { get; set; }
        public string c_fornecedor_cadastrado { get; set; }
        public string c_cnpj_forncedor { get; set; }
        public string c_tnumm { get; set; }
        public string c_dv2 { get; set; }
        public string c_codigo_tuss { get; set; }
        public string c_cod_ref_fabr { get; set; }
        public string c_nome_comercial { get; set; }
        public string c_descricao_tecnica { get; set; }
        public string c_descricao_generica { get; set; }
        public string c_preco { get; set; }
        public string c_vigencia { get; set; }
        public string c_rms { get; set; }
        public string c_validade { get; set; }
        public string c_classificacao { get; set; }
        public string c_especialidade { get; set; }
        public string c_origem { get; set; }
        public string c_observacao { get; set; }
        public Nullable<System.DateTime> sisdatai { get; set; }
        public Nullable<long> sisusuarioi { get; set; }
        public Nullable<System.DateTime> sisdataa { get; set; }
        public Nullable<long> sisusuarioa { get; set; }
        public Nullable<System.DateTime> sisdatae { get; set; }
        public Nullable<long> sisusuarioe { get; set; }
    }
}