using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviour;

public class UnhandledExceptionBehaviour<TRequest, TResponse>: IPipelineBehavior<TRequest,TResponse> where TRequest: IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(e, $"Unhandled Exception Occurred with Request Name: {requestName}, {request}");
            throw;
        }
    }
}