﻿using System.Collections.Generic;
using Vs.VoorzieningenEnRegelingen.Core;

namespace Vs.VoorzieningenEnRegelingen.BurgerPortaal.Objects.Interface
{
    public interface ISequence
    {
        string Yaml { get; set; }
        IParametersCollection Parameters { get; }
        IEnumerable<ISequenceStep> Steps { get; }

        IParametersCollection GetParametersToSend(int step);
        void AddStep(int requestStep, IExecutionResult result);
        void UpdateParametersCollection(IParametersCollection parameters);
    }
}