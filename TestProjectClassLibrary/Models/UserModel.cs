using System.Diagnostics.CodeAnalysis;

namespace TestProjectDbPart.Models
{
    [Serializable]
    public class UserModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        [NotNull]
        public string Email { get; set; }
    }
}
