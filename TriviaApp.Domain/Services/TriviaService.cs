using System;
using System.Linq;
using System.Threading.Tasks;
using TriviaApp.Domain.Mappers;
using TriviaApp.Domain.Models.Enums;
using TriviaApp.Domain.Models.Requests;
using TriviaApp.Domain.Models.Responses;

namespace TriviaApp.Domain.Services
{
    public class TriviaService(ITriviaDataProvider dataProvider) : ITriviaService
    {
        public async Task<GetQuestionsResponse> GetQuestions(GetQuestionsRequest request)
        {
            if (request.Amount < 1 || request.Amount > 50)
                throw new ArgumentOutOfRangeException(nameof(request), request.Amount, "Amount must be between 1 and 50.");

            try
            {
                var questionsResponse = await dataProvider.ProvideQuestions(request);

                return new GetQuestionsResponse { IsSuccess = questionsResponse.IsSuccess, Questions = questionsResponse.Questions.Map() };
            }
            catch
            {
                return new GetQuestionsResponse { IsSuccess = false };
            }
        }

        public async Task<GetTriviaConfigDataResponse> GetTriviaConfigData()
        {
            try
            {
                var categoriesResponse = await dataProvider.ProvideCategories();
                var difficulties = Enum.GetNames<QuestionDifficulty>().ToList();
                var types = Enum.GetNames<QuestionType>().ToList();

                return new GetTriviaConfigDataResponse
                {
                    Categories = categoriesResponse.Categories,
                    Difficulties = difficulties,
                    Types = types,
                    IsSuccess = categoriesResponse.IsSuccess
                };
            }
            catch
            {
                return new GetTriviaConfigDataResponse { IsSuccess = false };
            }
        }

        public async Task<bool> CheckAnswers(CheckAnswersRequest request)
        {
            try
            {
                return await dataProvider.CheckAnswers(request);
            }
            catch
            {
                return false;
            }
        }
    }
}
