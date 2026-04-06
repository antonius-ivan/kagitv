namespace ICLAco.SalesItem.API.Infrastructure.Exceptions
{
    public class SalesItemDomainException : Exception
    {
        public SalesItemDomainException()
        {

        }

        public SalesItemDomainException(string message)
            : base(message)
        {

        }

        public SalesItemDomainException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
