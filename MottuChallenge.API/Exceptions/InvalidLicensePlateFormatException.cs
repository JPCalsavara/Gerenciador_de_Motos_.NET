namespace MottuChallenge.API.Exceptions
{
    public class InvalidLicensePlateFormatException : Exception
    {
        public InvalidLicensePlateFormatException() : base("O formato da placa é inválido. O formato esperado é XXX-XXXX.") { }
    }
}