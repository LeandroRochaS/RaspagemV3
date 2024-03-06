using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RaspagemV3.Services.Whatsapp
{
    public class WhatsappService
    {
        public async Task<bool> SendMensageWhatsapp(string number, string Mensage)
        {
            try
            {
                var parameters = new NameValueCollection();
                var client = new WebClient();

                var url = "https://app.whatsgw.com.br/api/WhatsGw/Send/";

                parameters.Add("apikey", "06f614d9-16db-4699-936a-a1564f9f39cb");
                parameters.Add("phone_number", "5579991488569");
                parameters.Add("contact_phone_number", $"55{number}");
                parameters.Add("message_custom_id", "tste");
                parameters.Add("message_type", "text");
                parameters.Add("message_body", Mensage);

                byte[] response_data = await client.UploadValuesTaskAsync(url, "POST", parameters);
                string responseString = Encoding.UTF8.GetString(response_data);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FAIL: {ex.Message}");
                // Log the exception or handle it more gracefully
                return false;
            }
        }
    }
}
