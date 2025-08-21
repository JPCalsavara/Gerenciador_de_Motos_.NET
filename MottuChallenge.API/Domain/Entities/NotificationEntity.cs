using System.ComponentModel.DataAnnotations;

namespace MottuChallenge.API.Entities
{
    public class NotificationEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MotorcycleId { get; set; }
        public DateTime NotifiedAt { get; set; } = DateTime.UtcNow;
    }
}