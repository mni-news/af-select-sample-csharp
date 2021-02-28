using AlphaFlashSelectClient.dto;
using AlphaFlashSelectClient.stomp;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                    new AuthRequest { Username = "dev", Password = "4Development" }
                    );

            AuthResponse authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

            Console.WriteLine("ASD" + response.StatusCode);

            Console.WriteLine(authResponse.AccessToken);

            StompConnection stompConnection = new StompConnection("alphaflash01.chi0.mni-news.com", 61613);

            StompMessage connectResult =   stompConnection.connect(authResponse.AccessToken);
            stompConnection.Subscribe("/topic/observations");

            Console.WriteLine(connectResult.MessageType);

            while (true)
            {
                StompMessage stompMessage = stompConnection.readMessage();

                Console.WriteLine(stompMessage.MessageType);
                Console.WriteLine(stompMessage.Body);

                List<Observation> observatons = JsonSerializer.Deserialize<List<Observation>>(stompMessage.Body);


                foreach (Observation o in observatons)
                {
                    Console.WriteLine(o);
                }
            }


        }
    }
}
