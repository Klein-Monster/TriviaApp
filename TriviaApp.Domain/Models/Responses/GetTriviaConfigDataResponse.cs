using System.Collections.Generic;

namespace TriviaApp.Domain.Models.Responses
{
    public record GetTriviaConfigDataResponse : BaseResponse
    {
        public List<Category> Categories { get; init; }
        public List<string> Difficulties { get; init; }
        public List<string> Types { get; init; }
    }
}
