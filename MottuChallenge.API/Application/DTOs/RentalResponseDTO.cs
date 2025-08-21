using System.Text.Json.Serialization;

namespace MottuChallenge.API.DTOs
{
    public class RentalResponseDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("entregador_id")]
        public Guid DeliveryPersonIdentifier { get; set; }
        [JsonPropertyName("moto_id")]
        public Guid MotorcycleIdentifier { get; set; }
        [JsonPropertyName("data_inicio")]
        public DateTime StartDate { get; set; }
        [JsonPropertyName("data_termino")]
        public DateTime EndDate { get; set; }
        [JsonPropertyName("data_previsao_termino")]
        public DateTime PredictedEndDate { get; set; }
        [JsonPropertyName("plano")]
        public int PlanDays { get; set; }
        [JsonPropertyName("custo_total")]
        public decimal TotalCost { get; set; }
        [JsonPropertyName("ativo")]
        public bool IsActive { get; set; }
    }
}