using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TriviaApp.Adapter.Models.Responses
{
    public record TriviaCategoriesResponse
    {
        [JsonPropertyName("trivia_categories")]
        public List<TriviaCategory> Categories { get; init; }
    }
}
