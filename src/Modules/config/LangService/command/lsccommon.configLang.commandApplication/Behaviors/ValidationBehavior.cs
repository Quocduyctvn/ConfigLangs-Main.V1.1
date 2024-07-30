using FluentValidation;
using MediatR;
using ValidationException = lsccommon.configLang.commandDomain.Exceptions.ValidationException;

namespace lsccommon.configLang.commandApplication.Behaviors
{
    /// <summary>
    /// Pipeline for validation request before going to handle
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request,
                                            RequestHandlerDelegate<TResponse> next,
                                            CancellationToken cancellationToken)
        {
            // Get validator of request
            if (_validator is null)
            {
                return await next();
            }

            // Validate request
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid)
            {
                return await next();
            }

            // Throw errors if any
            var errors = validationResult.Errors.ConvertAll(x => x.ErrorMessage);
            throw new ValidationException(errors.ToArray());
        }
    }
}