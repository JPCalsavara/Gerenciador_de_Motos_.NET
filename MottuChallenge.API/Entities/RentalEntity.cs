using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottuChallenge.API.Entities
{
    public class RentalEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid DeliveryPersonId { get; set; }
        
        [ForeignKey("DeliveryPersonId")]
        public DeliveryPersonEntity DeliveryPerson { get; set; }

        [Required]
        public Guid MotorcycleId { get; set; }

        [ForeignKey("MotorcycleId")]
        public MotorcycleEntity Motorcycle { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PredictedEndDate { get; set; }
        public int PlanDays { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsActive { get; set; } = true;
    }
}