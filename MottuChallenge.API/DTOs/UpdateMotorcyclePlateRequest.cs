using System.Text.Json.Serialization;

namespace MottuChallenge.API.DTOs
{
    /// <summary>
    /// DTO para a requisição de atualização da placa de uma moto.
    /// </summary>
    public class UpdateMotorcyclePlateRequest
    {
        [JsonPropertyName("placa")]
        public string LicensePlate { get; set; }
    }
}