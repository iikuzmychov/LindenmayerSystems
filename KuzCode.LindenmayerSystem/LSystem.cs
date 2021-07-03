using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystem
{
    public class LSystem : ICloneable
    {
        private readonly List<Module> _axiom;
        private List<Module> _state;
        private readonly List<Producer> _producers;
        private readonly Dictionary<ModuleTemplate, Producer> _producersDictionary;

        public IReadOnlyList<Module> Axiom => _axiom.AsReadOnly();
        public IReadOnlyList<Module> State => _state.AsReadOnly();
        public IReadOnlyList<Producer> Producers => _producers.AsReadOnly();
        public int Step { get; private set; }

        public LSystem(List<Module> axiom, List<Producer> producers)
        {
            if (axiom is null)
                throw new ArgumentNullException(nameof(axiom));

            if (axiom.Any(axiom => axiom is null))
                throw new ArgumentException("Sequence contains null-objects", nameof(axiom));

            if (producers is null)
                throw new ArgumentNullException(nameof(axiom));

            if (producers.Any(producer => producer is null))
                throw new ArgumentException("Sequence contains null-objects", nameof(producers));

            if (producers.GroupBy(producer => producer.ModuleTemplate).Any(group => group.Count() > 1))
                throw new ArgumentException("Producers sequence contains more than 1 producer for same module", nameof(producers));

            _axiom               = axiom;
            _state               = axiom;
            _producers           = producers;
            _producersDictionary = producers.ToDictionary(key => key.ModuleTemplate);
        }

        public IReadOnlyList<Module> NextStep()
        {
            var newState = new List<Module>();

            for (int i = 0; i < _state.Count; i++)
            {
                var currentModule = _state[i];

                if (_producersDictionary.ContainsKey(currentModule.GetTemplate()))
                {
                    var previousModules = _state.Take(i).Select(module => module.GetTemplate()).ToList();
                    var nextModules     = _state.Skip(i + 1).Select(module => module.GetTemplate()).ToList();
                    var context         = new Producer.Context(previousModules, nextModules);
                    var producer        = _producersDictionary[currentModule.GetTemplate()];
                    var newModules      = producer.Produce(currentModule, context);

                    newState.AddRange(newModules);
                }
                else
                {
                    newState.Add(currentModule);
                }
            }

            _state = newState;
            Step++;

            return State;
        }

        public object Clone()
        {
            return new LSystem(_axiom.Clone(), _producers.Clone());
        }
    }
}
