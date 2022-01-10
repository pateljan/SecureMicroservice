using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.MVC.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Web.MVC.ApiServices
{
    public class MovieApiService : IMovieApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MovieApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<UserInfoViewModel> GetUserInfo()
        {
            var idpClient = _httpClientFactory.CreateClient("IDPClient");

            var metaDataResponse = await idpClient.GetDiscoveryDocumentAsync();
            if (metaDataResponse.IsError)
                throw new HttpRequestException("Something went wrong while requesting the access token");

            var accessToken = await _httpContextAccessor.HttpContext.
                                GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var userInfoResponse = await idpClient.GetUserInfoAsync(
                new UserInfoRequest
                {
                    Address = metaDataResponse.UserInfoEndpoint,
                    Token = accessToken
                });

            if (userInfoResponse.IsError)
                throw new HttpRequestException("Something went wrong while getting user info");

            var userinforDictionary = new Dictionary<string, string>();
            foreach (var claim in userInfoResponse.Claims)
            {
                userinforDictionary.Add(claim.Type, claim.Value);
            }

            return new UserInfoViewModel(userinforDictionary);
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            //Mothed 1

            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            //Api Call
            //var request = new HttpRequestMessage(HttpMethod.Get, "api/movies");
            
            //Ocelot ApiGateway
            var request = new HttpRequestMessage(HttpMethod.Get, "movies");

            var response = await httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            List<Movie> movieList = JsonConvert.DeserializeObject<List<Movie>>(content);
            return movieList;

            //Method 2

            //var apiClientCredentials = new ClientCredentialsTokenRequest
            //{
            //    Address = "https://localhost:5005/connect/token",
            //    ClientId = "movieClient",
            //    ClientSecret = "secret",
            //    Scope = "movieAPI"
            //};

            //var client = new HttpClient();

            //// just check if we can reach the Discovery document.
            //var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5005");
            //if (disco.IsError)
            //    return null;

            //// Authenticated and get an access token from Identity Server
            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
            //if (tokenResponse.IsError)
            //    return null;

            //// Send Request to Protected API
            ////Another HttpClient for talking now with our Protected API
            //var apiClient = new HttpClient();

            ////Set the access token in the request authrization : bearer token
            //apiClient.SetBearerToken(tokenResponse.AccessToken);

            //var response = await apiClient.GetAsync("https://localhost:5001/api/movies");
            //response.EnsureSuccessStatusCode();

            //var content = await response.Content.ReadAsStringAsync();

            //List<Movie> movieList = JsonConvert.DeserializeObject<List<Movie>>(content);
            //return movieList;
            
        }

        public async Task<Movie> GetMovie(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/movies/{id}");

            var response = await httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var _movie = JsonConvert.DeserializeObject<Movie>(content);

            return _movie;
        }

        public async Task<Movie> CreateMovie(Movie movie)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var requestModel = new StringContent(
                JsonConvert.SerializeObject(movie),
                Encoding.UTF8,
                Application.Json
                );

            var response = await httpClient.PostAsync("api/movie",requestModel).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var _movie = JsonConvert.DeserializeObject<Movie>(content);

            return _movie;

        }

        public async Task DeleteMovie(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var response = await httpClient.DeleteAsync($"api/movie/{id}").ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

        }
        

        public async Task<Movie> UpdateMovie(Movie movie)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var requestModel = new StringContent(
                JsonConvert.SerializeObject(movie),
                Encoding.UTF8,
                Application.Json
                );

            var response = await httpClient.PutAsync($"api/movies/{movie.Id}", requestModel).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var _movie = JsonConvert.DeserializeObject<Movie>(content);

            return _movie;
        }

        
    }
}
