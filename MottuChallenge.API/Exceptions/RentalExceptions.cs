namespace MottuChallenge.API.Exceptions
{
    public class DeliveryPersonNotEligibleException : Exception { public DeliveryPersonNotEligibleException() : base("Entregador não possui CNH do tipo 'A'.") { } }
    public class DeliveryPersonHasActiveRentalException : Exception { public DeliveryPersonHasActiveRentalException() : base("Entregador já possui uma locação ativa.") { } }
    public class MotorcycleAlreadyRentedException : Exception { public MotorcycleAlreadyRentedException() : base("A moto selecionada já está alugada.") { } }
    public class InvalidRentalPlanException : Exception { public InvalidRentalPlanException() : base("Plano de locação inválido.") { } }
    public class RentalNotFoundException : Exception { public RentalNotFoundException() : base("Locação não encontrada ou já finalizada.") { } }
}