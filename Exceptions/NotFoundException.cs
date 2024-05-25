namespace LittleLemon_API.Exceptions;

public class NotFoundException : SystemException
{
    public NotFoundException(string message) : base(message)
    {

    }
}