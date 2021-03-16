using AlphaFlashSelectClient.dto;
using AlphaFlashSelectClient.stomp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using AlphaFlashSelectClient.service;
using Extend;

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

            SelectDataService selectDataService = new SelectDataService(httpClient);


            List<Event> events = await selectDataService.getCalendarEventsBetween(
                DateTimeOffset.Now.Subtract(new TimeSpan(4, 0 , 0, 0)), 
                DateTimeOffset.Now.Add(new TimeSpan(4,0 , 0, 0))
            );

            events.ForEach( x => {
                Console.WriteLine($"{x.Id} - {x.Title} - {x.Date}");
            });

            List<DataSeries> series = await selectDataService.GetAllDataSeries();

            series.ForEach( s => Console.WriteLine($"{s.Id} - {s.Display}"));



            StompConnection stompConnection = new StompConnection("select.alphaflash.com", 61613);

            StompMessage connectResult =   stompConnection.connect(authResponse.AccessToken);
            stompConnection.Subscribe("/topic/observations");

            Console.WriteLine(connectResult.ToString());

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
