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

            if (result.Questions == null)
            {
                return formElement;
            }

            formElement.InferedType = GetInferedType(result.Questions);

            formElement.Name = result.Questions.Parameters[0].Name;
            formElement.Label = GetFromLookupTable(result.Questions.Parameters, _labels);
            formElement.Options = DefineOptions(result);
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

        private static Dictionary<string, string> DefineOptions(ExecutionResult result)
        {
            switch (GetInferedType(result.Questions))
            {
                case TypeInference.InferenceResult.TypeEnum.Boolean:
                    return BooleanToOptions(result);
                case TypeInference.InferenceResult.TypeEnum.List:
                    return ListToOptions(result.Questions);
                default:
                    return null;
            }
        }

        private static Dictionary<string, string> BooleanToOptions(ExecutionResult result)
        {
            var options = new Dictionary<string, string>();
            result.Questions.Parameters.ForEach(p => options.Add(p.Name, GetParameterDisplayName(p.Name, result.Parameters)));
            return options;
        }

        private static string GetParameterDisplayName(string name, ParametersCollection parameters)
        {
            switch(name)
            {
                case "hoger_dan_de_vermogensdrempel":
                    if (parameters.Any(p => p.Name == "alleenstaande" && (bool)p.Value))
                        return "Ja, mijn vermogen is hoger dan €114.776,00";
                    else return "Ja, het gezamenlijk vermogen is hoger dan €145.136,00";
                case "lager_dan_de_vermogensdrempel":
                    if (parameters.Any(p => p.Name == "alleenstaande" && (bool)p.Value))
                        return "Nee, mijn vermogen is lager dan €114.776,00";
                    else return "Nee, het gezamenlijk vermogen is lager dan €145.136,00";
                case "hoger_dan_de_inkomensdrempel":
                    if (parameters.Any(p => p.Name == "alleenstaande" && (bool)p.Value))
                        return "Ja, mijn inkomen is hoger dan €29.562,00";
                    else return "Ja, het gezamenlijk inkomen is hoger dan €37.885,00";
                case "lager_dan_de_inkomensdrempel":
                    if (parameters.Any(p => p.Name == "alleenstaande" && (bool)p.Value))
                        return "Nee, mijn inkomen is lager dan €29.562,00";
                    else return "Nee, het gezamenlijk inkomen is lager dan €37.885,00";
            }

            return name.Substring(0, 1).ToUpper() + name.Substring(1).Replace('_', ' ');
        }

        private static Dictionary<string, string> ListToOptions(QuestionArgs questions)
        {
            var result = new Dictionary<string, string>();
            (questions.Parameters.First().Value as List<object>).ForEach(v => result.Add(v.ToString(), v.ToString()));
            return result;
        }

        private static Dictionary<string, string> _labels = new Dictionary<string, string>
        {
            //{ "woonland", "Woonland" },
            //{ "alleenstaande", "Woonsituatie"},
            //{ "lager_dan_de_inkomensdrempel", "Inkomensdrempel"},
            //{ "lager_dan_de_vermogensdrempel", "Vermogensdrempel"}
            //{ "toetsingsinkomen_aanvrager", "" },
            //{ "toetsingsinkomen_gezamenlijk", "" }
        };

        private static Dictionary<string, string> _tagText = new Dictionary<string, string>
        {
        };

        private static Dictionary<string, string> _hintText = new Dictionary<string, string> {
            { "woonland", "Selecteer \"Anders\" wanneer het land niet in de lijst staat." },
            { "alleenstaande", "Geef aan of u alleenstaande bent of dat u een toeslagpartner heeft."},
            { "hoger_dan_de_inkomensdrempel", ""},
            { "hoger_dan_de_vermogensdrempel", ""},
            { "toetsingsinkomen_aanvrager", "Vul een getal in. Gebruik geen punt (\".\"), en slechts een komma (\",\") als scheidngsteken tussen euro's en centen." },
            { "toetsingsinkomen_gezamenlijk", "Vul een getal in. Gebruik geen punt (\".\"), en slechts een komma (\",\") als scheidngsteken tussen euro's en centen." }
        };

        internal static string GetValue(ISequence sequence, ExecutionResult result)
        {
            var value = GetSavedValue(sequence, result);
            if (GetInferedType(result.Questions) == TypeInference.InferenceResult.TypeEnum.List)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    value = GetDefaultListValue(result);
                }
            }
            if (GetInferedType(result.Questions) == TypeInference.InferenceResult.TypeEnum.Double)
            {
                value = value?.Replace('.', ',');
            }

            return value;
        }

        private static string GetSavedValue(ISequence sequence, ExecutionResult result)
        {
            //no saved value if there is no question
            if (result.Questions == null)
            {
                return null;
            }
            //find the step that is a match for this name
            var step = sequence.Steps.FirstOrDefault(s => s.IsMatch(result.Questions.Parameters.FirstOrDefault()));
            if (step == null)
            {
                return null;
            }

            //find the corresponding saved Parameter for this step
            var parameters = sequence.Parameters.Where(p => step.IsMatch(p));
            if (parameters == null || !parameters.Any())
            {
                return null;
            }
            if (parameters.Count() == 1)
            {
                return parameters.Single().ValueAsString;
            }

            if (parameters.First().Type == TypeInference.InferenceResult.TypeEnum.Boolean)
            {
                return parameters.FirstOrDefault(p => (bool)p.Value).Name;
            }

            return parameters.First().ValueAsString;
        }

        private static string GetDefaultListValue(ExecutionResult result)
        {
            var options = DefineOptions(result);
            if (options.Keys.Contains(string.Empty))
            {
                return string.Empty;
            }

            return options.Keys?.FirstOrDefault() ?? string.Empty;
        }
    }
}
