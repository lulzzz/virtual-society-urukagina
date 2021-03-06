﻿using System.Collections.Generic;
using System.Linq;
using Vs.Core.Diagnostics;
using Vs.Rules.Core.Exceptions;
using Vs.Rules.Core.Interfaces;
using Vs.Rules.Core.Model;
using Vs.VoorzieningenEnRegelingen.Core.TestData;
using Xunit;

namespace Vs.Rules.Core.Tests
{
    public class TableTests
    {
        [Fact]
        public void CanInferTypeFromColumValues()
        {
            // not needed for this test but not nullable.
            var debugInfoFake = new DebugInfo(new LineInfo(0, 0, 0), new LineInfo(0, 0, 0));
            var columnTypes = new List<ColumnType>();
            columnTypes.Add(new ColumnType(debugInfoFake, "woonland"));
            columnTypes.Add(new ColumnType(debugInfoFake, "factor"));
            var rows = new List<Row>();
            rows.Add(new Row(debugInfoFake, new List<Column>()
            {
                new Column(debugInfoFake,"Nederland"), new Column(debugInfoFake,"1.0")
            }));
            Table table = new Table(debugInfoFake, "woonlandfactoren", columnTypes, rows);
            Assert.True(columnTypes[0].Type == TypeInference.InferenceResult.TypeEnum.String);
            Assert.True(columnTypes[1].Type == TypeInference.InferenceResult.TypeEnum.Double);
        }

        [Fact]
        public void CanDetermineTableLookupValueFromQuestion()
        {
            var controller = new YamlScriptController();
            var result = controller.Parse(YamlTestFileLoader.Load(@"TableTests.yaml"));
            Assert.False(result.IsError);
            var parameters = new ParametersCollection() as IParametersCollection;
            controller.QuestionCallback = (FormulaExpressionContext sender, QuestionArgs args) =>
            {
                Assert.True(args.Parameters[0].Name == "woonland");
                Assert.True(args.Parameters[0].Type == TypeInference.InferenceResult.TypeEnum.List);
                // This list can be used to do a selection of a valid woonland
                Assert.True(((List<object>)args.Parameters[0].Value).Count > 0);
                // Provide an anwser by selecting an item: Finland from the list
                parameters.Add(new ClientParameter(args.Parameters[0].Name,
                    ((List<object>)args.Parameters[0].Value)[1], TypeInference.InferenceResult.TypeEnum.List, "Dummy"));
            };
            var executionResult = new ExecutionResult(ref parameters) as IExecutionResult;
            try
            {
                var workflow = controller.ExecuteWorkflow(ref parameters, ref executionResult);
            }
            catch (UnresolvedException)
            {
                // The server lookup needs to be evaluated again to get the resulting woonlandfactor.
                // In this case the client will not need to have to answer another question.
                // Maybe this can be put in core, in order to make the client logic simpler.
                var evaluateAgain = controller.ExecuteWorkflow(ref parameters, ref executionResult);
            }
            Assert.True(parameters[0].Name == "woonland");
            Assert.True((string)parameters[0].Value == "Finland");
            Assert.True(parameters[1].Name == "woonlandfactor");
            Assert.True((double)parameters[1].Value == 0.7161);
            Assert.True(parameters[2].Name == "recht");
            Assert.True((bool)parameters[2].Value == true);

            // Quick Hack to see if recht is false by selecting woonland: Anders
            parameters.Clear();
            parameters.Add(new ClientParameter("woonland", "Anders", TypeInference.InferenceResult.TypeEnum.List, "Dummy"));
            var recalculate = controller.ExecuteWorkflow(ref parameters, ref executionResult);
            Assert.True(parameters[0].Name == "woonland");
            Assert.True((string)parameters[0].Value == "Anders");
            Assert.True(parameters[1].Name == "woonlandfactor");
            Assert.True((double)parameters[1].Value == 0);
            Assert.True(parameters[2].Name == "recht");
            Assert.True((bool)parameters[2].Value == false);
            Assert.NotNull(recalculate.Stacktrace.FindLast(p => p.IsStopExecution == true));
        }

        [Fact]
        public void YamlCanParseAndEvaluateSituationalTables()
        {
            var controller = new YamlScriptController();
            var result = controller.Parse(YamlTestFileLoader.Load(
                @"UnitTests/SituationalTables/ValidSituations.yaml"));
            Assert.False(result.IsError);
            Assert.True(result.Model.Tables.ElementAt(0).IsSituational);
            Assert.True(result.Model.Tables.ElementAt(0).Situations.ElementAt(0).Expression == "keuze_c1");
            Assert.True(result.Model.GetTablesByName("tabel1").Count() == 3);

            var parameters = new ParametersCollection() {
                new ClientParameter("waarde_a", "70", TypeInference.InferenceResult.TypeEnum.Double,"Dummy"),
                new ClientParameter("waarde_b", "40", TypeInference.InferenceResult.TypeEnum.Double,"Dummy"),
                new ClientParameter("keuze_c1", "ja", TypeInference.InferenceResult.TypeEnum.Boolean,"Dummy"),
                new ClientParameter("keuze_c2", "nee", TypeInference.InferenceResult.TypeEnum.Boolean,"Dummy")
            } as IParametersCollection;

            var executionResult = null as IExecutionResult;
            executionResult = new ExecutionResult(ref parameters);
            executionResult = controller.ExecuteWorkflow(ref parameters, ref executionResult);
        }
    }
}
