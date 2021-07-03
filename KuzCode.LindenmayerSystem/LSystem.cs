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

            if (axiom.Any(module => module.IsUseableAsTemplateOnly))
                throw new ArgumentException("Axiom sequence contains modules useable only as templates", nameof(axiom));

            if (producers is null)
                throw new ArgumentNullException(nameof(axiom));

            if (producers.Any(producer => producer is null))
                throw new ArgumentException("Sequence contains null-objects", nameof(producers));

            if (producers
                .Any(producer1 => producers
                    .Any(producer2 => producer1.ModuleTemplate
                        .IsMatchTemplate(producer2.ModuleTemplate))))
            {
                throw new ArgumentException("Producers sequence contains producers with the same templates or more stringent templates", nameof(producers));
            }

            _axiom     = axiom;
            _state     = axiom;
            _producers = producers;
        }

        public IReadOnlyList<Module> NextStep()
        {
            var newState = new List<Module>();

            for (int i = 0; i < _state.Count; i++)
            {
                var currentModule = _state[i];

                var producer = _producers
                    .SingleOrDefault(producer => currentModule
                        .IsMatchTemplate(producer.ModuleTemplate));

                if (producer is not null)
                {
                    var previousModules = _state.Take(i).ToList();
                    var nextModules     = _state.Skip(i + 1).ToList();
                    var context         = new Producer.Context(previousModules, nextModules);
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

        public object Clone() => new LSystem(new(_axiom), _producers.Clone());
    }
}
