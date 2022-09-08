namespace DiaryApp.Exceptions;

public class AuthorizationInvalidException : Exception
{
    public AuthorizationInvalidException(string message) : base(message)
    {
        
    }
}