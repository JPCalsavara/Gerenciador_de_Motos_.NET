using System.Text.Json.Serialization;

namespace MottuChallenge.API.DTOs
{
    public class UpdateRentalReturnDateRequestDTO
    {
        [JsonPropertyName("data_devolucao")]
        public DateTime ReturnDate { get; set; }
    }
}