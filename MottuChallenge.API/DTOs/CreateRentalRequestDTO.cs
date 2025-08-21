using System.Text.Json.Serialization;

namespace MottuChallenge.API.DTOs
{
    public class CreateRentalRequestDTO
    {   
       
        [JsonPropertyName("entregador_id")]
        public Guid DeliveryPersonId { get; set; }

        [JsonPropertyName("moto_id")]
        public Guid MotorcycleId { get; set; }

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