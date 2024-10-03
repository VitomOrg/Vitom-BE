using Domain.Primitives;
using Microsoft.Extensions.Options;
using Net.payOS;
using System.Text;

namespace API.Extensions;

public static class PayOSExtension
{
    public static async Task<IHost> AddPayOS(this IHost host)
    {
        var _payOSSettings = host.Services.GetRequiredService<IOptionsMonitor<PayOSSettings>>();
        var payOSSettings = _payOSSettings.CurrentValue;
        string clientId = payOSSettings.ClientId;
        string apiKey = payOSSettings.ApiKey;
        string checkSumKey = payOSSettings.CheckSumKey;
        // PayOS payOS = new(clientId, apiKey, checkSumKey);
        Console.WriteLine("Client Id: " + clientId);
        Console.WriteLine("Api Key: " + apiKey);
        Console.WriteLine("Check Sum Key: " + checkSumKey);
        string webhook = "";
        try
        {
            // webhook = await payOS.confirmWebhook("https://vitom-api.persiehomeserver.com/payment/webhook/");
            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("x-client-id", clientId);
            httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
            string url = "https://api-merchant.payos.vn/v2/payment-requests/";
            var response = await httpClient.PostAsync(url, new StringContent("{\"webhookUrl\": \"https://vitom-api.persiehomeserver.com/payment/webhook\"}", Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                webhook = await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        Console.WriteLine("Webhook: " + webhook);
        return host;
    }
}