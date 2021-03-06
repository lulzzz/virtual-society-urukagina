﻿using System;
using System.Collections.Generic;
using Vs.Core.Diagnostics;
using Vs.Core.Serialization;
using Vs.Graph.Core.Helpers;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace Vs.Graph.Core.Data
{
    public class Constraints : List<Constraint>, IYamlConvertible, IDebugInfo
    {
        private class DeserializeTemplate : List<Constraint> { }

        public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
        {
            var o = (DeserializeTemplate)nestedObjectDeserializer(typeof(DeserializeTemplate));
            if (o == null)
                return;
            AddRange(o);
            DebugInfo = new DebugInfo().MapDebugInfo(parser.Current.Start, parser.Current.End);
        }

        public void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
        {
            nestedObjectSerializer(new List<Constraint>(this));
        }

        public DebugInfo DebugInfo { get; set; }
    }
}