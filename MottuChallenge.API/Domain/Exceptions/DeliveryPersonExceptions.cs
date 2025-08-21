namespace MottuChallenge.API.Exceptions;

public class DeliveryPersonNotFoundException : Exception
{
    public DeliveryPersonNotFoundException() : base("Entregador não encontrado.") { } 
    
}

public class DuplicateCnhNumberException : Exception
{
    public DuplicateCnhNumberException(string cnh) : base($"O número de CNH '{cnh}' já está cadastrado.") { } 

}


public class DuplicateCnpjException : Exception
{
    public DuplicateCnpjException(string cnpj) : base($"O CNPJ '{cnpj}' já está cadastrado.") { }
}


public class InvalidCnhTypeException : Exception
{
    public InvalidCnhTypeException(string type) : base($"O tipo de CNH '{type}' é inválido. Tipos aceitos: A, B, AB.") { }
}