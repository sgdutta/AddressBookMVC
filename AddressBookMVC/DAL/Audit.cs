//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AddressBookMVC.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Audit
    {
        public Nullable<int> InfoId { get; set; }
        public string Updates { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public int AuditId { get; set; }
    
        public virtual AddressDetail AddressDetail { get; set; }
    }
}
