using System;

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

    /*public TBuilder SetPredecessor(TPredecessor predecessor)
    {
        ArgumentNullException.ThrowIfNull(predecessor);

        PredecessorPredicate = actualPredecessor => actualPredecessor.Equals(predecessor);

        return _builderInstance;
    }*/

    public virtual void Reset()
    {
        PredecessorSymbol    = null;
        PredecessorPredicate = predecessor => true;
        ContextPredicate     = context => true;
    }

    /*#region Context
    public ProductionBuilder SetContext(ProductionContext context)
    {
        _productionContext = context ?? throw new ArgumentNullException(nameof(context));

        return this;
    }

    public ProductionBuilder SetContextPreviousModules(IEnumerable<Module> previousModules)
    {
        ArgumentNullException.ThrowIfNull(previousModules);

        _productionContext = new(previousModules, _productionContext.NextModules);

        return this;
    }

    public ProductionBuilder SetContextPreviousModule(Module previousModule)
    {
        ArgumentNullException.ThrowIfNull(previousModule);

        SetContextPreviousModules(new List<Module> { previousModule });

        return this;
    }

    public ProductionBuilder SetContextNextModules(IEnumerable<Module> nextModules)
    {
        ArgumentNullException.ThrowIfNull(nextModules);

        _productionContext = new(_productionContext.PreviousModules, nextModules);

        return this;
    }

    public ProductionBuilder SetContextNextModule(Module nextModule)
    {
        ArgumentNullException.ThrowIfNull(nextModule);

        SetContextNextModules(new List<Module> { nextModule });

        return this;
    }
    #endregion*/

    public abstract TProduction Build();
}
