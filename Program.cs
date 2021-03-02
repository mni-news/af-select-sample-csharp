using AlphaFlashSelectClient.dto;
using AlphaFlashSelectClient.stomp;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;

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

            HttpResponseMessage response = await httpClient
                .PostAsJsonAsync(
                    "https://api.alphaflash.com/api/auth/alphaflash-client/token", 
                    new AuthRequest { Username = "dev", Password = "4Development" }
                    );

        


            AuthResponse authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",authResponse.AccessToken
            );

            long totalPages = 0;
            long currentPage = 0;

            do {

                Page<Event> calendarData = await httpClient.GetFromJsonAsync<Page<Event>>(
                    $"https://api.alphaflash.com/api/select/calendar/events?page={currentPage}"
                );

                currentPage++;
                totalPages = calendarData.TotalPages;



                Console.WriteLine(calendarData.Content.Count);

                calendarData.Content.ForEach( (e)=>{
                    Console.WriteLine($"{e.Id} {e.Date}");
                });
              

                Console.WriteLine(currentPage);

               

            } while(currentPage < totalPages);



            StompConnection stompConnection = new StompConnection("select.alphaflash.com", 61613);

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
