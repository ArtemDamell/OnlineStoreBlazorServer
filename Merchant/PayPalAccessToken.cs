using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorShop.Merchant
{
    public class PayPalAccessToken
    {
        // Рамки (ограничения) запроса
        public string scope { get; set; }
        // Ключик доступа к API PayPal
        public string access_token { get; set; }
        // Тип доступа (API)
        public string token_type { get; set; }
        public string app_id { get; set; }
        // Время жизни запроса
        public int expires_in { get; set; }
        // Свойство для хранения описания случая
        public string nonce { get; set; }
    }
}
