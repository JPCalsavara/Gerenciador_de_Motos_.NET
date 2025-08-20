namespace MottuChallenge.API.Exceptions
{
    public class MotorcycleNotFoundException : Exception
    {
        public MotorcycleNotFoundException() : base("Moto não encontrada.") { }
    }
}