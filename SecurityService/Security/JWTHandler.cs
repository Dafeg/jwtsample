using SecurityService.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SecurityService.Security
{

    public class JWTHandler : ITokenHandler<JwtSecurityToken>
    {
        private const string  ALGORITHM = "HS256";

        public JwtSecurityToken GenerateToken(List<Claim> parametrs)
        {
            if(parametrs == null)
            {
                throw new Exception("Token payload can not be null");
            }

            var header = new JwtHeader();
            var payload = new JwtPayload("CHNU", string.Empty, parametrs, DateTime.Now, DateTime.Now.AddDays(7));
            header["alg"] = ALGORITHM;
            var encHeader = header.Base64UrlEncode();
            var encPayload = payload.Base64UrlEncode();


            var token = new JwtSecurityToken(header, payload, encHeader,encPayload, 
                CryptoProvider.GenerateSHMACHash($"{encHeader}.{encPayload}"));

            return token;
        }

        public JwtSecurityToken VerifyToken(string inputToken)
        {
            if(string.IsNullOrWhiteSpace(inputToken))
            {
                throw new Exception("Token string can not be null.");
            }

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(inputToken))
            {
                return null;
            }

            var tokenToVerify = handler.ReadJwtToken(inputToken);

            var encHeader = tokenToVerify.Header.Base64UrlEncode();
            var encPayload = tokenToVerify.Payload.Base64UrlEncode();
            var sign = CryptoProvider.GenerateSHMACHash($"{encHeader}.{encPayload}");
            if (tokenToVerify.RawSignature != sign)
            {
                return null;
            }

            if(tokenToVerify.Payload.ValidTo < DateTime.Now)
            {
                return null;
            }

            return tokenToVerify;
        }
    }
}
