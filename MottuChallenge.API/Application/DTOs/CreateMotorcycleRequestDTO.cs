using System.Text.Json.Serialization;

namespace MottuChallenge.API.DTOs
{
    public class CreateMotorcycleRequestDTO
    {
        [JsonPropertyName("ano")]
        public int Year { get; set; }
        [JsonPropertyName("modelo")]
        public string Model { get; set; } = string.Empty;
        [JsonPropertyName("placa")]
        public string LicensePlate { get; set; } = string.Empty;
    }
}