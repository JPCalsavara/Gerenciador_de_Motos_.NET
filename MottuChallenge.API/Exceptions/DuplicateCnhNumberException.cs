namespace MottuChallenge.API.Exceptions;

public class DuplicateCnhNumberException : Exception
{
     public DuplicateCnhNumberException(string cnh) : base($"O número de CNH '{cnh}' já está cadastrado.") { } 

}