using System;

namespace BlazorShop.Extensions
{
    public static class ImageConverter
    {
        /// <summary>
        /// Converts a byte array to a string representation of an image.
        /// </summary>
        /// <param name="image">The byte array to convert.</param>
        /// <returns>A string representation of the image.</returns>
        public static string ImageToDisplayConverter(this byte[] image)
        {
            if (image is null)
                return "https://askleo.askleomedia.com/wp-content/uploads/2004/06/no_image-300x245.jpg";

            var base64 = Convert.ToBase64String(image);
            var finalString = string.Format("data:image/jpg;base64,{0}", base64);

            return finalString;
        }
    }
}
