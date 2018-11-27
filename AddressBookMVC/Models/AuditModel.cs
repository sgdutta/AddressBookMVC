using System;
using System.ComponentModel.DataAnnotations;

namespace AddressBookMVC.Models
{
    public class AuditModel
    {
        
        public Nullable<int> InfoId { get; set; }
        public string Updates { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        [Key]
        public int AuditId { get; set; }

    }
}