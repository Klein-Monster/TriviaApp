using System;
using System.Collections.Generic;
using TriviaApp.Domain.Models.Enums;

namespace TriviaApp.Domain.Models
{
    public record InternalTriviaQuestion
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Question { get; init; }
        public string Category { get; init; }
        public string CorrectAnswer { get; init; }
        public List<string> IncorrectAnswers { get; init; }
        public QuestionDifficulty Difficulty { get; init; }
        public QuestionType QuestionType { get; init; }
    }
}
