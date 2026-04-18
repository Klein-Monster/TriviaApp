using System.Collections.Generic;

namespace TriviaApp.Domain.Models.Responses
{
    public record ProvideQuestionsResponse : BaseResponse
    {
        public List<InternalTriviaQuestion> Questions { get; init; }
    }
}
