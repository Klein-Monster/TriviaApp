using System.Collections.Generic;

namespace TriviaApp.Domain.Models.Requests
{
    public record CheckAnswersRequest
    {
        public List<AnsweredQuestion> AnsweredQuestions { get; init; }
    }
}
