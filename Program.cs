using AlphaFlashSelectClient.dto;
//using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlphaFlashSelectClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();

        }

        static async Task RunAsync()
        {

            HttpClient httpClient = new HttpClient();



            Console.WriteLine("ASD");
            HttpResponseMessage response = await httpClient
                .PostAsJsonAsync(
                    "https://api.alphaflash.com/api/auth/alphaflash-client/token", 
                    new AuthRequest { Username = "xxx", Password = "xxxx" }
                    );

            Console.WriteLine(JsonSerializer.Serialize(new AuthRequest { Username = "dev", Password = "4Development" }));

            Console.WriteLine("ASD");

            AuthResponse authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

            Console.WriteLine("ASD" + response.StatusCode);

            Console.WriteLine(authResponse.AccessToken);

        }
    }
}
