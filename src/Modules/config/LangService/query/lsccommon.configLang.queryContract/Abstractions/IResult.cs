namespace lsccommon.configLang.queryContract.Abstractions
{
    /// <summary>
    /// Interface provide result for every use case handled
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Indicate action is success or not
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Attached error if action were failed
        /// </summary>
        public IError? Error { get; }
    }
}