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
    
    public partial class marcas_operadoras
    {
        public long id { get; set; }
        public long marca { get; set; }
        public long operadora { get; set; }
        public Nullable<System.DateTime> sisdatai { get; set; }
        public Nullable<long> sisusuarioi { get; set; }
        public Nullable<System.DateTime> sisdataa { get; set; }
        public Nullable<long> sisusuarioa { get; set; }
        public Nullable<System.DateTime> sisdatae { get; set; }
        public Nullable<long> sisusuarioe { get; set; }
    
        public virtual marca marca1 { get; set; }
        public virtual operadora1 operadora1 { get; set; }
    }
}