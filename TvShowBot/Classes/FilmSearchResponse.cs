using System;
namespace TvShowBot
{
    public class FilmSearchResponse
    {
        public string? keyword { get; set; }
        public int pagesCount { get; set; }
        public int searchFilmsCountResult { get; set; }
        public FilmSearchResponse_films[]? films { get; set; }
    }
}

