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
    
    public partial class usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public usuario()
        {
            this.usuario_permissao = new HashSet<usuario_permissao>();
        }
    
        public int id { get; set; }
        public int id_operadora { get; set; }
        public int id_grupo { get; set; }
        public string nome_usuario { get; set; }
        public string email_usuario { get; set; }
        public string senha_usuario { get; set; }
    
        public virtual grupo grupo { get; set; }
        public virtual operadora operadora { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<usuario_permissao> usuario_permissao { get; set; }
    }
}
