using System;
using System.Security.Cryptography;
using System.Text;

namespace SecurityService.Helpers
{
    public static class CryptoProvider
    {
        private const string SECRET = "CCECA25CA15F5B407B754F9AA3AF92FDDE2E7CBA4B95017E2CA7BAEA2B2F8D37";

        public static string GenerateSHMACHash(string base64Content)
        {
            if(string.IsNullOrWhiteSpace(base64Content))
            {
                throw new Exception("Invalid content");
            }

            HMACSHA256 encryptor = new HMACSHA256(Encoding.ASCII.GetBytes(SECRET.ToCharArray()));

            byte[] output = encryptor.ComputeHash(Encoding.ASCII.GetBytes(base64Content.ToCharArray()));

            var hash = Convert.ToBase64String(output).Replace('+', '_')
                .Replace("=", "")
                .Replace('/', '-');

            return hash;
        }
    }
}
