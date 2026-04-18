using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using TriviaApp.Adapter.Mappers;
using TriviaApp.Adapter.Models;
using TriviaApp.Adapter.Models.Responses;
using TriviaApp.Domain.Models;
using TriviaApp.Domain.Models.Requests;
using TriviaApp.Domain.Models.Responses;
using TriviaApp.Domain.Services;

namespace TriviaApp.Adapter.Services
{
    public class TriviaDataProvider(HttpClient httpClient, IOptions<TriviaAdapterSettings> options, IQuestionsMemoryCache memoryCache) : ITriviaDataProvider
    {
        private readonly TriviaAdapterSettings _settings = options.Value;

        public async Task<ProvideCategoriesResponse> ProvideCategories()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<TriviaCategoriesResponse>(_settings.GetCategoriesURL);

                var categories = response.Categories?.Select(x =>
                    new Category
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();

                return new ProvideCategoriesResponse { Categories = categories };
            }
            catch
            {
                return new ProvideCategoriesResponse { IsSuccess = false };
            }
        }

        public async Task<ProvideQuestionsResponse> ProvideQuestions(GetQuestionsRequest request)
        {
            if (request == null)
                return new ProvideQuestionsResponse { IsSuccess = false };

            try
            {
                var requestURL = BuildGetQuestionsURL(request);

                var response = await httpClient.GetFromJsonAsync<TriviaQuestionsResponse>(requestURL);

                var mappedResponse = response.Map();

                memoryCache.Set(mappedResponse.Questions);

                return mappedResponse;
            }
            catch
            {
                return new ProvideQuestionsResponse { IsSuccess = false };
            }
        }

        public async Task<bool> CheckAnswers(CheckAnswersRequest request)
        {
            if (request == null)
                return false;

            try
            {
                foreach (var answeredQuestion in request.AnsweredQuestions)
                {
                    var cachedQuestion = memoryCache.Get(answeredQuestion.QuestionId);

                    if (cachedQuestion == null || !AnswerIsEqual(cachedQuestion.CorrectAnswer, answeredQuestion.Answer))
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool AnswerIsEqual(string correctAnswer, string answer) =>
            string.Equals(correctAnswer, answer, System.StringComparison.OrdinalIgnoreCase);

        private string BuildGetQuestionsURL(GetQuestionsRequest request)
        {
            var baseUrl = _settings.GetQuestionsBaseURL;

            var queryParams = new Dictionary<string, string>
            {
                ["amount"] = request.Amount.ToString()
            };

            if (request.CategoryId.HasValue)
                queryParams["category"] = request.CategoryId.Value.ToString();

            if (!string.IsNullOrWhiteSpace(request.Difficulty))
                queryParams["difficulty"] = request.Difficulty.ToLower();

            if (!string.IsNullOrWhiteSpace(request.Type))
                queryParams["type"] = request.Type.ToLower();

            return QueryHelpers.AddQueryString(baseUrl, queryParams);
        }
    }
}
