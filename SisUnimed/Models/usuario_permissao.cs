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
    
    public partial class usuario_permissao
    {
        
        [Display(Name = "C�digo")]
        [Required(ErrorMessage="Informe o c�digo")]
        public int id { get; set; }
        [Key]
        [Display(Name = "Usu�rio")]
        [Required(ErrorMessage="Informe o usu�rio")]
        public int id_usuario { get; set; }
        [Display(Name = "Operadora")]
        [Required(ErrorMessage="Informe Sim ou N�o")]
        public int operadora { get; set; }
        [Display(Name = "Inserir Operadora")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int operadora_i { get; set; }
        [Display(Name = "Alterar Operadora")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int operadora_a { get; set; }
        [Display(Name = "Excluir Operadora")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int operadora_d { get; set; }
        [Display(Name = "Grupo")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int grupo { get; set; }
        [Display(Name = "Incluir Grupo")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int grupo_i { get; set; }
        [Display(Name = "Alterar Grupo")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int grupo_a { get; set; }
        [Display(Name = "Excluir Grupo")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int grupo_d { get; set; }
        [Display(Name = "Permiss�o de Grupo")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int grupo_permissao { get; set; }
        [Display(Name = "Inserir Permissao de Grupo")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int grupo_permissao_i { get; set; }
        [Display(Name = "Alterar Permiss�o de Grupo")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int grupo_permissao_a { get; set; }
        [Display(Name = "Excluir Permiss�o de Grupo")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int grupo_permissao_d { get; set; }
        [Display(Name = "Usu�rio")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int usuario { get; set; }
        [Display(Name = "Incluir Usu�rio")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int usuario_i { get; set; }
        [Display(Name = "Alterar Usu�rio")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int usuario_a { get; set; }
        [Display(Name = "Excluir Usu�rio")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int usuario_d { get; set; }
        [Display(Name = "Permiss�o de Usu�rio")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int usuario_permissao1 { get; set; }
        [Display(Name = "Incluir Permiss�o de Usu�rio")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int usuario_permissao_i { get; set; }
        [Display(Name = "Alterar Permiss�o de Usu�rio")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int usuario_permissao_a { get; set; }
        [Display(Name = "Excluir Permiss�o de Usu�rio")]
        [Required(ErrorMessage = "Informe Sim ou N�o")]
        public int usuario_permissao_d { get; set; }
        [Display(Name = "Usu�rio")]        
        public virtual usuario usuario1 { get; set; }
    }
}