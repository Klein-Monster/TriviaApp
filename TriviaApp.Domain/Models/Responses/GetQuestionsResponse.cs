using System.Collections.Generic;

namespace TriviaApp.Domain.Models.Responses
{
    public record GetQuestionsResponse : BaseResponse
    {
        public List<PublicTriviaQuestion> Questions { get; init; }
    }
}
