using System.Security.Cryptography;
using System.Text;

namespace API.Helpers
{
    public class HashingHelper
    {
        public static string ComputeHash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Şifreyi byte dizisine çevir
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Byte dizisini hexadecimal string'e dönüştür
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
