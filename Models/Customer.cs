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
        [Required(ErrorMessage = "Please provide customer's name")]
        [StringLength(255)]
        public string Name { get; set; }
        public bool HasNewsletterSubscribed { get; set; }
        public MembershipType MembershipType { get; set; }
        [Display(Name = "Membership Type")]
        [Required(ErrorMessage = "Please provide membership type")]
        public byte MembershipTypeId { get; set; }
        [Display(Name = "Date of birth")]
        [Min18YearsIfMember]
        public DateTime? Birthdate { get; set; }

        public Customer()
        {

        }
    }
}
