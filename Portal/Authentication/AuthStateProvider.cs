using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Portal.Authentication
{
    /* class AuthStateProvider extinde clasa AuthenticationStateProvider
     * cu scopul de a gestiona starea de autentificare a utilizatorilor 
     * NotifyUserAuthentication, NotifyUserLogout  */
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _config;
        private readonly AuthenticationState _anonymous;

        public AuthStateProvider(HttpClient httpClient,
            ILocalStorageService localStorageService,
            IConfiguration config)
    
        {
            _httpClient = httpClient;
            _localStorage = localStorageService;
            _config = config;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
          
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string authTokenStorageKey = _config[key: "authTokenStorageKey"];
            var token = await _localStorage.GetItemAsStringAsync(authTokenStorageKey);

            if (string.IsNullOrWhiteSpace(token))
            {
                return _anonymous;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", token);
           
            return new AuthenticationState(
                user: new ClaimsPrincipal(
                 identity:   new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token),
                    authenticationType: "jwtAuthType")));
        }

        public void NotifyUserAuthentication(string token)
        {
           var authenticatedUser = new ClaimsPrincipal(
                identity: new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token),
                    authenticationType: "jwtAuthType"));

            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
