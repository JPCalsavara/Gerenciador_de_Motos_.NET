namespace MottuChallenge.API.Exceptions
{
    public class DuplicateLicensePlateException : Exception
    {
        public DuplicateLicensePlateException(string licensePlate) : base($"A placa '{licensePlate}' já está cadastrada.") { }
    }
}