using System.Text.Json.Serialization;

namespace MottuChallenge.API.DTOs
{
    public class UploadCnhRequestDTO
    {
        [JsonPropertyName("imagem_cnh")]
        public string CnhImageBase64 { get; set; }
    }
}