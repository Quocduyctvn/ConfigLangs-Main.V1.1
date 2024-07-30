namespace lsccommon.configLang.queryContract.Extensions
{
    /// <summary>
    /// Extensions for "Exception" class
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Use for add error code to exception
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="exceptionCode"></param>
        public static void AddErrorCode(this Exception exception, string exceptionCode)
        {
            if (exception.Data["errorCode"] is not null)
            {
                exception.Data["errorCode"] = exceptionCode;
                return;
            }

            exception.Data.Add("errorCode", exceptionCode);
        }

        /// <summary>
        /// Get error code from exception
        /// </summary>
        /// <param name="exception"></param>
        /// <returns>The error code as a string, or null if no error code is found</returns>
        public static string? GetErrorCode(this Exception exception)
        {
            return exception.Data["errorCode"]?.ToString();
        }
    }
}