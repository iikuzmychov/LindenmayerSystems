using System;
using System.Collections.Generic;

namespace KuzCode.LindenmayerSystems;

public abstract class ProductionBuilder<TPredecessor, TProduction, TBuilder>
    where TPredecessor : notnull, Module
    where TProduction  : Production<TPredecessor>
    where TBuilder     : ProductionBuilder<TPredecessor, TProduction, TBuilder>
{
    private readonly TBuilder _builderInstance;

    protected char? PredecessorSymbol;
    protected Predicate<TPredecessor> PredecessorPredicate;
    protected Predicate<ProductionContext> ContextPredicate;

    public ProductionBuilder()
    {
        _builderInstance = (TBuilder)this;

        Reset();
    }

    public TBuilder SetPredecessorSymbol(char predecessorSymbol)
    {
        PredecessorSymbol = predecessorSymbol;

        return _builderInstance;
    }

    public TBuilder SetPredecessorPredicate(Predicate<TPredecessor> predecessorPredicate)
    {
        PredecessorPredicate = predecessorPredicate ?? throw new ArgumentNullException(nameof(predecessorPredicate));

        return _builderInstance;
    }

    public TBuilder SetContextPredicate(Predicate<ProductionContext> contextPredicate)
    {
        ContextPredicate = contextPredicate ?? throw new ArgumentNullException(nameof(contextPredicate));

        return _builderInstance;
    }

    public TBuilder ConfigureContextPredicate(Action<ProductionContextPredicateBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        var contextPredicateBuilder = new ProductionContextPredicateBuilder();
        configure(contextPredicateBuilder);

        ContextPredicate = contextPredicateBuilder.Build();

        return _builderInstance;
    }

    public virtual void Reset()
    {
        PredecessorSymbol    = null;
        PredecessorPredicate = predecessor => true;
        ContextPredicate     = context => true;
    }

    public abstract TProduction Build();
}
