using System;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using Telegram.Bot.Requests.Abstractions;

namespace TvShowBot.Classes
{
    public class FilmsSearcher
    {
        private HttpClient httpClient { get; set; }
        private readonly string xApiKey = "8e7e7949-2fb0-473a-b8a0-cc1e460b0ca0";

        public FilmsSearcher()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", xApiKey);
        }

        public async Task<string> GetFilmAsync(string filmName)
        {
            JObject joResponse = await GetJsonAsString($"https://kinopoiskapiunofficial.tech/api/v2.1/films/search-by-keyword?keyword={filmName}");

            var filmId = joResponse.SelectToken("$.films.[0].filmId").ToString();

            joResponse = await GetJsonAsString($"https://kinopoiskapiunofficial.tech/api/v2.2/films/{filmId}");

            StringBuilder sb = new StringBuilder()
                .Append("<b>")
                .Append(joResponse.SelectToken("$.nameRu").ToString())
                .Append("</b>\n");

            if (joResponse.SelectToken("$.ratingImdb") != null)
            {
                sb.Append("\nОценка: " + joResponse.SelectToken("$.ratingImdb").ToString());
            }
            if (joResponse.SelectToken("$.webUrl") != null)
            {
                sb.Append("\nКинопоиск: " + joResponse.SelectToken("$.webUrl").ToString());
            }
            if (joResponse.SelectToken("$.startYear") != null)
            {
                sb.Append("\nДата начала: " + joResponse.SelectToken("$.startYear").ToString());
            }
            if (joResponse.SelectToken("$.endYear") != null)
            {
                sb.Append("\nДата конца: " + joResponse.SelectToken("$.endYear").ToString());
            }
            if (joResponse.SelectToken("$.description") != null)
            {
                sb.Append("\nОписание: " + joResponse.SelectToken("$.description").ToString());
            }

            return sb.ToString();
        }

        private async Task<JObject> GetJsonAsString(string path)
        {
            HttpResponseMessage response = (await httpClient.GetAsync(path)).EnsureSuccessStatusCode();
            var responseBody =  await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseBody);
        }
    }
}

