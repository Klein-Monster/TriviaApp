namespace TriviaApp.Domain.Models.Requests
{
    public record GetQuestionsRequest
    {
        public int Amount { get; init; } = 1;
        public int? CategoryId { get; init; }
        public string Difficulty { get; init; }
        public string Type { get; init; }
    }
}
