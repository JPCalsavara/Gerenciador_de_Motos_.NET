using System.ComponentModel.DataAnnotations;

namespace MottuChallenge.API.Entities
{
    public class MotorcycleEntity
    {
        [Key]
        public string Identifier { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
    }
}