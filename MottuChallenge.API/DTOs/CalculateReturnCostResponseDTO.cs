using System.Text.Json.Serialization;

namespace MottuChallenge.API.DTOs
{
    public class CalculateReturnCostResponseDTO
    {
        [JsonPropertyName("custo_total")]
        public decimal TotalCost { get; set; }
        [JsonPropertyName("detalhes")]
        public string Details { get; set; }
    }
}