namespace TvShowBot
{
    public class FilmSearchResponse_films
    {
        public int filmId { get; set; }
        public string? nameRu { get; set; }
        public string? nameEn { get; set; }
        public string? type { get; set; }
        public string? year { get; set; }
        public string? description { get; set; }
        public string? filmLength { get; set; }
        public Country[]? countries { get; set; }
        public Genre[]? genres { get; set; }
        public string? rating { get; set; }
        public int ratingVoteCount { get; set; }
        public string? posterUrl { get; set; }
        public string? posterUrlPreview { get; set; }
    }
}