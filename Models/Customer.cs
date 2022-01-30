using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasNewsletterSubscribed { get; set; }
        public MembershipType MembershipType { get; set; }
        public byte MembershipTypeId { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime? Birthdate { get; set; }

        public Customer()
        {

        }
    }
}
