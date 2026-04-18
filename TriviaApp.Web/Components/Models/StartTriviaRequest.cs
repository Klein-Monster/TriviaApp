using System.ComponentModel.DataAnnotations;

namespace TriviaApp.Web.Components.Models
{
    public class StartTriviaRequest
    {
        [Range(1, 50, ErrorMessage = "Amount must be between 1 and 50.")]
        public int Amount { get; set; } = 1;
        public int? CategoryId { get; set; }
        public string Difficulty { get; set; }
        public string Type { get; set; }
    }
}
