namespace MottuChallenge.API.Exceptions
{
    public class DuplicateCnpjException : Exception
    {
        public DuplicateCnpjException(string cnpj) : base($"O CNPJ '{cnpj}' já está cadastrado.") { }
    }
}