using System.Text.Json.Serialization;

namespace MottuChallenge.API.DTOs
{
    public class CreateRentalRequest
    {   
        [JsonPropertyName("identificador")]
        public string Identifier { get; set; }
        
        [JsonPropertyName("entregador_id")]
        public string DeliveryPersonIdentifier { get; set; }

        [JsonPropertyName("moto_id")]
        public string MotorcycleIdentifier { get; set; }

        [JsonPropertyName("data_inicio")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("data_termino")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("data_previsao_termino")]
        public DateTime PredictedEndDate { get; set; }

        [JsonPropertyName("plano")]
        public int PlanDays { get; set; }
    }
}