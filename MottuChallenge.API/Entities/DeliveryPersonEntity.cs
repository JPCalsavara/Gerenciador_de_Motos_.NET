using System.ComponentModel.DataAnnotations;

namespace MottuChallenge.API.Entities
{
    public class DeliveryPersonEntity
    {
        [Key] // Define o Identifier como chave primária
        public string Identifier { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string CnhNumber { get; set; } = string.Empty;
        public CnhType CnhType { get; set; }
        public string? CnhImageUrl { get; set; } // Pode ser nulo inicialmente
    }
}