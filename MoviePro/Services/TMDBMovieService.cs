using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MoviePro.Enums;
using MoviePro.Models.Settings;
using MoviePro.Models.TmDb;
using MoviePro.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace MoviePro.Services
{
    public class TMDBMovieService : IRemoteMovieService
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _httpClient;

        public TMDBMovieService(IOptions<AppSettings> appSettings,
            IHttpClientFactory httpClient)
        {
            _appSettings = appSettings.Value;
            _httpClient = httpClient;
        }

        public async Task<ActorDetail> ActorDetailAsync(int id)
        {
            ActorDetail actorDetail = new ActorDetail();

            var query = $"{_appSettings.TmDbSettings.BaseUrl}/person/{id}";
            var queryParams = new Dictionary<string, string>()
            {
                { "api_key" , _appSettings.MovieProSettings.TmDbApiKey },
                {"language", _appSettings.TmDbSettings.QueryOptions.Language }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var dcjs = new DataContractJsonSerializer(typeof(ActorDetail));
                using var responseStream = await response.Content.ReadAsStreamAsync();
                actorDetail = (ActorDetail)dcjs.ReadObject(responseStream);
            }

            return actorDetail;
        }

        public async Task<MovieDetail> MovieDetailAsync(int id)
        {
            MovieDetail movieDetail = new MovieDetail();

            var query = $"{_appSettings.TmDbSettings.BaseUrl}/movie/{id}";
            var queryParams = new Dictionary<string, string>()
            {
                { "api_key" , _appSettings.MovieProSettings.TmDbApiKey },
                {"language", _appSettings.TmDbSettings.QueryOptions.Language },
                {"append_to_response", _appSettings.TmDbSettings.QueryOptions.AppendToResponse }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var dcjs = new DataContractJsonSerializer(typeof(MovieDetail));
                using var responseStream = await response.Content.ReadAsStreamAsync();
                movieDetail = (MovieDetail)dcjs.ReadObject(responseStream);
            }

            return movieDetail;
        }

        public async Task<MovieSearch> SearchMoviesAsync(MovieCategory category, int count)
        {
            MovieSearch movieSearch = new MovieSearch();

            var query = $"{_appSettings.TmDbSettings.BaseUrl}/movie/{category}";
            var queryParams = new Dictionary<string, string>()
            {
                { "api_key" , _appSettings.MovieProSettings.TmDbApiKey },
                {"language", _appSettings.TmDbSettings.QueryOptions.Language },
                {"page", _appSettings.TmDbSettings.QueryOptions.Page }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            var client = _httpClient.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var dcjs = new DataContractJsonSerializer(typeof(MovieSearch));
                using var responseStream = await response.Content.ReadAsStreamAsync();
                movieSearch = (MovieSearch)dcjs.ReadObject(responseStream);
                movieSearch.results = movieSearch.results.Take(count).ToArray();
                movieSearch.results.ToList().ForEach(r => r.poster_path = $"{_appSettings.TmDbSettings.BaseImagePath}/{_appSettings.MovieProSettings.DefaultPosterSize}/{r.poster_path}");
            }

            return movieSearch;
        }
    }
}
