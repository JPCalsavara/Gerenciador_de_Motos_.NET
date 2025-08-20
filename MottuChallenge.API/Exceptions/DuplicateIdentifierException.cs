namespace MottuChallenge.API.Exceptions
{
    public class DuplicateIdentifierException : Exception
    {
        public DuplicateIdentifierException(string identifier) : base($"O identificador '{identifier}' já está cadastrado.") { }
    }
}