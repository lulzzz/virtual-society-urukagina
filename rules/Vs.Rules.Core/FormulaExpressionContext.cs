﻿using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Flee.PublicTypes;
using Vs.Rules.Core.Exceptions;
using Vs.Rules.Core.Financial;
using Vs.Rules.Core.Interfaces;
using Vs.Rules.Core.Model;
using static Vs.Rules.Core.YamlScriptController;

namespace Vs.Rules.Core
{
    public class FormulaExpressionContext
    {
        private ExpressionContext _context;
        private Model.Model _model;
        private Formula _formula;
        private ParametersCollection _parameters;

        public QuestionDelegate OnQuestion { get; }

        public static void Map(ref ParametersCollection parameters, VariableCollection variables)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (variables is null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            foreach (var parameter in parameters)
            {
                if (variables.ContainsKey(parameter.Name))
                {

                    variables.Remove(parameter.Name);
                }
                variables.Add(parameter.Name, parameter.Value.Infer());
            }
        }

        public FormulaExpressionContext(ref Model.Model model, ref IParametersCollection parameters, Formula formula, QuestionDelegate onQuestion, YamlScriptController controller)
        {
            _parameters = parameters as ParametersCollection ?? throw new ArgumentNullException(nameof(parameters));
            _formula = formula ?? throw new ArgumentNullException(nameof(formula));
            OnQuestion = onQuestion ?? throw new ArgumentNullException(nameof(onQuestion));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            // Define the context of our expression
            _context = new ExpressionContext(controller);
            _context.Options.ParseCulture = CultureInfo.InvariantCulture;
            // Allow the expression to use all static public methods of System.Math
            _context.Imports.AddType(typeof(Math));
            // Allow the expression to use all static overload public methods our CustomFunctions class
            _context.Imports.AddType(typeof(CustomFunctions));
            // Financial formulas
            _context.Imports.AddType(typeof(BankingFormulas));
            _context.Imports.AddType(typeof(CorporateFormulas));
            _context.Imports.AddType(typeof(FinancialFormulas));
            _context.Imports.AddType(typeof(FinancialMarketsFormulas));
            _context.Imports.AddType(typeof(GeneralFinanceFormulas));
            _context.Imports.AddType(typeof(StocksBondsFormulas));
            // this will visit ResolveVariableType
            _context.Variables.ResolveVariableType += ResolveVariableType;
            // this will visit ResolveVariableValue
            _context.Variables.ResolveVariableValue += ResolveVariableValue;
            Map(ref _parameters, _context.Variables);
        }

        private static Parameter Evaluate(FormulaExpressionContext caller, ref ExpressionContext context, ref Formula formula, ref ParametersCollection parameters1, QuestionDelegate onQuestion, ref Model.Model model)
        {
            if (parameters1 is null)
            {
                throw new ArgumentNullException(nameof(parameters1));
            }

            IDynamicExpression e = null;
            if (!formula.IsSituational)
            {
                try
                {
                    e = context.CompileDynamic(formula.Functions[0].Expression);
                    var result = e.Evaluate().Infer();
                    Parameter parameter;
                    parameter = new Parameter(formula.Name, result.Infer(), null, ref model);
                    parameter.IsCalculated = true;
                    parameters1.Add(parameter);
                    if (context.Variables.ContainsKey(parameter.Name))
                    {
                        context.Variables.Remove(parameter.Name);
                    }
                    context.Variables.Add(parameter.Name, parameter.Value.Infer());
                    return parameter;

                }
                catch (ExpressionCompileException)
                {
                    // Function can not evaluate further, before a Question/Answer sequence is fullfilled by the client.
                    throw new UnresolvedException($"Function {formula.Functions[0].Expression} can not evaluate further, before a Question/Answer sequence is fullfilled by the client.");
                }
            }
            else
            {
                foreach (var function in formula.Functions)
                {
                    foreach (var item in parameters1)
                    {
                        if (item.Name == function.Situation && (bool)item.Value == true)
                        {
                            try
                            {
                                e = context.CompileDynamic(function.Expression);
                                var parameter = new Parameter(formula.Name, e.Evaluate().Infer(), null, ref model);
                                parameter.IsCalculated = true;
                                parameters1.Add(parameter);
                                if (context.Variables.ContainsKey(parameter.Name))
                                {
                                    context.Variables.Remove(parameter.Name);
                                }
                                context.Variables.Add(parameter.Name, parameter.Value.Infer());
                                return parameter;

                            }
                            catch (ExpressionCompileException)
                            {
                                // Function can not evaluate further, before a Question/Answer sequence is fullfilled by the client.
                                throw new UnresolvedException($"Function {function.Expression} can not evaluate further, before a Question/Answer sequence is fullfilled by the client.");
                            }
                        }
                    }
                }
                StringBuilder situations = new StringBuilder();
                var parameters = new ParametersCollection();
                foreach (var function in formula.Functions)
                {
                    situations.Append(function.Situation + ",");
                    var parameter = new Parameter(function.Situation, TypeInference.Infer(function).Type, null, ref model);
                    parameters.Add(parameter);
                }
                if (onQuestion == null)
                    throw new Exception($"In order to evaluate variable one of the following situations:  {situations.ToString().Trim(',')}, you need to provide a delegate callback to the client for providing an answer");
                onQuestion(caller, new QuestionArgs("", parameters));
                // situation has to be formulated as an input parameter by the client.
                throw new UnresolvedException($"Can't evaluate formula {formula.Name} for situation. Please specify one of the following situations: {situations.ToString().Trim(',')}.");
            }

        }

        public Parameter Evaluate()
        {
            IDynamicExpression e;
            if (!_formula.IsSituational)
            {
                return NonSituationalEvaluate(out e);
            }
            else
            {
                return SituationalEvaluate(out e);
            }
        }

        private Parameter NonSituationalEvaluate(out IDynamicExpression e)
        {
            try
            {
                e = _context.CompileDynamic(_formula.Functions[0].Expression);
                var result = e.Evaluate().Infer();
                Parameter parameter;
                parameter = new Parameter(_formula.Name, result.Infer(), null, ref _model);
                parameter.IsCalculated = true;
                _parameters.Add(parameter);
                if (_context.Variables.ContainsKey(parameter.Name))
                {
                    _context.Variables.Remove(parameter.Name);
                }
                _context.Variables.Add(parameter.Name, parameter.Value.Infer());
                return parameter;

            }
            catch (ExpressionCompileException)
            {
                // Function can not evaluate further, before a Question/Answer sequence is fullfilled by the client.
                throw new UnresolvedException($"Function {_formula.Functions[0].Expression} can not evaluate further, before a Question/Answer sequence is fullfilled by the client.");
            }
        }

        private Parameter SituationalEvaluate(out IDynamicExpression e)
        {
            foreach (var function in _formula.Functions)
            {
                foreach (var item in _parameters)
                {
                    if (item.Name == function.Situation && bool.TryParse(item.Value.ToString(), out bool value) && value)
                    {
                        try
                        {
                            if (_formula.IsAutoFunc)
                            {
                                // do not compile, autofunc is derived from a user choice from step.
                                e = _context.CompileDynamic(function.Expression);
                                return item as Parameter;
                            }
                            else
                            {
                                e = _context.CompileDynamic(function.Expression);
                                var parameter = new Parameter(_formula.Name, e.Evaluate().Infer(), null, ref _model);
                                parameter.IsCalculated = true;
                                if (_parameters.GetParameter(_formula.Name) != null)
                                    // while reclaculating make sure to keep parameters distinct
                                    _parameters.Remove(_parameters.GetParameter(_formula.Name));

                                _parameters.Add(parameter);
                                if (_context.Variables.ContainsKey(parameter.Name))
                                {
                                    _context.Variables.Remove(parameter.Name);
                                }
                                _context.Variables.Add(parameter.Name, parameter.Value.Infer());
                                return parameter;
                            }
                        }
                        catch (ExpressionCompileException ex)
                        {
                            // Function can not evaluate further, before a Question/Answer sequence is fullfilled by the client.
                            throw new UnresolvedException($"Function {function.Expression} can not evaluate further, before a Question/Answer sequence is fullfilled by the client.", ex);
                        }
                    }
                }
            }
            StringBuilder situations = new StringBuilder();
            var parameters = new ParametersCollection();
            foreach (var function in _formula.Functions)
            {
                situations.Append(function.Situation + ",");
                var parameter = new Parameter(function.Situation, null, TypeInference.Infer(function).Type, ref _model);
                parameters.Add(parameter);
            }
            if (OnQuestion == null)
                throw new Exception($"In order to evaluate variable one of the following situations:  {situations.ToString().Trim(',')}, you need to provide a delegate callback to the client for providing an answer");
            OnQuestion(this, new QuestionArgs("", parameters));
            // situation has to be formulated as an input parameter by the client.
            throw new UnresolvedException($"Can't evaluate formula {_formula.Name} for situation. Please specify one of the following situations: {situations.ToString().Trim(',')}.");
        }

        private void ResolveVariableValue(object sender, ResolveVariableValueEventArgs e)
        {
            // resolve variable from formulas model.
            var recursiveFormula = (from p in _model.Formulas where p.Name == e.VariableName select p).SingleOrDefault();
            if (recursiveFormula != null)
            {
                // this variable is a formula. Recurvsively execute this formula.
                e.VariableValue = FormulaExpressionContext.Evaluate(this, ref _context, ref recursiveFormula, ref _parameters, OnQuestion, ref _model).Value.Infer();
                //var context = new FormulaExpressionContext(ref _model, ref _parameters, recursiveFormula, OnQuestion);
                //e.VariableValue = context.Evaluate().Value.Infer();
            }
            else
            {
                if (_parameters.Find(p => p.Name == e.VariableName) != null)
                    return;
                // this variable is an input variable. Provide a question to the client for an answer.
                if (OnQuestion == null)
                    throw new Exception($"In order to evaluate variable '${e.VariableName}', you need to provide a delegate callback to the client for providing an answer");
                OnQuestion(this, new QuestionArgs("", new ParametersCollection() { new Parameter(e.VariableName, null, TypeInference.Infer(e.VariableType).Type, ref _model) }));
            }
        }

        private void ResolveVariableType(object sender, ResolveVariableTypeEventArgs e)
        {
            // resolve variable from formulas model.
            var recursiveFormula = (from p in _model.Formulas where p.Name == e.VariableName select p).SingleOrDefault();
            if (recursiveFormula != null)
            {
                // this variable is a formula. Recurvsively execute this formula.
                // var context = new FormulaExpressionContext(ref _model, ref _parameters, recursiveFormula, OnQuestion);
                e.VariableType = typeof(double);
            }
            else
            {
                if (_parameters.Find(p => p.Name == e.VariableName) != null)
                    return;
                // this variable is an input variable. Provide a question to the client for an answer.
                if (OnQuestion == null)
                    throw new Exception($"In order to evaluate variable '${e.VariableName}', you need to provide a delegate callback to the client for providing an answer");
                OnQuestion(this, new QuestionArgs("", new ParametersCollection() { new Parameter(e.VariableName, null, TypeInference.Infer(e.VariableType).Type, ref _model) }));
            }
        }
    }
}
