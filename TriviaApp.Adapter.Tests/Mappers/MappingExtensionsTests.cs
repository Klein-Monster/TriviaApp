using System;
using System.Linq;
using AutoFixture;
using TriviaApp.Adapter.Mappers;
using TriviaApp.Adapter.Models;
using TriviaApp.Adapter.Models.Enums;
using TriviaApp.Domain.Models;

namespace TriviaApp.Adapter.Tests.Mappers
{
    public class MappingExtensionsTests
    {
        //The use of AutoFixture makes the tests cleaner by only needing to focus on the properties
        //of an object which matter for the test.
        private readonly Fixture _fixture;

        public MappingExtensionsTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void MapTriviaQuestion_when_triviaquestion_is_null_returns_null()
        {
            //Arrange
            TriviaQuestion source = null;

            //Act
            InternalTriviaQuestion mappedQuestion = source.Map();

            //Assert
            Assert.Null(mappedQuestion);
        }

        [Fact]
        public void MapTriviaQuestion_when_difficulty_is_invalid_throws_argumentexception()
        {
            //Arrange
            var invalidEnumValue = (QuestionDifficulty)9999;
            var source = _fixture.Build<TriviaQuestion>()
                .With(x => x.Difficulty, invalidEnumValue)
                .Create();

            // Act
            var exception = Assert.Throws<ArgumentException>(() => source.Map());

            // Assert
            Assert.Contains("Invalid enum value for QuestionDifficulty", exception.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void MapTriviaQuestion_when_type_is_invalid_throws_argumentexception()
        {
            //Arrange
            var invalidEnumValue = (QuestionType)9999;
            var source = _fixture.Build<TriviaQuestion>()
                .With(x => x.QuestionType, invalidEnumValue)
                .Create();

            // Act
            var exception = Assert.Throws<ArgumentException>(() => source.Map());

            // Assert
            Assert.Contains("Invalid enum value for QuestionType", exception.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void MapTriviaQuestion_when_source_is_valid_maps_all_properties_correctly()
        {
            //Arrange
            var source = _fixture.Create<TriviaQuestion>();

            // Act
            var mappedQuestion = source.Map();

            // Assert
            Assert.Equal(source.Question, mappedQuestion.Question);
            Assert.Equal(source.Category, mappedQuestion.Category);

            //I use the Map() methods because they should be tested in separate unit tests.
            //Testing these enum mappings are not in scope for this unit test.
            Assert.Equal(source.Difficulty.Map(), mappedQuestion.Difficulty);
            Assert.Equal(source.QuestionType.Map(), mappedQuestion.QuestionType);

            Assert.Equal(source.CorrectAnswer, mappedQuestion.CorrectAnswer);
            Assert.Equal(source.IncorrectAnswers.OrderBy(x => x), mappedQuestion.IncorrectAnswers.OrderBy(x => x));
        }
    }
}
