using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystems;

public class RegularProductionBuilder<TPredecessor>
    : ProductionBuilder<TPredecessor, RegularProduction<TPredecessor>, RegularProductionBuilder<TPredecessor>>
    where TPredecessor : notnull, Module
{
    private ProductionMethod<TPredecessor>? _productionMethod;

    public override void Reset()
    {
        base.Reset();

        _productionMethod = null;
    }

    public RegularProductionBuilder<TPredecessor> SetProductionMethod(ProductionMethod<TPredecessor> productionMethod)
    {
        ArgumentNullException.ThrowIfNull(productionMethod);

        _productionMethod = productionMethod;

        return this;
    }

    #region SetSuccessors

    public RegularProductionBuilder<TPredecessor> SetSuccessors(IEnumerable<Module> successors)
    {
        ArgumentNullException.ThrowIfNull(successors);

        if (successors.Any(successor => successor is null))
            throw new ArgumentException("Sequence contains null elements.", nameof(successors));

        _productionMethod = (_, _) => successors;

        return this;
    }

    public RegularProductionBuilder<TPredecessor> SetSuccessors(params Module[] successors) =>
        SetSuccessors((IEnumerable<Module>)successors);

    #endregion

    public override RegularProduction<TPredecessor> Build()
    {
        if (PredecessorSymbol is null)
            throw new AggregateException("The predecessor symbol has not been set.");

        if (_productionMethod is null)
            throw new AggregateException("The production method has not been set.");

        var production = new RegularProduction<TPredecessor>(PredecessorSymbol.Value, PredecessorPredicate, ContextPredicate, _productionMethod);

        Reset();

        return production;
    }
}
