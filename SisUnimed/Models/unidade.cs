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
    
    public partial class unidade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public unidade()
        {
            this.materiais = new HashSet<materiai>();
        }
    
        public long id { get; set; }
        public string c_sigla { get; set; }
        public string c_descricao { get; set; }
        public Nullable<System.DateTime> sisdatai { get; set; }
        public Nullable<long> sisusuarioi { get; set; }
        public Nullable<System.DateTime> sisdataa { get; set; }
        public Nullable<long> sisusuarioa { get; set; }
        public Nullable<System.DateTime> sisdatae { get; set; }
        public Nullable<long> sisusuarioe { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<materiai> materiais { get; set; }
    }
}
