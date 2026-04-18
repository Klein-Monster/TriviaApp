using System.Text.Json.Serialization;

namespace TriviaApp.Adapter.Models
{
    public record TriviaCategory
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }
    }
}
