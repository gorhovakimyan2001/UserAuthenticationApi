using System.ComponentModel.DataAnnotations;

namespace TestProjectServicePart.ModelDtos
{
    public class UserModelDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
