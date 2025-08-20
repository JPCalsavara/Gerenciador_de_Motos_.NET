namespace MottuChallenge.API.Exceptions;

public class InvalidFileTypeException : Exception {
    public InvalidFileTypeException(string message) : base(message) { }
}
