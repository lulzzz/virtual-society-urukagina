﻿using Vs.VoorzieningenEnRegelingen.BurgerPortaal.Objects;
using Vs.VoorzieningenEnRegelingen.Core;
using Vs.VoorzieningenEnRegelingen.Core.Model;
using Vs.VoorzieningenEnRegelingen.Service.Controllers;

namespace Vs.VoorzieningenEnRegelingen.BurgerPortaal.Controllers
{
    public class SequenceController : ISequenceController
    {
        public ISequence Sequence { get; set; }
        public int CurrentStep { get; set; } = 0;
        public int RequestStep { get; set; } = 0;
        public ParseResult ParseResult { get; set; }
        public ExecutionResult LastExecutionResult { get; set; }

        private IServiceController _serviceController { get; set; }

        public SequenceController(IServiceController serviceController)
        {
            _serviceController = serviceController;

            Sequence = new Sequence("https://raw.githubusercontent.com/sjefvanleeuwen/virtual-society-urukagina/master/doc/test-payloads/zorgtoeslag-2019.yml");
            if (ParseResult == null)
            {
                ParseResult = _serviceController.Parse(GetParseRequest());
            }
        }

        public ExecuteRequest GetExecuteRequest(ParametersCollection parameters = null)
        {
            return new ExecuteRequest
            {
                Config = Sequence.Yaml,
                Parameters = parameters
            };
        }

        public ParseRequest GetParseRequest()
        {
            return new ParseRequest
            {
                Config = Sequence.Yaml
            };
        }

        public void ExecuteStep(IParameter currentParameter)
        {
            SaveCurrentParameter(currentParameter);
            //var requestParameters = GetRequestParameters();
            var requestParameters = Sequence.GetParametersToSend(RequestStep);
            var request = GetExecuteRequest(requestParameters);
            LastExecutionResult = _serviceController.Execute(request);

            Sequence.UpdateParametersCollection(LastExecutionResult.Parameters);

            Sequence.AddStep(RequestStep, LastExecutionResult);
            CurrentStep = RequestStep;
        }

        private void SaveCurrentParameter(IParameter currentParameter)
        {
            if (currentParameter != null)
            {
                Sequence.Parameters.UpSert(currentParameter);
            }
        }
    }
}
