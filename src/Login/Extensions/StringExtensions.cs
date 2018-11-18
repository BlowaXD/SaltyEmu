using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Login.Extensions
{
    public static class StringExtensions
    {
        public static string Sha512(this string inputString)
        {
            using (SHA512 hash = SHA512.Create())
            {
                return string.Join(string.Empty, hash.ComputeHash(Encoding.UTF8.GetBytes(inputString)).Select(item => item.ToString("x2")));
            }
        }
    }
}