using System.Text.Json.Serialization;
using MottuChallenge.API.Entities;

namespace MottuChallenge.API.DTOs
{
    public class DeliveryPersonResponseDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("nome")]
        public string Name { get; set; }

        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; }

        [JsonPropertyName("data_nascimento")]
        public DateOnly BirthDate { get; set; }

        [JsonPropertyName("numero_cnh")]
        public string CnhNumber { get; set; }

        [JsonPropertyName("tipo_cnh")]
        public string CnhType { get; set; }

        [JsonPropertyName("imagem_cnh_url")]
        public string? CnhImageUrl { get; set; }
    }
}