using System;
using System.Collections.Generic;

namespace KuzCode.LindenmayerSystem
{
    public class ModuleTransformedEventArgs : EventArgs
    {
        private readonly List<Module> _successors;

        public Module Module { get; }
        public IReadOnlyList<Module> Successors => _successors.AsReadOnly();

        public ModuleTransformedEventArgs(Module module, List<Module> successors)
        {
            Module      = module ?? throw new ArgumentNullException(nameof(module));
            _successors = successors ?? throw new ArgumentNullException(nameof(successors));
        }
    }
}
