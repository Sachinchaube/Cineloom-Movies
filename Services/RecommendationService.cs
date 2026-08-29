using System.Text;
using System.Text.Json;
using MovieBookingPro.Models;

namespace MovieBookingPro.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<RecommendationService> _logger;

        public RecommendationService(HttpClient httpClient, IConfiguration config, ILogger<RecommendationService> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
        }

        public async Task<List<Movie>> GetRecommendationsAsync(Movie currentMovie, List<Movie> allMovies)
        {
            var candidates = allMovies.Where(m => m.MovieId != currentMovie.MovieId).ToList();

            if (candidates.Count == 0)
            {
                return new List<Movie>();
            }

            var apiKey = _config["OpenRouter:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey) || apiKey == "YOUR_OPENROUTER_API_KEY_HERE")
            {
                _logger.LogWarning("OpenRouter API key not configured. Falling back to genre-based recommendations.");
                return FallbackRecommendations(currentMovie, candidates);
            }

            try
            {
                var movieListText = string.Join("\n", candidates.Select(m =>
                    $"- ID:{m.MovieId} | Title: {m.Title} | Genre: {m.Genre} | Language: {m.Language}"));

                var prompt = $@"A user is currently viewing the movie ""{currentMovie.Title}"" (Genre: {currentMovie.Genre}, Language: {currentMovie.Language}).

From the list below, pick up to 4 movies that this user would most likely also enjoy, based on genre and language similarity.

Movie list:
{movieListText}

Respond ONLY with a JSON array of the chosen MovieId integers, e.g. [3,7,1,9]. No explanation, no markdown, just the JSON array.";

                var requestBody = new
                {
                    model = "meta-llama/llama-3.1-8b-instruct",
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 200
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/chat/completions")
                {
                    Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
                };
                request.Headers.Add("Authorization", $"Bearer {apiKey}");

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("OpenRouter API returned {StatusCode}. Falling back.", response.StatusCode);
                    return FallbackRecommendations(currentMovie, candidates);
                }

                var responseText = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseText);

                var content = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "[]";

                // Strip any markdown fencing the model might add despite instructions
                content = content.Trim().Trim('`').Replace("json", "", StringComparison.OrdinalIgnoreCase).Trim();

                var startIdx = content.IndexOf('[');
                var endIdx = content.LastIndexOf(']');
                if (startIdx == -1 || endIdx == -1 || endIdx <= startIdx)
                {
                    return FallbackRecommendations(currentMovie, candidates);
                }

                var jsonArray = content.Substring(startIdx, endIdx - startIdx + 1);
                var ids = JsonSerializer.Deserialize<List<int>>(jsonArray) ?? new List<int>();

                var recommended = candidates.Where(m => ids.Contains(m.MovieId)).ToList();

                return recommended.Count > 0 ? recommended : FallbackRecommendations(currentMovie, candidates);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "AI recommendation call failed. Falling back to genre-based recommendations.");
                return FallbackRecommendations(currentMovie, candidates);
            }
        }

        private List<Movie> FallbackRecommendations(Movie currentMovie, List<Movie> candidates)
        {
            var sameGenre = candidates
                .Where(m => m.Genre.Equals(currentMovie.Genre, StringComparison.OrdinalIgnoreCase))
                .Take(4)
                .ToList();

            if (sameGenre.Count >= 1)
            {
                return sameGenre;
            }

            return candidates.Take(4).ToList();
        }
    }
}
