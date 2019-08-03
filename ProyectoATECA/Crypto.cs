using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ProyectoATECA
{
    public static class Crypto
    {
        public static string Hash(string value)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();
            Byte[] originalBytes = encoder.GetBytes(value);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            value = BitConverter.ToString(encodedBytes).Replace("-", "");
            var result = value.ToLower();
            return result;
        }
    }
}