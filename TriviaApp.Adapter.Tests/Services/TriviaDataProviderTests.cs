using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using TriviaApp.Adapter.Models;
using TriviaApp.Adapter.Models.Responses;
using TriviaApp.Adapter.Services;
using TriviaApp.Domain.Models;
using TriviaApp.Domain.Models.Requests;

namespace TriviaApp.Adapter.Tests.Services
{
    public class TriviaDataProviderTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IQuestionsMemoryCache> _memoryCacheMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly TriviaDataProvider _triviaDataProvider;
        private readonly TriviaAdapterSettings _triviaAdapterSettings;

        public TriviaDataProviderTests()
        {
            _fixture = new Fixture();
            _memoryCacheMock = new();
            _httpMessageHandlerMock = new();

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            _triviaAdapterSettings = new TriviaAdapterSettings
            {
                GetCategoriesURL = "https://testcategoriesapi.com/categories",
                GetQuestionsBaseURL = "https://testquestionsapi.com/questions"
            };

            var triviaAdapterOptions = Options.Create(_triviaAdapterSettings);

            _triviaDataProvider = new TriviaDataProvider(httpClient, triviaAdapterOptions, _memoryCacheMock.Object);
        }

        #region CheckAnswers

        [Fact]
        public async Task CheckAnswers_when_all_answers_are_correct_returns_true()
        {
            // Arrange
            var cachedQuestions = _fixture.CreateMany<InternalTriviaQuestion>();

            foreach (var cachedQuestion in cachedQuestions)
                _memoryCacheMock.Setup(x => x.Get(cachedQuestion.Id))
                    .Returns(cachedQuestion);

            var correctlyAnsweredQuestions = cachedQuestions
                                                .Select(x => new AnsweredQuestion
                                                {
                                                    QuestionId = x.Id,
                                                    Answer = x.CorrectAnswer
                                                }).ToList();

            var request = _fixture.Build<CheckAnswersRequest>()
                            .With(x => x.AnsweredQuestions, correctlyAnsweredQuestions)
                            .Create();

            // Act
            var result = await _triviaDataProvider.CheckAnswers(request);

            // Assert
            Assert.True(result);
        }

        [InlineData(1, 3)]
        [InlineData(2, 3)]
        [InlineData(3, 3)]
        [Theory]
        public async Task CheckAnswers_when_any_answer_is_incorrect_return_false(int numberOfIncorrectAnswers, int totalNumberOfAnswers)
        {
            // Arrange
            var cachedQuestions = _fixture.CreateMany<InternalTriviaQuestion>(totalNumberOfAnswers);

            foreach (var cachedQuestion in cachedQuestions)
                _memoryCacheMock.Setup(x => x.Get(cachedQuestion.Id))
                    .Returns(cachedQuestion);

            var correctlyAnsweredQuestions = cachedQuestions
                                                .Select((question, index) => new AnsweredQuestion
                                                {
                                                    QuestionId = question.Id,
                                                    Answer = index < numberOfIncorrectAnswers
                                                        ? "WRONG ANSWER"
                                                        : question.CorrectAnswer
                                                }).ToList();

            var request = _fixture.Build<CheckAnswersRequest>()
                            .With(x => x.AnsweredQuestions, correctlyAnsweredQuestions)
                            .Create();

            // Act
            var result = await _triviaDataProvider.CheckAnswers(request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckAnswers_when_question_cannot_be_found_in_cache_return_false()
        {
            // Arrange
            var cachedQuestions = _fixture.CreateMany<InternalTriviaQuestion>();

            InternalTriviaQuestion returnValueForCache = null;

            foreach (var cachedQuestion in cachedQuestions)
                _memoryCacheMock.Setup(x => x.Get(cachedQuestion.Id))
                    .Returns(returnValueForCache);

            var correctlyAnsweredQuestions = cachedQuestions
                                                .Select(x => new AnsweredQuestion
                                                {
                                                    QuestionId = x.Id,
                                                    Answer = x.CorrectAnswer
                                                }).ToList();

            var request = _fixture.Build<CheckAnswersRequest>()
                            .With(x => x.AnsweredQuestions, correctlyAnsweredQuestions)
                            .Create();

            // Act
            var result = await _triviaDataProvider.CheckAnswers(request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckAnswers_when_request_is_null_return_false()
        {
            // Arrange
            CheckAnswersRequest request = null;

            // Act
            var result = await _triviaDataProvider.CheckAnswers(request);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region ProvideQuestions

        [Fact]
        public async Task ProvideQuestions_when_request_is_null_return_IsSuccess_false()
        {
            // Arrange
            GetQuestionsRequest request = null;

            // Act
            var result = await _triviaDataProvider.ProvideQuestions(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task ProvideQuestions_when_category_is_missing_the_url_does_not_contain_it()
        {
            // Arrange
            var request = _fixture.Build<GetQuestionsRequest>()
                .Without(x => x.CategoryId)
                .Create();

            // Act
            var result = await _triviaDataProvider.ProvideQuestions(request);

            // Assert
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => !req.RequestUri.ToString().Contains("category")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task ProvideQuestions_when_difficulty_is_missing_the_url_does_not_contain_it()
        {
            // Arrange
            var request = _fixture.Build<GetQuestionsRequest>()
                .Without(x => x.Difficulty)
                .Create();

            // Act
            var result = await _triviaDataProvider.ProvideQuestions(request);

            // Assert
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => !req.RequestUri.ToString().Contains("difficulty")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task ProvideQuestions_when_type_is_missing_the_url_does_not_contain_it()
        {
            // Arrange
            var request = _fixture.Build<GetQuestionsRequest>()
                .Without(x => x.Type)
                .Create();

            // Act
            var result = await _triviaDataProvider.ProvideQuestions(request);

            // Assert
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => !req.RequestUri.ToString().Contains("type")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task ProvideQuestions_when_category_is_present_the_url_does_contain_it()
        {
            // Arrange
            var request = _fixture.Create<GetQuestionsRequest>();

            // Act
            var result = await _triviaDataProvider.ProvideQuestions(request);

            // Assert
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("category")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task ProvideQuestions_when_difficulty_is_present_the_url_does_contain_it()
        {
            // Arrange
            var request = _fixture.Create<GetQuestionsRequest>();

            // Act
            var result = await _triviaDataProvider.ProvideQuestions(request);

            // Assert
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("difficulty")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task ProvideQuestions_when_type_is_present_the_url_does_contain_it()
        {
            // Arrange
            var request = _fixture.Create<GetQuestionsRequest>();

            // Act
            var result = await _triviaDataProvider.ProvideQuestions(request);

            // Assert
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("type")),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task ProvideQuestions_when_valid_request_is_sent_url_contains_correct_parameters()
        {
            // Arrange
            var request = _fixture.Create<GetQuestionsRequest>();
            var getCapturedRequest = SetupGetQuestionsRequest();

            // Act
            await _triviaDataProvider.ProvideQuestions(request);

            // Assert
            var capturedRequest = getCapturedRequest();

            Assert.NotNull(capturedRequest);

            var query = QueryHelpers.ParseQuery(capturedRequest.RequestUri.Query);

            Assert.Equal(request.Amount.ToString(), query["amount"]);
            Assert.Equal(request.CategoryId.Value.ToString(), query["category"]);
            Assert.Equal(request.Difficulty.ToLower(), query["difficulty"]);
            Assert.Equal(request.Type.ToLower(), query["type"]);
        }

        [Fact]
        public async Task ProvideQuestions_when_valid_request_is_sent_returns_correct_questions()
        {
            // Arrange
            var request = _fixture.Create<GetQuestionsRequest>();
            var getQuestionsResponse = _fixture.Build<TriviaQuestionsResponse>()
                                        .With(x => x.ResponseCode, 0)
                                        .Create();

            SetupGetQuestionsRequest(getQuestionsResponse);

            // Act
            var result = await _triviaDataProvider.ProvideQuestions(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            Assert.NotNull(result.Questions);
            Assert.NotEmpty(result.Questions);

            Assert.Equal(getQuestionsResponse.Questions.Count, result.Questions.Count);

            foreach (var question in getQuestionsResponse.Questions)
                Assert.Single(result.Questions, x => x.Question.Equals(question.Question, StringComparison.Ordinal));
        }

        #endregion

        #region Helper methods

        private Func<HttpRequestMessage> SetupGetQuestionsRequest(TriviaQuestionsResponse response = null)
        {
            response ??= _fixture.Create<TriviaQuestionsResponse>();

            HttpRequestMessage capturedRequest = null;

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri.ToString().Contains(_triviaAdapterSettings.GetQuestionsBaseURL)
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Callback<HttpRequestMessage, CancellationToken>((req, _) =>
                {
                    capturedRequest = req;
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(
                        JsonSerializer.Serialize(response),
                        Encoding.UTF8,
                        "application/json")
                });

            return () => capturedRequest;
        }

        #endregion
    }
}
