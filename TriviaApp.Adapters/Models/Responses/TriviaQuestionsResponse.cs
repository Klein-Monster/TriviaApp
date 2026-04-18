using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TriviaApp.Adapter.Models.Responses
{
    public record TriviaQuestionsResponse
    {
        [JsonPropertyName("response_code")]
        public int ResponseCode { get; init; }

        [JsonPropertyName("results")]
        public List<TriviaQuestion> Questions { get; init; }
    }
}
