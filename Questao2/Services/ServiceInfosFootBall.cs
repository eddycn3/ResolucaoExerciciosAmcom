using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Questao2.Dtos;
using Questao2.Services.Interfaces;

namespace Questao2.Services
{
    public class ServiceInfosFootballMatches : IServiceInfosFootballMatches
    {
        public readonly string UrlService = "https://jsonmock.hackerrank.com/api/football_matches";
        private readonly HttpClient _httpClient;
        public ServiceInfosFootballMatches(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(UrlService);
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<FootballMatchDTO>> GetFootballMatches(int ano, string time)
        {
            var response = await _httpClient.GetAsync($"?year={ano}&team1={time}");
            var matchs = await DeserializarObjetoResponse<IEnumerable<FootballMatchDTO>>(response);
            return matchs;
        }

        private async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var jsonString = await responseMessage.Content.ReadAsStringAsync();

            var rootElement = JsonSerializer.Deserialize<JsonElement>(jsonString);
            var dataElement = rootElement.GetProperty("data");

            return JsonSerializer.Deserialize<T>(dataElement, options);
        }
    }
}
