using System.Threading.Tasks;
using TriviaApp.Domain.Models.Requests;
using TriviaApp.Domain.Models.Responses;

namespace TriviaApp.Domain.Services
{
    public interface ITriviaService
    {
        public Task<GetTriviaConfigDataResponse> GetTriviaConfigData();
        public Task<GetQuestionsResponse> GetQuestions(GetQuestionsRequest request);
        Task<bool> CheckAnswers(CheckAnswersRequest request);
    }
}
