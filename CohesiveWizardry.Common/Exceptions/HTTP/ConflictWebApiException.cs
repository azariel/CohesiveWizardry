namespace CohesiveWizardry.Common.Exceptions.HTTP
{
    public class ConflictWebApiException : WebApiException
    {
        public ConflictWebApiException(string errorCode, string message, Exception innerException = null) : base(errorCode, message, innerException)
        {
        }
    }
}
