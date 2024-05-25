namespace LittleLemon_API.Exceptions;

public class BadRequestException : SystemException
{
    public BadRequestException(string message) : base(message)
    {

    }
}