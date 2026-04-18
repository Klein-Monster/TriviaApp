using System.Text.Json.Serialization;
using TriviaApp.Adapter.Models.Enums;

namespace TriviaApp.Adapter.Models
{
    public record TriviaQuestion
    {
        [JsonPropertyName("question")]
        public string Question { get; init; }

        [JsonPropertyName("category")]
        public string Category { get; init; }

        [JsonPropertyName("correct_answer")]
        public string CorrectAnswer { get; init; }

        [JsonPropertyName("incorrect_answers")]
        public string[] IncorrectAnswers { get; init; }

        [JsonPropertyName("difficulty")]
        public QuestionDifficulty Difficulty { get; init; }

        [JsonPropertyName("type")]
        public QuestionType QuestionType { get; init; }
    }
}
