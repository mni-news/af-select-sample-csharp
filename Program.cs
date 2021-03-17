using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AlphaFlash.Select.Service;
using AlphaFlash.Select.Dto;
using System.Threading;
using System.Reflection;
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
    ""username"":""test"",
    ""password"":""xxxxxx""
}
");

                return;
            }

            HttpClient httpClient = new HttpClient();
            AuthenticationService authenticationService = new AuthenticationService(httpClient);
            SelectDataService selectDataService = new SelectDataService(httpClient);
            SelectRealTimeDataService realTimeDataService = new SimpleSelectRealTimeDataService(
                "select.alphaflash.com", 61614, true
            );
            
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

                });

            },null,5000,5000);
    

            List<DataSeries> allDataSeries = await selectDataService.GetAllDataSeries();

            realTimeDataService.ConnectHandler = message =>Console.WriteLine("Connected");
            realTimeDataService.DisconnectHandler = ()=>Console.WriteLine("Disconnected");
            realTimeDataService.ConnectFailHandler = message => Console.WriteLine("Connection Failed");
            realTimeDataService.ExceptionHandler = e => Console.WriteLine(e.Message);

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
