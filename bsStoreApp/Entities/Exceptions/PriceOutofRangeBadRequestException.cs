namespace Entities.Exceptions
{
    public class PriceOutofRangeBadRequestException : BadRequestException
    {
        public PriceOutofRangeBadRequestException() : base("Maximum price should be less than 5000 and greater than 10")
        {
        }
    }
}
