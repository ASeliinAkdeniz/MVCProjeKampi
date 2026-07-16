using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Concrete
{
    public static class HashHelper
    {
        public static string CreateHash(string input)
        {
            if (input == null) input = "";
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}