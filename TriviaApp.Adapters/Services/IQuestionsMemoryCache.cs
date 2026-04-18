using System;
using System.Collections.Generic;
using TriviaApp.Domain.Models;

namespace TriviaApp.Adapter.Services
{
    public interface IQuestionsMemoryCache
    {
        void Set(List<InternalTriviaQuestion> questions);
        InternalTriviaQuestion Get(Guid id);
    }
}
