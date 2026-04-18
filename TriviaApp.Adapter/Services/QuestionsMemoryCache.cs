using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using TriviaApp.Domain.Models;

namespace TriviaApp.Adapter.Services
{
    public class QuestionsMemoryCache(IMemoryCache cache) : IQuestionsMemoryCache
    {
        public void Set(List<InternalTriviaQuestion> questions)
        {
            foreach (var question in questions)
                cache.Set(question.Id, question);
        }

        public InternalTriviaQuestion Get(Guid id)
        {
            cache.TryGetValue(id, out InternalTriviaQuestion question);
            return question;
        }
    }
}
