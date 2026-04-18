namespace TriviaApp.Domain.Models.Responses
{
    public abstract record BaseResponse
    {
        public bool IsSuccess { get; set; } = true;
    }
}
