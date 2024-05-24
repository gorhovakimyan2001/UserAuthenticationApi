using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectServicePart.ModelDtos
{
    public class AuthenticationRegisterDto
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordConfirm { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
