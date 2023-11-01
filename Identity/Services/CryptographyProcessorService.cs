using RapidPayTest.Application.DTOs.Security;
using RapidPayTest.Application.Interfaces.Services.Security;
using System;
using System.Security.Cryptography;
using System.Text;

namespace RapidPayTest.Identity.Services
{
    public class CryptographyProcessorService : ICryptographyProcessorService
    {
        private byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }
        public PasswordAndSaltedStringInfo GetPasswordAndSecurityKeyInfo(string password)
        {
            string salt = String.Format("{0}*$#dsf354#587!@{1}ddfdsff435#$", DateTime.Now.ToString("ddMMYYss"), Guid.NewGuid().ToString());
            var PlainPassword = Encoding.UTF8.GetBytes(string.Format("a3dsfsdfdsf4{0}asd324esdf{0}fsdfsdfdss*9{1}", password, salt));
            var PlainAutoGenerateSalt = Encoding.UTF8.GetBytes(salt);

            var GenerateSaltedHash = this.GenerateSaltedHash(PlainPassword, PlainAutoGenerateSalt);

            return new PasswordAndSaltedStringInfo() { HashedPassword = Convert.ToBase64String(GenerateSaltedHash), SecurityKey = salt };
        }

        public bool PasswordsMatch(string password, string securityKey, string HashedPassword)
        {
            string salt = securityKey;
            var PlainPassword = Encoding.UTF8.GetBytes(string.Format("a3dsfsdfdsf4{0}asd324esdf{0}fsdfsdfdss*9{1}", password, salt));
            var PlainAutoGenerateSalt = Encoding.UTF8.GetBytes(salt);

            var GenerateSaltedHash = this.GenerateSaltedHash(PlainPassword, PlainAutoGenerateSalt);

            return (HashedPassword == Convert.ToBase64String(GenerateSaltedHash)) ? true : false;
        }

    }
}
