namespace TeamworkApp.Application.Auth;

public class EmailAlreadyInUseException : Exception
{
    public EmailAlreadyInUseException(string email) : base($"The email '{email}' is already in use.")
    {

    }
}
