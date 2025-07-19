namespace CohesiveWizardry.Common.Exceptions
{
    /// <summary>
    /// Helping functions relative to exceptions.
    /// </summary>
    public static class ExceptionUtils
    {
        public static string BuildExceptionAndInnerExceptionsMessage(Exception exception, int spacingLevel = 1)
        {
            if (exception == null)
                return null;

            string message = $"{"".PadLeft(spacingLevel)}[{Environment.NewLine}{"".PadLeft(spacingLevel)} Message=[{exception.Message}]{Environment.NewLine} {"".PadLeft(spacingLevel)}StackTrace=[{exception.StackTrace}]{Environment.NewLine}{"".PadLeft(spacingLevel)}]{Environment.NewLine}";

            if (exception.InnerException != null)
                message += $"{"".PadLeft(spacingLevel)}InnerException={Environment.NewLine}{"".PadLeft(spacingLevel)}[{Environment.NewLine}{BuildExceptionAndInnerExceptionsMessage(exception.InnerException, spacingLevel + 1)}{"".PadLeft(spacingLevel)}]{Environment.NewLine}";

            return message;
        }
    }
}
