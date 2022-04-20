using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;

namespace BlazorShop.Merchant
{
    public class PDTHolder
    {
        public double GrossTotal { get; set; }
        public int InvoceNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string PayerFirstName { get; set; }
        public double PaymentFee { get; set; }
        public string BusinessEmail { get; set; }
        public string PayerEmail { get; set; }
        public string TxToken { get; set; }
        public string PayerLastName { get; set; }
        public string ReceiverEmail { get; set; }
        public string ItemName { get; set; }
        public string Currency { get; set; }
        public string TransactionId { get; set; }
        public string SubscriberId { get; set; }
        public string Custom { get; set; }

        static string authToken, txToken, query, strResponse;

        public static PDTHolder Success(string tx)
        {

            PayPalConfig payPalConfig = PayPal.GetPayPalConfig();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            authToken = payPalConfig.AuthToken;
            txToken = tx;
            query = string.Format($"cmd=_notify-synch&tx={txToken}&at={authToken}");
            string url = payPalConfig.PostUrl;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = query.Length;

            StreamWriter strOut = new(request.GetRequestStream(), System.Text.Encoding.ASCII);
            strOut.Write(query);
            strOut.Close();

            StreamReader strIn = new(request.GetResponse().GetResponseStream());
            strResponse = strIn.ReadToEnd();
            strIn.Close();

            if (strResponse.StartsWith("SUCCESS"))
                return PDTHolder.Parse(strResponse);
            return null;
        }

        static PDTHolder Parse(string postData)
        {
            string sKey, sValue;
            PDTHolder ph = new();

            try
            {
                string[] stringArray = postData.Split('\n');
                int i;

                for (i = 1; i < stringArray.Length - 1; i++)
                {
                    string[] array1 = stringArray[i].Split('=');

                    sKey = array1[0];
                    sValue = HttpUtility.UrlDecode(array1[1]);

                    switch (sKey)
                    {
                        case "mc_gross":
                            ph.GrossTotal = double.Parse(sValue, CultureInfo.InvariantCulture);
                            break;
                        case "invoce":
                            ph.InvoceNumber = Convert.ToInt32(sValue);
                            break;
                        case "payment_status":
                            ph.PaymentStatus = Convert.ToString(sValue);
                            break;
                        case "first_name":
                            ph.PayerFirstName = Convert.ToString(sValue);
                            break;
                        case "mc_fee":
                            ph.PaymentFee = double.Parse(sValue, CultureInfo.InvariantCulture);
                            break;
                        case "business":
                            ph.BusinessEmail = Convert.ToString(sValue);
                            break;
                        case "payer_email":
                            ph.PayerEmail = Convert.ToString(sValue);
                            break;
                        case "Tx_Token":
                            ph.TxToken = Convert.ToString(sValue);
                            break;
                        case "last_name":
                            ph.PayerLastName = Convert.ToString(sValue);
                            break;
                        case "receiver_email":
                            ph.ReceiverEmail = Convert.ToString(sValue);
                            break;
                        case "item_name":
                            ph.ItemName = Convert.ToString(sValue);
                            break;
                        case "mc_currency":
                            ph.Currency = Convert.ToString(sValue);
                            break;
                        case "txn_id":
                            ph.TransactionId = Convert.ToString(sValue);
                            break;
                        case "custom":
                            ph.Custom = Convert.ToString(sValue);
                            break;
                        case "subscr_id":
                            ph.SubscriberId = Convert.ToString(sValue);
                            break;
                    }
                }
                return ph;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }


}
