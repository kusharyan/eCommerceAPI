namespace eCommerceApi.Exceptions
{
    public abstract class AppExcepton : Exception
    {
        public int StatusCode { get; }

        protected AppExcepton(string message, int statusCode) : base(message)
        {
            StatusCode =  statusCode;
        }
    }
}