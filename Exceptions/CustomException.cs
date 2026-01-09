namespace eCommerceApi.Exceptions
{
    public class NotFoundException : AppExcepton
    {
        public NotFoundException(string message) 
            : base(message, StatusCodes.Status404NotFound) { }
    }

    public class BadRequestException : AppExcepton
    {
        public BadRequestException(string message)
            : base(message, StatusCodes.Status400BadRequest) { }
    }

    public class ConflictException : AppExcepton
    {
        public ConflictException(string message)
            : base(message, StatusCodes.Status409Conflict) { }
    }
}