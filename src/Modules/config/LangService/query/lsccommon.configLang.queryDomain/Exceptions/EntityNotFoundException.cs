namespace lsccommon.configLang.queryDomain.Exceptions
{
    /// <summary>
    /// Exception for specific entity is not found
    /// </summary>
    /// <param name="id">id of not found entity</param>
    public class EntityNotFoundException(object? id) : NotFoundException($"Entity with Id: {id} not found");
}