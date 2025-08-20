namespace MottuChallenge.API.Exceptions;

public class DeliveryPersonNotFoundException : Exception
{
    public DeliveryPersonNotFoundException() : base("Entregador não encontrado.") { } 
    
}
