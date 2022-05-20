using KuzCode.LindenmayerSystems.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KuzCode.LindenmayerSystems;

/// <summary>
/// Class representing the Lindenmayer system (L-system) - a parallel rewriing system.
/// </summary>
public class LSystem
{
    private readonly Module[] _axiom;
    private readonly Dictionary<char, IProduction<Module>[]> _productions;
    private IList<Module> _state;

    public ReadOnlyCollection<Module> Axiom => Array.AsReadOnly(_axiom);
    public ReadOnlyCollection<Module> State => new(_state);
    public ReadOnlyDictionary<char, IProduction<Module>[]> Productions => new(_productions);
    public int Step { get; private set; } = 0;

    public LSystem(Module[] axiom, IEnumerable<IProduction<Module>> productions)
    {
        ArgumentNullException.ThrowIfNull(axiom);
        ArgumentNullException.ThrowIfNull(productions);

        if (axiom.Any(module => module is null))
            throw new ArgumentException("Sequence contains null elements.", nameof(axiom));

        if (productions.Any(production => production is null))
            throw new ArgumentException("Sequence contains null elements.", nameof(productions));

        _axiom = axiom;
        _state = _axiom;

        _productions = productions
            .GroupBy(production => production.PredecessorSymbol)
            .ToDictionary(group => group.Key, group => group.ToArray());
    }

    public static IEnumerable<Module> EnumerateAsModules(string @string, bool ignoreWhiteSpaces = true)
    {
        ArgumentNullException.ThrowIfNull(@string);

        foreach (var symbol in @string)
        {
            if (!ignoreWhiteSpaces || char.IsWhiteSpace(symbol))
                yield return new Module(symbol);
        }
    }

    public static Module[] ParseAsModules(string @string, bool ignoreWhiteSpaces = true)
    {
        ArgumentNullException.ThrowIfNull(@string);

        int modulesCount;
        IEnumerator<char> symbolsEnumerator;

        if (ignoreWhiteSpaces)
        {
            var symbolsWithoutSpaces = @string.Where(symbol => !char.IsWhiteSpace(symbol));

            modulesCount = symbolsWithoutSpaces.Count();
            symbolsEnumerator = symbolsWithoutSpaces.GetEnumerator();
        }
        else
        {
            modulesCount = @string.Length;
            symbolsEnumerator = @string.GetEnumerator();
        }

        var modules = new Module[modulesCount];

        for (int i = 0; i < modulesCount; i++)
        {
            symbolsEnumerator.MoveNext();

            modules[i] = new Module(symbolsEnumerator.Current);
        }

        return modules;
    }

    public ReadOnlyCollection<Module> NextSteps(int stepsCount)
    {
        if (stepsCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(stepsCount));

        for (int i = 0; i < stepsCount; i++)
        {
            var newState = new List<Module>(_state.Count);

            for (int moduleIndex = 0; moduleIndex < _state.Count; moduleIndex++)
            {
                var predecessor = _state[moduleIndex];
                var successors  = (IEnumerable<Module>?)null;

                if (_productions.TryGetValue(predecessor.Symbol, out var productions))
                {
                    var reversedLeftContext = _state.TakeBackwards(moduleIndex - 1);
                    var rigthContext        = _state.Skip(moduleIndex + 1);
                    var context             = new ProductionContext(reversedLeftContext, rigthContext);

                    foreach (var production in productions!)
                    {
                        if (production.TryGenerateSuccessors(predecessor, context, out successors))
                            break;
                    }
                }

                if (successors is null)
                    newState.Add(predecessor);
                else
                    newState.AddRange(successors);
            }

            _state = newState;
        }

        Step += stepsCount;

        return State;
    }

    public ReadOnlyCollection<Module> NextStep() => NextSteps(1);

    public void Reset()
    {
        _state = _axiom;
        Step = 0;
    }
}
