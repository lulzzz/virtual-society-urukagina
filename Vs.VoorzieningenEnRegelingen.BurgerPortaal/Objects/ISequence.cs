﻿using System.Collections.Generic;
using Vs.VoorzieningenEnRegelingen.Core;

namespace Vs.VoorzieningenEnRegelingen.BurgerPortaal.Objects
{
    public interface ISequence
    {
        string Yaml { get; set; }
        IParametersCollection Parameters { get; }
        IEnumerable<IStep> Steps { get; }

        IParametersCollection GetParametersToSend(int step);
        void AddStep(int requestStep, IExecutionResult result);
        void UpdateParametersCollection(IParametersCollection parameters);
    }
}