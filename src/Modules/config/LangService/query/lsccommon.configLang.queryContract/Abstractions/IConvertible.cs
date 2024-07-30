namespace lsccommon.configLang.queryContract.Abstractions
{
    /// <summary>
    /// Marked object as convertible to domain entity
    /// </summary>
    /// <typeparam name="TDomainEntity">Generic type of domain entity</typeparam>
    public interface IConvertible<out TDomainEntity>
    {
        /// <summary>
        /// Convert to domain entity
        /// </summary>
        /// <returns>Domain entity</returns>
        public TDomainEntity ToDomainEntity();
    }
}