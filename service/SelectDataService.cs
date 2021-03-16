

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AlphaFlashSelectClient.dto;

namespace AlphaFlashSelectClient.service {

    class SelectDataService{
        private readonly HttpClient httpClient;

        int PAGE_SIZE = 10;
    

        public SelectDataService(HttpClient httpClient){
            this.httpClient = httpClient;
        }

        public async Task<Page<Event>> getCalendarEventPage(DateTimeOffset start, DateTimeOffset end, long page, int size){
            string request = $"https://api.alphaflash.com/api/select/calendar/events?" + 
                             $"start={start.ToUnixTimeMilliseconds()}&end={end.ToUnixTimeMilliseconds()}" + 
                             $"&page={page}&size={size}";
        
            return await httpClient.GetFromJsonAsync<Page<Event>>(request);
        }

        public async Task<List<Event>> getCalendarEventsBetween(DateTimeOffset start, DateTimeOffset end){
            return await this.Depage<Event>( (page, size) => this.getCalendarEventPage(start,end,page, size ) );
        }

        public async Task<Page<DataSeries>> GetDataSeriesPage(long page, int size){
            string request = $"https://api.alphaflash.com/api/select/series?" + 
                             $"&page={page}&size={size}";
        
            return await httpClient.GetFromJsonAsync<Page<DataSeries>>(request);
        }

        public async Task<List<DataSeries>> GetAllDataSeries(){
            return await this.Depage<DataSeries>( this.GetDataSeriesPage );
        }

        

        public async Task<List<T>> Depage<T>(Func<long, int, Task<Page<T>>> query){
            long pageCount = 1;

            List<T> result = new List<T>();

            for (long i = 0; i < pageCount; i++){
                Page<T> page = await query( i, PAGE_SIZE);

                result.AddRange(page.Content);
                
                pageCount = page.TotalPages;
            }

            return result;
        }

    }
}