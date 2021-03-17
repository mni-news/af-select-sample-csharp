

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AlphaFlash.Select.Dto;

namespace AlphaFlash.Select.Service {

    class SelectDataService{
        private readonly HttpClient httpClient;
    
        public SelectDataService(HttpClient httpClient){
            this.httpClient = httpClient;
        }

        public async Task<Page<Event>> getCalendarEventPage(DateTimeOffset start, DateTimeOffset end, long page, int size){
            string request = $"https://api.alphaflash.com/api/select/calendar/events?" + 
                             $"start={start.ToUnixTimeMilliseconds()}&end={end.ToUnixTimeMilliseconds()}" + 
                             $"&page={page}&size={size}";
        
            return await httpClient.GetFromJsonAsync<Page<Event>>(request);
        }

        public async Task<List<Event>> getCalendarEventsBetween(DateTimeOffset start, DateTimeOffset end, int pageSize = 100){
            return await this.Depage<Event>( (page, size) => this.getCalendarEventPage(start,end,page, size ), pageSize );
        }

        public async Task<Page<DataSeries>> GetDataSeriesPage(long page, int size){
            string request = $"https://api.alphaflash.com/api/select/series?" + 
                             $"&page={page}&size={size}";
        
            return await httpClient.GetFromJsonAsync<Page<DataSeries>>(request);
        }

        public async Task<List<DataSeries>> GetAllDataSeries(int pageSize = 100){
            return await this.Depage<DataSeries>( this.GetDataSeriesPage, pageSize );
        }

        public async Task<DataSeries> GetDataSeries(long id){ 
            return await httpClient.GetFromJsonAsync<DataSeries>($"https://api.alphaflash.com/api/select/series/{id}");
        }

        public async Task<Page<DataRelease>> GetDataReleasePage(long page, int size){
            string request = $"https://api.alphaflash.com/api/select/release?" + 
                             $"&page={page}&size={size}";
        
            return await httpClient.GetFromJsonAsync<Page<DataRelease>>(request);
        }

        public async Task<List<DataRelease>> GetAllDataReleases(int pageSize = 100){
            return await this.Depage<DataRelease>( this.GetDataReleasePage , pageSize);
        }

        public async Task<DataRelease> GetDataRelease(long id){ 
            return await httpClient.GetFromJsonAsync<DataRelease>($"https://api.alphaflash.com/api/select/release/{id}");
        }

        

        

        public async Task<List<T>> Depage<T>(Func<long, int, Task<Page<T>>> query, int pageSize){
            long pageCount = 1;

            List<T> result = new List<T>();

            for (long i = 0; i < pageCount; i++){
                Page<T> page = await query( i, pageSize);

                result.AddRange(page.Content);
                
                pageCount = page.TotalPages;
            }

            return result;
        }

    }
}