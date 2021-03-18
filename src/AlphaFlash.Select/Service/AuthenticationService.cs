

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AlphaFlash.Select.Dto;

namespace AlphaFlash.Select.Service {

    class AuthenticationService{


        private readonly HttpClient httpClient;

        public AuthenticationService(HttpClient httpClient){
            this.httpClient = httpClient;
        }

        public async Task<AuthResponse> Authenticate(string username, string password){
            HttpResponseMessage response = await httpClient
                .PostAsJsonAsync(
                    "https://api.alphaflash.com/api/auth/alphaflash-client/token", 
                    new AuthRequest { Username = "dev", Password = "4Development" }
                    );

            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }


        public async Task<AuthResponse> Refresh(string refreshToken){
             HttpResponseMessage response = await httpClient
                .PostAsJsonAsync(
                    "https://api.alphaflash.com/api/auth/refresh", 
                    new RefreshRequest { RefreshToken = refreshToken}
                    );

            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

    }
}