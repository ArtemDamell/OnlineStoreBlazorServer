using System;

namespace BlazorShop.Merchant
{
    /* ********************************
     В этом общем классе будут хранится все данные о запросе к PayPal
     И о покупателе
     ФОРМИРОВАНИЕ ЗАПРОСА К PAYPAL
     * *******************************/
    public class PayPalRequest
    {
        public string id { get; set; }
        // Намерение (купить)
        public string intent { get; set; }
        // Состояние транзакции (статус: e.g. approved)
        public string state {  get; set; }
        // Связываем модель с покупателем
        public Payer payer { get; set; }
        public Transaction[] transactions {  get; set; }
        public DateTime creat_time { get; set; }
        public Link[] links {  get; set; }

        /* ********************************
            Создаём класс покупателя
        * ********************************/
        public class Payer
        {
            public string payment_method { get; set; }
        }

        /* ********************************
        Создаём класс информации о транзакции
        * ********************************/
        public class Transaction
        {
            public Amount amount { get; set; }
            public object[] related_resources { get; set; }
            public string description { get; set; }
        }

        /* ********************************
        Создаём класс информации о сумме
        * ********************************/
        public class Amount
        {
            public string total { get; set; }
            public string currency { get; set; }
        }

        /* ********************************
        Создаём класс информации о ссылках
        * ********************************/
        public class Link
        {
            public string href { get; set; }
            public string rel { get; set; }
            public string method { get; set; }
        }
    }
}
