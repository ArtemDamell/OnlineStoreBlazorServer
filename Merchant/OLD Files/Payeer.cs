using System;
using System.Security.Cryptography;
using System.Text;

namespace BlazorShop.Merchant
{
    // Урок 12 (4)
    public static class Payeer
    {
        // Создаём 2 статических метода для формирования платежа
        public static string SignHash(string text)
        {
            byte[] data = Encoding.Default.GetBytes(text);
            var result = new SHA256Managed().ComputeHash(data);
            return BitConverter.ToString(result).Replace("-", "").ToUpper();
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}