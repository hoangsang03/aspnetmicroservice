using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger) :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestType = typeof(TRequest).Name;
        string responseType = typeof(TResponse).Name;
        const int EXECUTION_TIME_THRESHOLD_MS = 3000;

        logger.LogInformation("[START] Handling Request: {RequestType} - Response: {ResponseType} - RequestData: {@RequestData}", requestType, responseType, request);

        var timer = Stopwatch.StartNew();

        var response = await next();

        timer.Stop();
        var executionTimeMilliseconds = timer.ElapsedMilliseconds;


        if (executionTimeMilliseconds > EXECUTION_TIME_THRESHOLD_MS)
        {
            logger.LogWarning("[PERFORMANCE] The request {RequestType} took {ExecutionTime} milliseconds.",
            typeof(TRequest).Name, executionTimeMilliseconds);
        }

        logger.LogInformation("[END] Completed handling {RequestType} with {ResponseType} in {ExecutionTime} milliseconds", requestType, responseType, executionTimeMilliseconds);

        return response;
    }
}
