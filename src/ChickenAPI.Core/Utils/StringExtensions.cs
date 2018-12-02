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

        public static string Truncate(this string str, int length)
        {
            if (str.Length > length)
            {
                return str.Substring(0, length);
            }

            return str;
        }
    }
}