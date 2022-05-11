using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KuzCode.LindenmayerSystems;

/// <summary>
/// Class representing the Lindenmayer system (L-system) - a parallel rewriing system.
/// </summary>
public class LSystem : ICloneable
{
    private readonly Module[] _axiom;
    private readonly IProduction<Module>[] _productions;
    private IList<Module> _state;

    /// <summary>
    /// Start state
    /// </summary>
    public IReadOnlyCollection<Module> Axiom => Array.AsReadOnly(_axiom);

    /// <summary>
    /// Current modules
    /// </summary>
    public IReadOnlyCollection<Module> State => new ReadOnlyCollection<Module>(_state);

    /// <summary>
    /// Producers
    /// </summary>
    public IReadOnlyCollection<IProduction<Module>> Productions => Array.AsReadOnly(_productions);

    /// <summary>
    /// Count of steps. Starts from zero. It increments when <see cref="NextStep"/> is called
    /// </summary>
    public int Step { get; private set; } = 0;

    /// <summary>
    /// The event that occurs when transforming a new module
    /// </summary>
    //public EventHandler<ModuleTransformedEventArgs>? ModuleTransformed;

    /// <param name="axiom">Start state</param>
    public LSystem(Module[] axiom, IProduction<Module>[] productions)
    {
        ArgumentNullException.ThrowIfNull(axiom);
        ArgumentNullException.ThrowIfNull(productions);

        if (axiom.Any(module => module is null))
            throw new ArgumentException("Axiom contains null modules.");

        _axiom       = axiom;
        _state       = _axiom;
        _productions = productions;
    }

    /// <summary>
    /// Take the next step. All modules from <see cref="State"/> will be transformed by producers
    /// </summary>
    /// <returns>New state</returns>
    public IReadOnlyCollection<Module> NextStep()
    {
        var newState     = new List<Module>(_state.Count);

        for (int i = 0; i < _state.Count; i++)
        {
            var predecessor  = _state[i];
            var leftContext  = _state.Take(i);
            var rigthContext = _state.Skip(i + 1);
            var context      = new ProductionContext(leftContext, rigthContext);

            IEnumerable<Module>? successors = null;

            foreach (var production in _productions)
            {
                if (production.TryGenerateSuccessors(predecessor, context, out successors))
                    break;
            }

            if (successors is null)
                newState.Add(predecessor);
            else
                newState.AddRange(successors);

            //ModuleTransformed?.Invoke(this, new(predecessor, successors));
        }

        _state = newState;
        Step++;

        return State;
    }

    /// <summary>
    /// Take the next steps. All modules from <see cref="State"/> will be transformed by producers <paramref name="stepsCount"/>-times
    /// </summary>
    /// <param name="stepsCount">Steps count</param>
    /// <returns></returns>
    public IReadOnlyCollection<Module> NextSteps(int stepsCount)
    {
        if (stepsCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(stepsCount));

        for (int i = 0; i < stepsCount; i++)
            NextStep();

        return State;
    }

    /// <summary>
    /// Reset system to start state. Count of steps resets to zero
    /// </summary>
    public void Reset()
    {
        _state = _axiom;
        Step   = 0;
    }

    public object Clone() => new LSystem(_axiom, _productions);
}
