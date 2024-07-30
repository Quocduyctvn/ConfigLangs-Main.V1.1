using lsccommon.configLang.commandContract.Abstractions;

namespace lsccommon.configLang.commandContract.Shared
{
    /// <summary>
    /// Provide result with specific type of data for use case handle
    /// </summary>
    public class Result<TModel>(bool isSuccess, IError? error = null, TModel? value = default) : IResult
    {
        public bool IsSuccess { get; } = isSuccess;
        public IError? Error { get; } = error;
        public TModel? Value { get; private set; } = value;

        /// <summary>
        /// Implicit cast from model to generic result
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Result<TModel>(TModel value)
        {
            return new Result<TModel>(true, value: value);
        }

        /// <summary>
        /// Implicit cast from result to generic result
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Result<TModel>(Result value)
        {
            return new Result<TModel>(value.IsSuccess, value.Error);
        }
    }
}