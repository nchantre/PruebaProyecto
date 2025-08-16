namespace RealEstate.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, string id)
            : base($"{entityName} with Id {id} was not found.") { }
    }

    public class InvalidIdFormatException : Exception
    {
        public InvalidIdFormatException(string id)
            : base($"The provided Id '{id}' is not a valid ObjectId.") { }
    }
}
