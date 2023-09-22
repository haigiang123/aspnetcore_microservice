using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {

        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehaviour(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();
            var response = await next();
            _timer.Stop();

            var elapsedMiliseconds = _timer.ElapsedMilliseconds;
            
            if(elapsedMiliseconds < 500) { return response; }

            var requestName = typeof(TRequest).Name;
            _logger.LogWarning($"Application Long Runtime Request: {requestName} ({elapsedMiliseconds} milisecond {request})");

            return response;
        }
    }
}
