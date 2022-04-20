using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorShop.Extensions
{
    public static class Base64Converter
    {
        public static string ToBase64Encode(this string _text)
        {
            var byteText = System.Text.Encoding.UTF8.GetBytes(_text);
            return System.Convert.ToBase64String(byteText);
        }

        public static string FromBase64Decode(this string _base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(_base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
