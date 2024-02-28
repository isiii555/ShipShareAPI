using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Persistence.Helpers
{
    public static class PasswordHashHelper
    {
        public static void CreatePassword(string password,out byte[] salt,out byte[] passwordHash)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public static bool ComparePassword(string password, byte[] passwordHash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var newPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); 
            return newPassword.SequenceEqual(passwordHash);
        }
    }
}
