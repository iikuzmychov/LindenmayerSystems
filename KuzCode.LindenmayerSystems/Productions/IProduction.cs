﻿using System.Collections.Generic;

namespace KuzCode.LindenmayerSystems;

public interface IProduction<out TPredecessor>
    where TPredecessor : notnull, Module
{
    public char PredecessorSymbol { get; }

    public bool TryGenerateSuccessors(Module predecessor, ProductionContext context, out IEnumerable<Module>? successors);
}
