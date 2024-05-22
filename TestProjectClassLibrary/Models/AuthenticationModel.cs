using System.Diagnostics.CodeAnalysis;

namespace TestProjectDbPart.Models
{
    public class AuthenticationModel
    {
        [NotNull]
        public long PasswordHash { get; set; }

        [NotNull]
        public string PasswordSalt { get; set; }

        [NotNull]
        public string Email { get; set; }
    }
}
