using System;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using Telegram.Bot.Requests.Abstractions;
using System.IO.Pipes;
using System.Data;

namespace TvShowBot.Classes
{
    public class FilmsSearcher
    {
        private HttpClient httpClient { get; set; }
        private IDictionary<string, string> _keys;

        public FilmsSearcher()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _keys = Program.GetKeys();
            string api_token;
            if(!_keys.TryGetValue("api_token",out api_token))
            {
                Console.WriteLine("Can't find api_token");
                Environment.Exit(1);
            }    
            httpClient.DefaultRequestHeaders.Add("X-API-KEY", api_token);
        }

        public async Task<string> GetFilmAsync(string filmName)
        {
            string search_by_keyboard_path;
            if (!_keys.TryGetValue("search_by_keyboard_path", out search_by_keyboard_path))
            {
                Console.WriteLine("Can't find search_by_keyboard_path");
                Environment.Exit(2);
            }

            search_by_keyboard_path += filmName;
            JObject joResponse = await GetJsonAsString(search_by_keyboard_path);
            var filmId = joResponse.SelectToken("$.films.[0].filmId").ToString();

            string search_by_id;
            if (!_keys.TryGetValue("search_by_id", out search_by_id))
            {
                Console.WriteLine("Can't find search_by_id");
                Environment.Exit(3);
            }
            search_by_id += filmId;
            joResponse = await GetJsonAsString(search_by_id);
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

