using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TriviaApp.Domain.Models;

namespace TriviaApp.Domain.Mappers
{
    public static class MappingExtensions
    {
        public static PublicTriviaQuestion Map(this InternalTriviaQuestion source)
        {
            if (source == null)
                return null;

            return new PublicTriviaQuestion
            {
                Id = source.Id,
                Question = WebUtility.HtmlDecode(source.Question),
                Difficulty = source.Difficulty,
                QuestionType = source.QuestionType,
                Category = source.Category,
                AnswerOptions = BuildAnswers(source)
            };
        }

        public static List<PublicTriviaQuestion> Map(this List<InternalTriviaQuestion> source) =>
            [.. source?.Select(x => x.Map())];

        /// <summary>
        /// Reorder the list randomly so the correct answer has a different index every time when question type is 'Multiple'.
        /// When the type is 'Boolean' we want the same order 'True', 'False' every time for consistency.
        /// The answers are decoded so the front-end can show them.
        /// This is business logic and shouldn't be in a mapper. The mapper should be thin, but for the sake of
        /// not over-complicating this small project I chose to put it in here for now.
        /// </summary>
        /// <param name="source">Source question</param>
        /// <returns>Returns list of combined answers</returns>
        private static List<string> BuildAnswers(InternalTriviaQuestion source)
        {
            var answers = source.IncorrectAnswers
                .Append(source.CorrectAnswer)
                .Select(WebUtility.HtmlDecode);

            if (source.QuestionType == Models.Enums.QuestionType.Boolean)
                return [.. answers.OrderByDescending(x => x)];

            return [.. answers.OrderBy(_ => Random.Shared.Next())];
        }
    }
}
