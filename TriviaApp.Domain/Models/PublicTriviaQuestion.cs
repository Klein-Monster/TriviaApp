using System;
using System.Collections.Generic;
using TriviaApp.Domain.Models.Enums;

namespace TriviaApp.Domain.Models
{
    public record PublicTriviaQuestion
    {
        public Guid Id { get; init; }
        public string Question { get; init; }
        public string Category { get; init; }
        public List<string> AnswerOptions { get; init; }
        public QuestionDifficulty Difficulty { get; init; }
        public QuestionType QuestionType { get; init; }
    }
}
