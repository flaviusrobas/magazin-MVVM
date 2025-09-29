using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Portal.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Portal.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorageService;
        private readonly IConfiguration _config;
        private string authTokenStorageKey;
        public AuthenticationService(HttpClient client,
                                     AuthenticationStateProvider authStateProvider,
                                     ILocalStorageService localStorageService,
                                     IConfiguration config  )
        {
            _client = client;
            _authStateProvider = authStateProvider;
            _localStorageService = localStorageService;
            _config = config;
            authTokenStorageKey = _config[key:"authTokenStorageKey"];
        }

        //public async Task<Result>               Login(Request)
        public async Task<AuthenticatedUserModel> Login(AuthenticationUserModel userForAuthentication)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", userForAuthentication.Email),
                new KeyValuePair<string, string>("password", userForAuthentication.Password),
            });

            string api = _config["api"] + _config["tokenEndpoint"];
            var authResult = await _client.PostAsync(api, data);
            var authContent = await authResult.Content.ReadAsStringAsync();

            if (authResult.IsSuccessStatusCode == false)
            {
                return null;
            }

            var result = JsonSerializer.Deserialize<AuthenticatedUserModel>(
                authContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await _localStorageService.SetItemAsync("authToken", result.Access_Token);

            //doar prin casting (AuthStateProvider) putem apela NotifyUserAuthentication
            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Access_Token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Access_Token);

            return result;

        }

        public async Task Logout()
        {
            //stergem tokenul din local storage
            await _localStorageService.RemoveItemAsync("authToken");

            //doar prin casting ((AuthStateProvider)_authStateProvider) putem apela NotifyUserLogout
            //_authStateProvider.NotifyUserLogout() nu functioneaza deoarece nu are o metoda de autenticare definita
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();

            //stergem anteturile de autentificare din header-ul HttpClient  
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
