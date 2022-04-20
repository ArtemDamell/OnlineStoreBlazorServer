using System;

namespace BlazorShop.Extensions
{
    public static class ImageConverter
    {
        public static string ImageToDisplayConverter(this byte[] image)
        {
            // Проверяем, есть ли картинка
            if (image is null)
                return "https://askleo.askleomedia.com/wp-content/uploads/2004/06/no_image-300x245.jpg";

            // Конвертируем массив байт в base64
            var base64 = Convert.ToBase64String(image);

            // Создаём финальный вариант строки
            var finalString = string.Format("data:image/jpg;base64,{0}", base64);

            // Возвращаем результат конвертации
            return finalString;
        }
    }
}
