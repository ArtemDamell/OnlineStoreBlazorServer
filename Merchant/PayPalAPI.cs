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
        public IConfiguration configuration { get; }
        public PayPalAPI(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        /// <summary>
        /// Gets the redirect URL to PayPal for a given payment.
        /// </summary>
        /// <param name="total">The total amount of the payment.</param>
        /// <param name="currency">The currency of the payment.</param>
        /// <param name="appointmentId">The ID of the appointment.</param>
        /// <returns>The redirect URL to PayPal.</returns>
        public string getRedirectUrlToPayPal(double total, string currency, int appointmentId)
        {
            try
            {
                return Task.Run(async () =>
                {
                    HttpClient http = GetPayPalHttpClient();
                    PayPalAccessToken accessToken = await GetPayPalAccessTokenAsync(http);
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

        /// <summary>
        /// Executes a PayPal payment with the given paymentId and payerId.
        /// </summary>
        /// <param name="paymentId">The paymentId of the payment to be executed.</param>
        /// <param name="payerId">The payerId of the payment to be executed.</param>
        /// <returns>A PayPalResponse object containing the response from the PayPal API.</returns>
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

        /// <summary>
        /// Creates an HttpClient with the PayPal API URL and a timeout of 30 seconds.
        /// </summary>
        /// <returns>An HttpClient with the PayPal API URL and a timeout of 30 seconds.</returns>
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

        /// <summary>
        /// Gets the PayPal access token asynchronously.
        /// </summary>
        /// <param name="http">The HTTP client.</param>
        /// <returns>The PayPal access token.</returns>
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

        /// <summary>
        /// Creates a PayPal payment request for a given appointment.
        /// </summary>
        /// <param name="http">The HttpClient to use for the request.</param>
        /// <param name="accessToken">The PayPal access token.</param>
        /// <param name="total">The total amount of the payment.</param>
        /// <param name="currency">The currency of the payment.</param>
        /// <param name="appointmentId">The ID of the appointment.</param>
        /// <returns>The created PayPal payment request.</returns>
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

        /// <summary>
        /// Executes a PayPal payment with the given paymentId and payerId.
        /// </summary>
        /// <param name="http">The HttpClient to use for the request.</param>
        /// <param name="accessToken">The access token to use for authentication.</param>
        /// <param name="paymentId">The paymentId of the payment to execute.</param>
        /// <param name="payerId">The payerId of the payment to execute.</param>
        /// <returns>The response from the PayPal API.</returns>
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