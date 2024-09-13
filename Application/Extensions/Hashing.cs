using Domain.DTOs.Security.Login;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class Hashing
    {
        //private const int SaltSize = 128 / 8;
        //private const int KeySize = 256 / 8;
        //private const int Iterations = 10000;
        //private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        //private const char Delimiter = ';';

        //public static string Main(string password)
        //{
        //    //byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        //    //string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //    //                password: password,
        //    //                salt: salt,
        //    //                prf: KeyDerivationPrf.HMACSHA256,
        //    //                iterationCount: 100000,
        //    //                numBytesRequested: 256 / 8
        //    //));

            
        //}

    }
}
