using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectServicePart.ModelDtos
{
    public class AuthenticationLoginCheckDto
    {
        public long PasswordHash { get; set; }

        public string PasswordSalt { get; set; }
    }
}
