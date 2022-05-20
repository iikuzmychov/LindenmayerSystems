using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystems;

public class StochasticProductionBuilder<TPredecessor>
    : ProductionBuilder<TPredecessor, StochasticProduction<TPredecessor>, StochasticProductionBuilder<TPredecessor>>
    where TPredecessor : notnull, Module
{
    private List<ProductionMethodWithWeigth<TPredecessor>> _productionMethods;
    private Random? _random;

    public override void Reset()
    {
        base.Reset();

        _productionMethods = new();
        _random            = null;
    }

    public StochasticProductionBuilder<TPredecessor> AddProductionMethod(int weight, ProductionMethod<TPredecessor> productionMethod)
    {
        ArgumentNullException.ThrowIfNull(productionMethod);

        _productionMethods.Add(new(weight, productionMethod));

        return this;
    }

    public StochasticProductionBuilder<TPredecessor> AddIdentityProductionMethod(int weight) =>
        AddProductionMethod(weight, (module, _) => new Module[] { module });

    #region AddSuccessors

    public StochasticProductionBuilder<TPredecessor> AddSuccessors(int weight, IEnumerable<Module> successors)
    {
        ArgumentNullException.ThrowIfNull(successors);

        if (successors.Any(successor => successor is null))
            throw new ArgumentException("Sequence contains null elements.", nameof(successors));

        _productionMethods.Add(new(weight, (_, _) => successors));

        return this;
    }

    public StochasticProductionBuilder<TPredecessor> AddSuccessors(int weight, params Module[] successors) =>
        AddSuccessors(weight, (IEnumerable<Module>)successors);

    #endregion

    public StochasticProductionBuilder<TPredecessor> SetRandom(Random random)
    {
        _random = random ?? throw new ArgumentNullException(nameof(random));

        return this;
    }

    public override StochasticProduction<TPredecessor> Build()
    {
        if (PredecessorSymbol is null)
            throw new AggregateException("The predecessor symbol has not been set.");

        if (_productionMethods is null)
            throw new AggregateException("Production methods have not been set.");

        var production = new StochasticProduction<TPredecessor>(PredecessorSymbol.Value, PredecessorPredicate, ContextPredicate, _productionMethods, _random ?? new());

        Reset();

        return production;
    }
}