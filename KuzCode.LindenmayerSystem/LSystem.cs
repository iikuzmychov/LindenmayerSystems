using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystem;

/// <summary>
/// Class representing Lindenmayer system (L-system) - parallel rewriting system.
/// </summary>
public class LSystem : ICloneable
{
    private readonly List<Module> _axiom;
    private readonly List<IProducer<Module>> _producers;
    private List<Module> _state;

    /// <summary>
    /// Start state
    /// </summary>
    public IReadOnlyList<Module> Axiom => _axiom.AsReadOnly();

    /// <summary>
    /// Current modules
    /// </summary>
    public IReadOnlyList<Module> State => _state.AsReadOnly();

    /// <summary>
    /// Producers
    /// </summary>
    public IReadOnlyList<IProducer<Module>> Producers => _producers.AsReadOnly();

    /// <summary>
    /// Count of steps. Starts from zero. It increments when <see cref="NextStep"/> is called
    /// </summary>
    public int Step { get; private set; }

    /// <summary>
    /// The event that occurs when transforming a new module
    /// </summary>
    public EventHandler<ModuleTransformedEventArgs>? ModuleTransformed;

    /// <param name="axiom">Start state</param>
    public LSystem(List<Module> axiom, List<IProducer<Module>> producers)
    {
        ArgumentNullException.ThrowIfNull(axiom);
        ArgumentNullException.ThrowIfNull(producers);

        if (axiom.Any(module => module is null))
            throw new ArgumentException("Axiom contains null modules.");

        _axiom     = axiom;
        _state     = new List<Module>(axiom);
        _producers = producers;
    }

    /// <summary>
    /// Take the next step. All modules from <see cref="State"/> will be transformed by producers
    /// </summary>
    /// <returns>New state</returns>
    public IReadOnlyList<Module> NextStep()
    {
        var newState = new List<Module>();

        for (int i = 0; i < _state.Count; i++)
        {
            List<Module>? newModules = null;

            var currentModule     = _state[i];
            var currentModuleType = currentModule.GetType();
            var previousModules   = _state.Take(i);
            var nextModules       = _state.Skip(i + 1);
            var currentContext    = new ProductionContext(previousModules, nextModules);

            foreach (var producer in _producers)
            {
                var producerModuleType = producer.GetInputModuleType();

                if (currentModuleType != producerModuleType && !currentModuleType.IsSubclassOf(producerModuleType))
                    continue;

                var producedModules = producer.Produce(currentModule, currentContext);

                if (producedModules.Count == 1 && producedModules[0] == currentModule)
                {
                    continue;
                }
                else
                {
                    newModules = producedModules;
                    break;
                }
            }

            if (newModules is null)
                newModules = new List<Module> { currentModule };

            ModuleTransformed?.Invoke(this, new(currentModule, newModules));
            newState.AddRange(newModules);
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
    public IReadOnlyList<Module> NextSteps(int stepsCount)
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
        _state = new(_axiom);
        Step = 0;
    }

    public object Clone() => new LSystem(new(_axiom), _producers);
}
