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
    
    public partial class fornecedore
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public fornecedore()
        {
            this.materiais = new HashSet<materiai>();
            this.fornecedores_anexos = new HashSet<fornecedores_anexos>();
            this.fornecedores_emails = new HashSet<fornecedores_emails>();
            this.fornecedores_materiais = new HashSet<fornecedores_materiais>();
            this.fornecedores_regioes = new HashSet<fornecedores_regioes>();
            this.fornecedores_telefones = new HashSet<fornecedores_telefones>();
            this.negociacoes = new HashSet<negociaco>();
        }
        [Key]
        [Display(Name="C�digo")]
        public long id { get; set; }

        [Display(Name="Raz�o Social")]
        [Required(ErrorMessage="Raz�o Social deve ser informada")]
        public string c_razao_social { get; set; }
        [Display(Name="E-mail Principal")]
        [Required(ErrorMessage="E-mail Principal deve ser informado")]
        public string c_email_principal { get; set; }
        [Display(Name="C�digo Alterativo")]
        [Required(ErrorMessage="C�digo Alterantivo deve ser informado")]
        public string c_codigo { get; set; }
        [Display(Name="CNPJ")]
        [Required(ErrorMessage="CNPJ deve ser informado")]
        public string c_cnpj { get; set; }
        [Display(Name="Respons�vel")]
        [Required(ErrorMessage="Respons�vel deve ser informado")]
        public string c_responsavel { get; set; }
        [Display(Name="Operadora")]
        public long operadora { get; set; }
        [Display(Name="Situa��o")]
        public int f_situacao { get; set; }
        [Display(Name="Tipo de Fornecedor")]
        public int f_tipo_fornecedor { get; set; }
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
    
        public virtual operadora1 operadora1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<materiai> materiais { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fornecedores_anexos> fornecedores_anexos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fornecedores_emails> fornecedores_emails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fornecedores_materiais> fornecedores_materiais { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fornecedores_regioes> fornecedores_regioes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fornecedores_telefones> fornecedores_telefones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<negociaco> negociacoes { get; set; }
    }
    public class ListaFornecedor
    {
        public long id { get; set; }        
        public string c_razao_social { get; set; }       
        public string c_email_principal { get; set; }      
        public string c_codigo { get; set; }       
        public string c_cnpj { get; set; }      
        public string c_responsavel { get; set; }       
        public string operadora { get; set; }      
        public string f_situacao { get; set; }       
        public string f_tipo_fornecedor { get; set; }       
        public System.DateTime? sisdatai { get; set; }       
        public string sisusuarioi { get; set; }      
        public System.DateTime? sisdataa { get; set; }      
        public string sisusuarioa { get; set; }
    }
    public class vModelDetalheFornecedor
    {
        public virtual fornecedore vfornecedor { get; set; }
        public virtual fornecedores_anexos vfornecedores_anexos { get; set; }
        public virtual fornecedores_bancos vfornecedores_bancos { get; set; }
        public virtual fornecedores_emails vfornecedores_emails { get; set; }
        public virtual fornecedores_enderecos vfornecedores_enderecos { get; set; }
        public virtual fornecedores_materiais vfornecedores_materiais { get; set; }
        public virtual fornecedores_regioes vfornecedores_regioes { get; set; }
        public virtual fornecedores_telefones vfornecedores_telefones { get; set; }
    }
}
