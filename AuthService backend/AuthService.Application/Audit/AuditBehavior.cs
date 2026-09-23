using AuthService.Application.Auth.Session;
using AuthService.Domain.Audit;
using AuthService.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Audit
{
    public sealed class AuditBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IAuditableRequest
    {
        public IAuditService _audit;


        public AuditBehavior(IAuditService audit)
        {
            _audit = audit;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            var response = await next();

            if (response is Result result)
            {
                if (result.IsSuccess)
                {
                    await _audit.LogSuccessAsync(request.EventType.Value);
                }
                else
                {
                    await _audit.LogFailureAsync(request.EventType.Value);
                }
            }

            return response;
        }
    }
}
