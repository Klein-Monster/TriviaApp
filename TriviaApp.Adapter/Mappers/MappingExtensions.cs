using System;
using System.Collections.Generic;
using System.Linq;
using TriviaApp.Domain.Models;
using TriviaApp.Domain.Models.Enums;
using TriviaApp.Domain.Models.Responses;

namespace TriviaApp.Adapter.Mappers
{
    public static class MappingExtensions
    {
        public static InternalTriviaQuestion Map(this Models.TriviaQuestion source)
        {
            if (source == null)
                return null;

            return new InternalTriviaQuestion
            {
                Question = source.Question,
                Difficulty = source.Difficulty.Map(),
                QuestionType = source.QuestionType.Map(),
                Category = source.Category,
                CorrectAnswer = source.CorrectAnswer,
                IncorrectAnswers = [.. source.IncorrectAnswers]
            };
        }

        public static List<InternalTriviaQuestion> Map(this List<Models.TriviaQuestion> source) =>
            [.. source?.Select(x => x.Map())];

        public static ProvideQuestionsResponse Map(this Models.Responses.TriviaQuestionsResponse source)
        {
            if (source == null)
                return null;

            return new ProvideQuestionsResponse
            {
                IsSuccess = source.ResponseCode == 0,
                Questions = source.Questions.Map()
            };
        }

        public static QuestionDifficulty Map(this Models.Enums.QuestionDifficulty source)
        {
            return source switch
            {
                Models.Enums.QuestionDifficulty.Easy => QuestionDifficulty.Easy,
                Models.Enums.QuestionDifficulty.Medium => QuestionDifficulty.Medium,
                Models.Enums.QuestionDifficulty.Hard => QuestionDifficulty.Hard,
                _ => throw new ArgumentException("Invalid enum value for QuestionDifficulty", nameof(source))
            };
        }

        public static QuestionType Map(this Models.Enums.QuestionType source)
        {
            return source switch
            {
                Models.Enums.QuestionType.Multiple => QuestionType.Multiple,
                Models.Enums.QuestionType.Boolean => QuestionType.Boolean,
                _ => throw new ArgumentException("Invalid enum value for QuestionType", nameof(source))
            };
        }
    }
}
