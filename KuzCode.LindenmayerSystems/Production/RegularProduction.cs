using System;

namespace KuzCode.LindenmayerSystems;

public class RegularProduction<TPredecessor> : Production<TPredecessor>
    where TPredecessor : notnull, Module
{
    private readonly ProductionMethod<TPredecessor> _productionMethod;

    public RegularProduction(Predicate<TPredecessor> predecessorPredicate,
        Predicate<ProductionContext> contextPredicate, ProductionMethod<TPredecessor> productionMethod)
        : base(predecessorPredicate, contextPredicate)
    {
        _productionMethod = productionMethod ?? throw new ArgumentNullException(nameof(productionMethod));
    }

    protected override ProductionMethod<TPredecessor> GetProductionMethod() => _productionMethod;
}
