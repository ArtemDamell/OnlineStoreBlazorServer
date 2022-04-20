using BlazorInputFile;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorShop.StaticData
{
    public static class InputImageHandler
    {
        public static async Task<byte[]> HandleImage(IFileListEntry[] files)
        {
            // Заносим файл в новую локальную переменную
            var file = files.FirstOrDefault();

            // Проверяем, найдено ли изображение
            if (file is not null)
            {
                // Создает поток, резервным хранилищем которого является память.
                MemoryStream memoryStream = new();

                // Копируем изображение в этот поток
                await file.Data.CopyToAsync(memoryStream);

                // Записываем данные из потока  локальную переменную
                var ImageUploaded = memoryStream.ToArray();
                return ImageUploaded;
            }
            return null;
        }
    }
}
