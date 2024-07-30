using lsccommon.configLang.queryContract.Abstractions;

namespace lsccommon.configLang.queryContract.Shared
{
    /// <summary>
    /// Provide result for use case handle
    /// </summary>
    public class Result : IResult
    {
        public bool IsSuccess { get; }
        public IError? Error { get; }

        public Result(bool isSuccess, IError? error = null)
        {
            // Throw an exception if the parameters are inconsistent
            if (isSuccess && error is not null ||
                !isSuccess && error is null)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// Create failure result with failed state and attached error 
        /// </summary>
        /// <param name="error">Error of failure</param>
        /// <returns>Failure result with error</returns>
        public static Result Failure(IError error)
        {
            return new Result(false, error);
        }

        /// <summary>
        /// Create success result with success state
        /// </summary>
        /// <returns>Success result without data</returns>
        public static Result Success()
        {
            return new Result(true);
        }

        /// <summary>
        /// Create success result with data 
        /// </summary>
        /// <typeparam name="TModel">Generic type</typeparam>
        /// <param name="value"></param>
        /// <returns>Success result contain data</returns>
        public static Result<TModel> Success<TModel>(TModel value)
        {
            return new Result<TModel>(true, value: value);
        }

        /// <summary>
        /// Create failure result with failed state, attached error and data
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="value"></param>
        /// <param name="error"></param>
        /// <returns>A failed Result object with the given value and error</returns>
        public static Result<TModel> Failure<TModel>(TModel value, IError error)
        {
            return new Result<TModel>(true, error, value);
        }
    }
}