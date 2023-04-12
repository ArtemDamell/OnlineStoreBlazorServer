namespace BlazorShop.Extensions
{
    public static class Base64Converter
    {
        /// <summary>
        /// Converts a string to a Base64 encoded string.
        /// </summary>
        /// <param name="_text">The string to be encoded.</param>
        /// <returns>A Base64 encoded string.</returns>
        public static string ToBase64Encode(this string _text)
        {
            var byteText = System.Text.Encoding.UTF8.GetBytes(_text);
            return System.Convert.ToBase64String(byteText);
        }

        /// <summary>
        /// Decodes a base64 encoded string to a UTF8 string.
        /// </summary>
        /// <param name="_base64EncodedData">The base64 encoded string.</param>
        /// <returns>The decoded UTF8 string.</returns>
        public static string FromBase64Decode(this string _base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(_base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
