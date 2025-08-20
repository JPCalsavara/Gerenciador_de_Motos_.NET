namespace MottuChallenge.API.Exceptions;

public class InvalidCnhTypeException : Exception
{
    public InvalidCnhTypeException(string type) : base($"O tipo de CNH '{type}' é inválido. Tipos aceitos: A, B, AB.") { }
}
