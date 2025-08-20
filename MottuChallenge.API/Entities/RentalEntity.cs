using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottuChallenge.API.Entities
{
    public class RentalEntity
    {
        [Key]
        public String Identifier { get; set; }

        [Required]
        public string DeliveryPersonIdentifier { get; set; }
        
        [ForeignKey("DeliveryPersonIdentifier")]
        public DeliveryPersonEntity DeliveryPerson { get; set; }

        [Required]
        public string MotorcycleIdentifier { get; set; }

        [ForeignKey("MotorcycleIdentifier")]
        public MotorcycleEntity Motorcycle { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PredictedEndDate { get; set; }
        public int PlanDays { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsActive { get; set; } = true;
    }
}