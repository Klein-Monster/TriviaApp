using System.Collections.Generic;

namespace TriviaApp.Domain.Models.Responses
{
    public record ProvideCategoriesResponse : BaseResponse
    {
        public List<Category> Categories { get; init; }
    }
}
