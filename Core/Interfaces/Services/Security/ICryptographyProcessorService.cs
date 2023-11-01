using RapidPayTest.Application.DTOs.Security;

namespace RapidPayTest.Application.Interfaces.Services.Security
{
    public interface ICryptographyProcessorService
    {
        public PasswordAndSaltedStringInfo GetPasswordAndSecurityKeyInfo(string password);

        bool PasswordsMatch(string password, string securityKey, string HashedPassword);
    }
}
