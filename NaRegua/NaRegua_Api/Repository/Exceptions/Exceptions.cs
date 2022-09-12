namespace NaRegua_Api.Repository.Exceptions
{
    public class PrimaryKeyException : Exception 
    {
        public PrimaryKeyException(string message) : base(message) { }
    }

    public class CannotScheduleException : Exception
    {
        public CannotScheduleException(string message) : base(message) { }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User not found.") { }
    }

    public class IncorretPasswordException : Exception
    {
        public IncorretPasswordException() : base("Incorrect password") { }
    }
}
