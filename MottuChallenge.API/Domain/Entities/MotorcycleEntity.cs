using System.ComponentModel.DataAnnotations;

namespace MottuChallenge.API.Entities;

public class MotorcycleEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
}