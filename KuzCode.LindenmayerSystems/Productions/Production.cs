using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace KuzCode.LindenmayerSystems;

public delegate IEnumerable<Module> ProductionMethod<TPredecessor>(TPredecessor module, ProductionContext context)
    where TPredecessor : notnull, Module;

public abstract class Production<TPredecessor> : IProduction<TPredecessor>
    where TPredecessor : notnull, Module
{
    private readonly Predicate<TPredecessor> _predecessorPredicate;
    private readonly Predicate<ProductionContext> _contextPredicate;

    public char PredecessorSymbol { get; }

    public Production(char predecessorSymbol, Predicate<TPredecessor> predecessorPredicate, Predicate<ProductionContext> contextPredicate)
    {
        PredecessorSymbol     = predecessorSymbol;
        _predecessorPredicate = predecessorPredicate ?? throw new ArgumentNullException(nameof(predecessorPredicate));
        _contextPredicate     = contextPredicate ?? throw new ArgumentNullException(nameof(contextPredicate));
    }

    protected abstract ProductionMethod<TPredecessor> GetProductionMethod();

    #region GenerateSuccessors

    private IEnumerable<Module> GenerateSuccessorsWithoutNullChecking(TPredecessor predecessor, ProductionContext context)
    {
        if (predecessor.Symbol != PredecessorSymbol || !_predecessorPredicate(predecessor))
            throw new ArgumentException("The production cannot generate successors from the current predecessor.", nameof(predecessor));

        if (!_contextPredicate(context))
            throw new ArgumentException("The production cannot generate successors in the current context.", nameof(predecessor));

        var produceMethod = GetProductionMethod();
        var modules = produceMethod.Invoke(predecessor, context);

        return modules;
    }

    public IEnumerable<Module> GenerateSuccessors(TPredecessor predecessor, ProductionContext context)
    {
        ArgumentNullException.ThrowIfNull(predecessor);
        ArgumentNullException.ThrowIfNull(context);

        return GenerateSuccessorsWithoutNullChecking(predecessor, context);
    }

    #endregion

    #region TryGenerateSuccessors

    private bool TryGenerateSuccessorsWithoutNullChecking(TPredecessor predecessor, ProductionContext context, out IEnumerable<Module>? successors)
    {
        try
        {
            successors = GenerateSuccessorsWithoutNullChecking(predecessor, context);

            return true;
        }
        catch (ArgumentException)
        {
            successors = null;

            return false;
        }
    }

    public bool TryGenerateSuccessors(TPredecessor predecessor, ProductionContext context, out IEnumerable<Module>? successors)
    {
        ArgumentNullException.ThrowIfNull(predecessor);
        ArgumentNullException.ThrowIfNull(context);

        return TryGenerateSuccessorsWithoutNullChecking(predecessor, context, out successors);
    }

    bool IProduction<TPredecessor>.TryGenerateSuccessors(Module predecessor, ProductionContext context, out IEnumerable<Module>? successors)
    {
        ArgumentNullException.ThrowIfNull(predecessor);
        ArgumentNullException.ThrowIfNull(context);

        if (predecessor.GetType() != typeof(TPredecessor))
        {
            successors = null;

            return false;
        }

        return TryGenerateSuccessorsWithoutNullChecking((TPredecessor)predecessor, context, out successors);
    }

    #endregion
}
