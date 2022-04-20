using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlazorShop.Merchant
{
    public class PayPalAPI
    {
        // Создаём бубличную переменную для доступа к конфигурации
        // Из любого другова класса, но делаем доступной ТОЛЬКО ДЛЯ ЧТЕНИЯ
        public IConfiguration configuration { get; }

        // Присваеваем экземпляр через конструктор
        public PayPalAPI(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public string getRedirectUrlToPayPal(double total, string currency, int appointmentId)
        {
            // Так как на этом этапе может возникнуть ошибка
            // Заключаем всё в конструкцию TRY/CATCH
            try
            {
                return Task.Run(async () =>
                {
                    // Конфигурируем HTTP запрос для PayPal
                    HttpClient http = GetPayPalHttpClient();
                    // Получаем токен доступа к магазину
                    PayPalAccessToken accessToken = await GetPayPalAccessTokenAsync(http);
                    // Сохраняем результат ответа от PayPal
                    PayPalRequest createdPayment = await CreatePayPalPaymentAsync(http, accessToken, total, currency, appointmentId);
                    return createdPayment.links.First(x => x.rel == "approval_url").href;
                }).Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Failed to login to PayPal");
                return null;
            }
        }

        public async Task<PayPalResponse> exequtedPayment(string paymentId, string payerId)
        {
            try
            {
                HttpClient http = GetPayPalHttpClient();
                PayPalAccessToken accessToken = await GetPayPalAccessTokenAsync(http);
                return await ExecutePaypalPaymentAsync(http, accessToken, paymentId, payerId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Failed to login to PayPal");
                return null;
            }
        }

        private HttpClient GetPayPalHttpClient()
        {
            string paypalConfig = configuration["PayPal:urlAPI"];

            var http = new HttpClient
            {
                BaseAddress = new Uri(paypalConfig),
                Timeout = TimeSpan.FromSeconds(30)
            };

            return http;
        }

        private async Task<PayPalAccessToken> GetPayPalAccessTokenAsync(HttpClient http)
        {
            byte[] bytes = Encoding.GetEncoding("iso-8859-1")
                .GetBytes($"{configuration["PayPal:clientId"]}:{configuration["PayPal:secret"]}");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));

            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            };

            request.Content = new FormUrlEncodedContent(form);

            HttpResponseMessage response = await http.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();
            var accessToken = JsonConvert.DeserializeObject<PayPalAccessToken>(content);

            return accessToken;
        }

        private async Task<PayPalRequest> CreatePayPalPaymentAsync(HttpClient http, PayPalAccessToken accessToken, double total, string currency, int appointmentId)
        {
            HttpRequestMessage request = new(HttpMethod.Post, "v1/payments/payment");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

            var payment = JObject.FromObject(new
            {
                intent = "sale",
                redirect_urls = new
                {
                    return_url = configuration["PayPal:returnUrl"],
                    cancel_url = configuration["PayPal:cancelUrl"]
                },
                payer = new { payment_method = "paypal" },
                transactions = JArray.FromObject(new[]
                {
                    new
                    {
                        amount = new
                        {
                            total = total,
                            currency = currency
                        },
                        description = appointmentId.ToString()
                    }
                })
            });

            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await http.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
            PayPalRequest createdPaypalPayment = JsonConvert.DeserializeObject<PayPalRequest>(content);
            return createdPaypalPayment;
        }

        async Task<PayPalResponse> ExecutePaypalPaymentAsync(HttpClient http, PayPalAccessToken accessToken, string paymentId, string payerId)
        {
            HttpRequestMessage request = new(HttpMethod.Post, $"v1/payments/payment/{paymentId}/execute");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

            var payment = JObject.FromObject(new
            {
                payer_id = payerId
            });
            request.Content = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await http.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();
            PayPalResponse executedPayment = JsonConvert.DeserializeObject<PayPalResponse>(content);
            return executedPayment;
        }
    }
}