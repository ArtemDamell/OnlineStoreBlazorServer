using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace BlazorShop.Merchant
{
    public static class PayPal
    {
        public static PayPalConfig GetPayPalConfig()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                 .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            return new PayPalConfig()
            {
                AuthToken = configuration["PayPal:AuthToken"],
                PostUrl = configuration["PayPal:PostUrl"],
                Business = configuration["PayPal:Business"],
                ReturnUrl = configuration["PayPal:ReturnUrl"],
            };
        }
    }
}
