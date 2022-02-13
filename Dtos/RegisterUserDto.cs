using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Dtos
{
    public class RegisterUserDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public int RoleId { get; set; } = 1;

        public string Name { get; set; }

        public byte MembershipTypeId { get; set; }

        public DateTime? Birthdate { get; set; }
    }
}
