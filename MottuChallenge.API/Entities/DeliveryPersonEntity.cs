using System.ComponentModel.DataAnnotations;

namespace MottuChallenge.API.Entities;

public class DeliveryPersonEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string CnhNumber { get; set; } = string.Empty;
    public CnhType CnhType { get; set; }
    public string? CnhImageUrl { get; set; }
}