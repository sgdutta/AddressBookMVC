using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AddressBookMVC.Models
{
    public class AddressModel
    {
        [Key]
        public int InfoId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> Zip { get; set; }
        public string emailaddress { get; set; }

        public virtual ICollection<AuditModel> Audits { get; set; }
    }
}