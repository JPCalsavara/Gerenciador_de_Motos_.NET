namespace MottuChallenge.API.Exceptions;


public class DuplicateLicensePlateException : Exception
{
    public DuplicateLicensePlateException() : base("A placa informada já está cadastrada.") { }
    public DuplicateLicensePlateException(string licensePlate) : base($"A placa '{licensePlate}' já está cadastrada.") { }
    public DuplicateLicensePlateException(string message, Exception innerException) : base(message, innerException) { }
}

public class InvalidLicensePlateFormatException : Exception
{
    private const string DefaultMessage = "O formato da placa é inválido. O formato esperado é XXX-XXXX.";

    public InvalidLicensePlateFormatException() : base(DefaultMessage) { }
    public InvalidLicensePlateFormatException(string message) : base(message) { }
    public InvalidLicensePlateFormatException(string message, Exception innerException) : base(message, innerException) { }
}

public class MotorcycleNotFoundException : Exception
{
    public MotorcycleNotFoundException() : base("A moto não foi encontrada.") { }
    public MotorcycleNotFoundException(string identifier) : base($"A moto com o identificador '{identifier}' não foi encontrada.") { }
    public MotorcycleNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}