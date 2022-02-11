using System;
using System.Text;
using ArtAuction.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;

namespace ArtAuction.Core.Application.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IConfiguration _configuration;

        public PasswordService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetHash(string inputPassword)
        {
            var salt = Encoding.UTF8.GetBytes(_configuration["Cryptography:PasswordSalt"]);
            
            var hashedPass = Convert.ToHexString(
                KeyDerivation.Pbkdf2(
                    password: inputPassword, 
                    salt: salt, 
                    prf: KeyDerivationPrf.HMACSHA256, 
                    iterationCount: 10000, 
                    numBytesRequested: 256 / 8
                )
            );

            return hashedPass;
        }

        public bool IsPasswordCorrect(string password, string hash)
        {
            return hash.Equals(GetHash(password));
        }
    }
}