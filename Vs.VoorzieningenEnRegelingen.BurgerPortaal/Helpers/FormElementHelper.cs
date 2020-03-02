﻿using System;
using System.Collections.Generic;
using System.Linq;
using Vs.VoorzieningenEnRegelingen.BurgerPortaal.Objects;
using Vs.VoorzieningenEnRegelingen.BurgerPortaal.Shared.Components.FormElements;
using Vs.VoorzieningenEnRegelingen.Core;

namespace Vs.VoorzieningenEnRegelingen.BurgerPortaal.Helpers
{
    public static class FormElementHelper
    {
        public static IFormElement ParseExecutionResult(ExecutionResult result)
        {
            var formElement = new FormElement() as IFormElement;
            formElement.InferedType = GetInferedType(result.Questions);

            formElement.Name = result.Questions.Parameters[0].Name;
            formElement.Label = GetFromLookupTable(result.Questions.Parameters, _labels);
            formElement.Options = DefineOptions(result.Questions);
            formElement.TagText = GetFromLookupTable(result.Questions.Parameters, _tagText, false);
            formElement.HintText = GetFromLookupTable(result.Questions.Parameters, _hintText, false);
            return formElement;
        }

        private static TypeInference.InferenceResult.TypeEnum GetInferedType(QuestionArgs questions)
        {
            return questions.Parameters.FirstOrDefault().Type;
        }

        private static string GetFromLookupTable(ParametersCollection parameters, Dictionary<string, string> dictionary, bool showDefaultText = false)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            var key = parameters.FirstOrDefault(p => dictionary.Keys.Contains(p.Name))?.Name ?? string.Empty;
            if (dictionary.Keys.Contains(key))
            {
                return dictionary[key];
            }

            return showDefaultText ? $"Unknown for {parameters[0].Name}" : string.Empty;
        }

        public static string DefineFormElementType(ExecutionResult result)
        {
            //if (result.Questions.ToList().Last())
            return string.Empty;
        }

        private static Dictionary<string, string> DefineOptions(QuestionArgs questions)
        {
            switch (GetInferedType(questions))
            {
                case TypeInference.InferenceResult.TypeEnum.Boolean:
                    return BooleanToOptions(questions);
                case TypeInference.InferenceResult.TypeEnum.List:
                    return ListToOptions(questions);
                default:
                    return null;
            }
        }

        private static Dictionary<string, string> BooleanToOptions(QuestionArgs questions)
        {
            var result = new Dictionary<string, string>();
            questions.Parameters.ForEach(p => result.Add(p.Name, GetParameterDisplayName(p.Name)));
            return result;
        }

        private static string GetParameterDisplayName(string name)
        {
            return name.Substring(0, 1).ToUpper() + name.Substring(1).Replace('_', ' ');
        }

        private static Dictionary<string, string> ListToOptions(QuestionArgs questions)
        {
            var result = new Dictionary<string, string>();
            (questions.Parameters.First().Value as List<object>).ForEach(v => result.Add(v.ToString(), v.ToString()));
            return result;
        }

        private static Dictionary<string, string> _labels = new Dictionary<string, string> {
            { "woonland", "Woonland" },
            { "alleenstaande", "Woonsituatie"}
        };

        private static Dictionary<string, string> _tagText = new Dictionary<string, string>
        {

        };

        private static Dictionary<string, string> _hintText = new Dictionary<string, string> {
            { "woonland", "Selecteer \"Anders\" wanneer het land niet in de lijst staat." },
            { "alleenstaande", "Geef aan of u alleenstaande bent of dat u samen woont met een toeslagpartner."}
        };

        internal static string GetValue(ISequence sequence, ExecutionResult result)
        {
            var value = GetSavedValue(sequence, result);
            if (string.IsNullOrWhiteSpace(value) && GetInferedType(result.Questions) == TypeInference.InferenceResult.TypeEnum.List)
            {
                value = GetDefaultListValue(result);
            }

            return value;
        }

        private static string GetSavedValue(ISequence sequence, ExecutionResult result)
        {
            //find the step that is a match for this name
            var step = sequence.Steps.FirstOrDefault(s => s.IsMatch(result.Questions.Parameters.FirstOrDefault()));
            if (step == null)
            {
                return string.Empty;
            }

            //find the corresponding saved Parameter for this step
            var parameter = sequence.Parameters.FirstOrDefault(p => step.IsMatch(p));
            if (parameter == null)
            {
                return string.Empty;
            }

            if (parameter.Type == TypeInference.InferenceResult.TypeEnum.Boolean)
            {
                return parameter.Name;
            }

            return parameter.ValueAsString;
        }

        private static string GetDefaultListValue(ExecutionResult result)
        {
            var options = DefineOptions(result.Questions);
            if (options.Keys.Contains(string.Empty))
            {
                return string.Empty;
            }

            return options.Keys?.FirstOrDefault() ?? string.Empty;
        }
    }
}