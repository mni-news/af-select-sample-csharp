using AlphaFlashSelectClient.dto;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace AlphaFlashSelectClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            HttpClient httpClient = new HttpClient();


            string json = JsonConvert.SerializeObject(new AuthRequest { username = "joe", password = "bob" });

            Console.WriteLine(json);



        }
    }
}
