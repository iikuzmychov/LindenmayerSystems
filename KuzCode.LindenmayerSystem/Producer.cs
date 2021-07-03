using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystem
{
    public delegate List<Module> ProduceMethod(Module module);

    /// <summary>
    /// Class representing <seealso cref="LindenmayerSystem.Module"/> transformation rules
    /// </summary>
    public class Producer : ICloneable
    {
        #region Context
        public class Context : ICloneable
        {
            private readonly List<ModuleTemplate> _previousModules;
            private readonly List<ModuleTemplate> _nextModules;

            public IReadOnlyList<ModuleTemplate> PreviousModules => _previousModules.AsReadOnly();
            public IReadOnlyList<ModuleTemplate> NextModules => _nextModules.AsReadOnly();

            public static Context NoContext => new(new(), new());

            public Context(List<ModuleTemplate> previousModules, List<ModuleTemplate> nextModules)
            {
                if (previousModules is null)
                    throw new ArgumentNullException(nameof(previousModules));

                if (previousModules.Any(module => module is null))
                    throw new ArgumentException("Sequence contains null-objects", nameof(previousModules));

                if (nextModules is null)
                    throw new ArgumentNullException(nameof(nextModules));

                if (nextModules.Any(module => module is null))
                    throw new ArgumentException("Sequence contains null-objects", nameof(nextModules));

                _previousModules = previousModules;
                _nextModules     = nextModules;
            }

            /// <returns><see langword="true"/> if <paramref name="otherContext"/> same or more stricted than current context</returns>
            public bool IsSuitableForOther(Context otherContext)
            {
                if (otherContext is null)
                    throw new ArgumentNullException(nameof(otherContext));

                if (otherContext._previousModules.Count > _previousModules.Count)
                    return false;

                // comparing otherContext._previousModules and last N modules this._previousModules. N = difference of otherContext._previousModules
                if (otherContext._previousModules
                    .SequenceEqual(_previousModules
                        .Skip(_previousModules.Count - otherContext._previousModules.Count)) == false)
                {
                    return false;
                }

                if (otherContext._nextModules.Count > _nextModules.Count)
                    return false;

                // comparing otherContext._previousModules and first N modules this._previousModules. N = count of otherContext._nextModules
                if (otherContext._nextModules
                    .SequenceEqual(_nextModules
                        .Take(otherContext._nextModules.Count)) == false)
                {
                    return false;
                }

                return true;
            }

            #region Clone
            object ICloneable.Clone() => Clone();

            public Context Clone() => new(new(_previousModules), new(_nextModules));
            #endregion

            #region Equals
            public override bool Equals(object obj)
            {
                var context = obj as Context;

                if (context is null)
                    return false;

                if (ReferenceEquals(this, context))
                    return true;

                if (GetType() != context.GetType())
                    return false;

                return context._previousModules.SequenceEqual(_previousModules) && context._nextModules.SequenceEqual(_nextModules);
            }

            public static bool operator ==(Context context1, Context context2)
            {
                if (context1 is null)
                {
                    if (context2 is null)
                        return true;
                    else
                        return false;
                }

                return context1.Equals(context2);
            }

            public static bool operator !=(Context context1, Context context2)
                => !(context1 == context2);
            #endregion            

            public override int GetHashCode() => (_previousModules, _nextModules).GetHashCode();

            public override string ToString()
                => string.Join(", ", _previousModules) + " < X > " + string.Join(", ", _nextModules);
        }
        #endregion

        public ModuleTemplate ModuleTemplate { get; }
        public ProduceMethod ProduceMethod { get; }

        /// <summary>
        /// The context in which the transformation of the module will be performed
        /// </summary>
        public Context ProductionContext { get; }

        public Producer(ModuleTemplate moduleTemplate, ProduceMethod produceMethod, Context productionContext)
        {
            if (moduleTemplate is null)
                throw new ArgumentNullException(nameof(moduleTemplate));

            if (produceMethod is null)
                throw new ArgumentNullException(nameof(produceMethod));

            if (productionContext is null)
                throw new ArgumentNullException(nameof(productionContext));

            ModuleTemplate    = moduleTemplate;
            ProduceMethod     = produceMethod;
            ProductionContext = productionContext;
        }

        public Producer(ModuleTemplate moduleTemplate, ProduceMethod produceMethod)
            : this(moduleTemplate, produceMethod, Context.NoContext) { }

        public Producer(ModuleTemplate moduleTemplate, List<Module> successors, Context productionContext)
            : this(moduleTemplate, module => successors, productionContext)
        {
            if (successors is null)
                throw new ArgumentNullException(nameof(successors));

            if (successors.Any(module => module is null))
                throw new ArgumentException("Sequence contains null-objects", nameof(successors));
        }

        public Producer(ModuleTemplate moduleTemplate, List<Module> successors)
            : this(moduleTemplate, successors, Context.NoContext) { }

        public Producer(ModuleTemplate moduleTemplate, Module successor, Context productionContext)
            : this(moduleTemplate, new List<Module> { successor }, productionContext) { }

        public Producer(ModuleTemplate moduleTemplate, Module successor)
            : this(moduleTemplate, successor, Context.NoContext) { }

        public virtual List<Module> Produce(Module module, Context context)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));

            if (module.GetTemplate() != ModuleTemplate)
                throw new ArgumentException("Module do not suit template");

            if (context is null)
                throw new ArgumentNullException(nameof(context));

            if (context.IsSuitableForOther(ProductionContext))
            {
                var successors = ProduceMethod.Invoke(module);

                if (successors is null)
                    throw new AggregateException("Successors sequence is null");

                if (successors.Any(module => module is null))
                    throw new AggregateException("Successors sequence contains null-objects");

                return successors;
            }
            else
            {
                return new List<Module> { module.Clone() };
            }
        }
        
        #region Clone
        object ICloneable.Clone() => Clone();

        public object Clone() =>
            new Producer(ModuleTemplate, ProduceMethod, ProductionContext.Clone());
        #endregion
    }
}
