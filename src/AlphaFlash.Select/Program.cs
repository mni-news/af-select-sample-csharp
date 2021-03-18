using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaFlash.Select.Service;
using AlphaFlash.Select.Dto;
using System.Threading;
using AlphaFlash.Select.Demo;
using System.Text.Json;
using System.IO;

namespace AlphaFlash.Select
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();

        }

        static async Task RunAsync()
        {
            Config config;

            try{
                config =  JsonSerializer.Deserialize<Config>(File.ReadAllText("config.json"));
            } catch (Exception e){

                Console.WriteLine( 
@"Could not load config file. To Run, place a file called 'config.json' in the project. Example:

{
    ""username"":""xxxxxx"",
    ""password"":""xxxxxx""
}
");

                return;
            }

            //
            // Initialize help classes
            //
            HttpClient httpClient = new HttpClient();
            AuthenticationService authenticationService = new AuthenticationService(httpClient);
            SelectDataService selectDataService = new SelectDataService(httpClient);
            SelectRealTimeDataService realTimeDataService = new SimpleSelectRealTimeDataService(
                "select.alphaflash.com", 61614, true
            );
            

            //
            // Authenticate, then set up a scheduled task to refresh tokens
            //
            AuthResponse authResponse = await authenticationService.Authenticate(config.Username,config.Password);

            httpClient.DefaultRequestHeaders.Authorization = authResponse.AuthenticationHeaderValue;
            realTimeDataService.AccessToken = authResponse.AccessToken;
            string refreshToken = authResponse.RefreshToken;


            Timer timer = new Timer(state=>{

                authenticationService.Refresh(refreshToken).ContinueWith(task=>{

                    AuthResponse response = task.Result;

                    httpClient.DefaultRequestHeaders.Authorization = response.AuthenticationHeaderValue;
                    realTimeDataService.AccessToken = response.AccessToken;
                    refreshToken = response.RefreshToken;

                    Console.WriteLine("Refreshed Tokens");

                });

            },null,120_000,120_000);
    

            
            //
            // Some handlers for events in the real time service
            //
            realTimeDataService.ConnectHandler = message =>Console.WriteLine("Connected");
            realTimeDataService.DisconnectHandler = ()=>Console.WriteLine("Disconnected");
            realTimeDataService.ConnectFailHandler = message => Console.WriteLine("Connection Failed");
            realTimeDataService.ExceptionHandler = e => Console.WriteLine(e.Message);
            realTimeDataService.HeartbeatHandler = () => Console.WriteLine("Hearbeat");


            //
            // This handler gets actual data from the service
            //
            List<DataSeries> allDataSeries = await selectDataService.GetAllDataSeries();

            allDataSeries.ForEach(series=>{
                Console.WriteLine($"{series.Id} - {series.Display} - {series.Scale?.Display} - {series.Type.Display}");
            });

            realTimeDataService.ObservationHandler = observaions => {

                allDataSeries.ForEach(series => {

                    observaions.ForEach(observaion => {
                        if (series.Id == observaion.DataSeriesId){
                            Console.WriteLine($"{series.Id} - {series.Display}: {observaion.Value}");
                        }
                    });
                    
                });

            };

            realTimeDataService.Run();


        }
    }
}
