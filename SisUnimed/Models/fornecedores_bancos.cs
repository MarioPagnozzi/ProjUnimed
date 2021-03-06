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
    using System.ComponentModel.DataAnnotations;
    
    public partial class fornecedores_bancos
    {
        [Key]
        [Display(Name="C�digo")]
        public long id { get; set; }
        [Display(Name="Banco")]
        public long banco { get; set; }
        [Display(Name="Ag�ncia")]
        [Required(ErrorMessage="Ag�ncia deve ser informada")]
        public string c_agencia { get; set; }
        [Display(Name="Conta Corrente")]
        [Required(ErrorMessage="Conta Corrente deve ser informada")]
        public string c_conta { get; set; }
        [Display(Name="Titular")]
        [Required(ErrorMessage="Titular deve ser informada")]
        public string c_titular { get; set; }
        [Display(Name="Cidade")]
        [Required(ErrorMessage="Cidade deve ser informada")]
        public string c_cidade { get; set; }
        [Display(Name="UF")]
        public Nullable<long> estado { get; set; }
        [Display(Name="Data Inclus�o")]
        public Nullable<System.DateTime> sisdatai { get; set; }
        [Display(Name="Usu�rio Inclus�o")]
        public Nullable<long> sisusuarioi { get; set; }
        [Display(Name="Data Altera��o")]
        public Nullable<System.DateTime> sisdataa { get; set; }
        [Display(Name="Usu�rio Altera��o")]
        public Nullable<long> sisusuarioa { get; set; }
        public Nullable<System.DateTime> sisdatae { get; set; }
        public Nullable<long> sisusuarioe { get; set; }
        public long fornecedor { get; set; }
    
        public virtual banco banco1 { get; set; }
        public virtual estado estado1 { get; set; }
        public virtual fornecedore fornecedore { get; set; }
    }
    public class ListaFornecedorBanco
    {
        public long id { get; set; }
        public long fornecedor { get; set; }
        public string banco { get; set; }
        public string c_agencia { get; set; }
        public string c_conta { get; set; }
        public string c_titular { get; set; }
        public string c_cidade { get; set; }
        public string estado { get; set; }
        public System.DateTime? sisdatai { get; set; }
        public string sisusuarioi { get; set; }
        public System.DateTime? sisdataa { get; set; }
        public string sisusuarioa { get; set; }
    }
}
