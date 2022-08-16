using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using api.Model;

namespace api.Helper
{
    public static class HelperFunction
    {

        public static void PasswordConfiguration(String Password, ref User user){
             
            using var hmac = new HMACSHA256();
            
            // just store Hash of password is very danger.. 
            // https://auth0.com/blog/adding-salt-to-hashing-a-better-way-to-store-passwords/
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Password));
            user.PasswordSalt = hmac.Key;
        }



        public static bool IsPasswordRight(byte[] PasswordHash, byte[] PasswordSalt, string EnterPassword)
        {
            using var hmac = new HMACSHA256(PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(EnterPassword));

            for(int i = 0; i < computedHash.Length; i++){
                if(computedHash[i] != PasswordHash[i]){
                    return false;
                }
            }

            return true;
        }

        public static bool IsBirthDayValid(DateTime dateTime)
        {
            int age = DateTime.Now.Year - dateTime.Year;
            if(age < 18) return false;

            return true;
        }
    }
}