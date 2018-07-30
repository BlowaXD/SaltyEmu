using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ChickenAPI.Core.Utils
{
    public static class StringExtensions
    {
        public static string ToSha512(this string str)
        {
            using (SHA512 hash = SHA512.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(str)).Select(item => item.ToString("x2")));
            }
        }
    }
}