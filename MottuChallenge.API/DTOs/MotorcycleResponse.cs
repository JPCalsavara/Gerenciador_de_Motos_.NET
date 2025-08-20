using System.Text.Json.Serialization;

namespace MottuChallenge.API.DTOs
{
    /// <summary>
    /// DTO para representar os dados de uma moto na resposta da API.
    /// </summary>
    public class MotorcycleResponse
    {
        
        [JsonPropertyName("identificador")]
        public string Identifier { get; set; }
        
        [JsonPropertyName("ano")]
        public int Year { get; set; }
        
        [JsonPropertyName("modelo")]
        public string Model { get; set; }
        
        [JsonPropertyName("placa")]
        public string LicensePlate { get; set; }
    }
}