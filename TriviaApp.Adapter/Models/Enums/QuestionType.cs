using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TriviaApp.Adapter.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum QuestionType
    {
        [JsonStringEnumMemberName("multiple")]
        Multiple,
        [JsonStringEnumMemberName("boolean")]
        Boolean
    }
}
