using System.Threading.Tasks;
using TriviaApp.Domain.Models.Requests;
using TriviaApp.Domain.Models.Responses;

namespace TriviaApp.Domain.Services
{
    public interface ITriviaDataProvider
    {
        public Task<ProvideCategoriesResponse> ProvideCategories();
        public Task<ProvideQuestionsResponse> ProvideQuestions(GetQuestionsRequest request);
        Task<bool> CheckAnswers(CheckAnswersRequest request);
    }
}