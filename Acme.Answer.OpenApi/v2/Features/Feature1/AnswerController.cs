﻿using Acme.Answer.OpenApi.v2.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Threading.Tasks;

namespace Acme.Answer.OpenApi.v2.Features.Feature1
{
    /// <summary>
    /// Rules API integrates the rule engine and exposes it as OAS3.
    /// Uses best practices from: https://github.com/RicoSuter/NSwag/wiki/AspNetCoreOpenApiDocumentGenerator
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiVersion("2.0-feature1")]
    [Route("api/v{version:apiVersion}/qa")]
    [OpenApiTag("Answer API", Description = "This is current api")]
    [ApiController]
    public class AnswerController : Controllers.AnswerController
    {
        [HttpPost("some method")]
        public async Task<AnswerPayload> SomeMethod(QuestionPayload payload)
        {
            return new AnswerPayload();
        }
    }
}
