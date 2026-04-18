using System;

namespace TriviaApp.Domain.Models
{
    public record AnsweredQuestion
    {
        public Guid QuestionId { get; init; }
        public string Answer { get; init; }
    }
}
