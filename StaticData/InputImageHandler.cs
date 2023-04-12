using BlazorInputFile;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorShop.StaticData
{
    public static class InputImageHandler
    {
        /// <summary>
        /// Handles the image by copying it to a MemoryStream and returning the byte array.
        /// </summary>
        /// <param name="files">The files to be handled.</param>
        /// <returns>The byte array of the image.</returns>
        public static async Task<byte[]> HandleImage(IFileListEntry[] files)
        {
            var file = files.FirstOrDefault();
            if (file is not null)
            {
                MemoryStream memoryStream = new();
                await file.Data.CopyToAsync(memoryStream);
                var ImageUploaded = memoryStream.ToArray();
                return ImageUploaded;
            }
            return null;
        }
    }
}
