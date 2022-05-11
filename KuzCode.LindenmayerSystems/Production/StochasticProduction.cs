using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystems;

public record ProductionMethodWithWeigth<TPredecessor>
    where TPredecessor : notnull, Module
{
    public ProductionMethod<TPredecessor> Method { get; }
    public int Weight { get; }

    public ProductionMethodWithWeigth(ProductionMethod<TPredecessor> method, int weight)
    {
        ArgumentNullException.ThrowIfNull(nameof(method));
        
        if (weight <= 0)
            throw new ArgumentOutOfRangeException(nameof(weight));

        Method = method;
        Weight = weight;
    }
}

public class StochasticProduction<TPredecessor> : Production<TPredecessor>
    where TPredecessor : notnull, Module
{
    private readonly ProductionMethodWithWeigth<TPredecessor>[] _productionMethods;
    private readonly int _totalProductionMethodsWeight;
    private readonly Random _random;

    #region Constructors
    public StochasticProduction(Predicate<TPredecessor> predecessorPredicate, Predicate<ProductionContext> contextPredicate,
        IEnumerable<ProductionMethodWithWeigth<TPredecessor>> productionMethods, Random random)
        : base(predecessorPredicate, contextPredicate)
    {
        ArgumentNullException.ThrowIfNull(productionMethods);
        ArgumentNullException.ThrowIfNull(random);

        if (!productionMethods.Any())
            throw new ArgumentException("The sequence contains no elements.", nameof(productionMethods));

        _productionMethods            = productionMethods.OrderBy(method => method.Weight).ToArray();
        _totalProductionMethodsWeight = _productionMethods.Sum(method => method.Weight);
        _random                       = random;
    }

    public StochasticProduction(Predicate<TPredecessor> predecessorPredicate, Predicate<ProductionContext> contextPredicate,
        IEnumerable<ProductionMethodWithWeigth<TPredecessor>> productionMethods)
        : this(predecessorPredicate, contextPredicate, productionMethods, new()) { }
    #endregion

    protected override ProductionMethod<TPredecessor> GetProductionMethod()
    {
        var randomValue = _random.Next(1, _totalProductionMethodsWeight + 1);

        // binary search the randomly generated value for the choice of production method
        // adapted code from https://dotzero.blog/weighted-random-simple/
        var highIndex   = _productionMethods.Length;
        var lowIndex    = 0;
        int methodIndex;

        do
        {
            methodIndex = (highIndex + lowIndex) / 2;

            if (_productionMethods[methodIndex].Weight < randomValue)
                lowIndex = methodIndex + 1;
            else if (_productionMethods[methodIndex].Weight > randomValue)
                highIndex = methodIndex - 1;
            else
                return _productionMethods[methodIndex].Method;
        }
        while (lowIndex < highIndex);

        if (lowIndex != highIndex)
            return _productionMethods[methodIndex].Method;
        else if (_productionMethods[lowIndex].Weight >= randomValue)
            return _productionMethods[lowIndex].Method;
        else
            return _productionMethods[lowIndex + 1].Method;
    }
}
