using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystems;

public record ProductionMethodWithWeigth<TPredecessor>
    where TPredecessor : notnull, Module
{
    public int Weight { get; }
    public ProductionMethod<TPredecessor> Method { get; }

    public ProductionMethodWithWeigth(int weight, ProductionMethod<TPredecessor> method)
    {
        ArgumentNullException.ThrowIfNull(nameof(method));
        
        if (weight <= 0)
            throw new ArgumentOutOfRangeException(nameof(weight));

        Weight = weight;
        Method = method;
    }
}

public class StochasticProduction<TPredecessor> : Production<TPredecessor>
    where TPredecessor : notnull, Module
{
    private readonly ProductionMethodWithWeigth<TPredecessor>[] _productionMethods;
    private readonly int _totalProductionMethodsWeight;
    private readonly Random _random;

    #region Constructors

    public StochasticProduction(char predecessorSymbol, Predicate<TPredecessor> predecessorPredicate,
        Predicate<ProductionContext> contextPredicate, IEnumerable<ProductionMethodWithWeigth<TPredecessor>> productionMethods,
        Random random)
        : base(predecessorSymbol, predecessorPredicate, contextPredicate)
    {
        ArgumentNullException.ThrowIfNull(productionMethods);
        ArgumentNullException.ThrowIfNull(random);

        if (!productionMethods.Any())
            throw new ArgumentException("The sequence contains no elements.", nameof(productionMethods));

        _productionMethods            = productionMethods.OrderBy(method => method.Weight).ToArray();
        _totalProductionMethodsWeight = _productionMethods.Sum(method => method.Weight);
        _random                       = random;
    }

    public StochasticProduction(char predecessorSymbol, Predicate<TPredecessor> predecessorPredicate,
        Predicate<ProductionContext> contextPredicate, IEnumerable<ProductionMethodWithWeigth<TPredecessor>> productionMethods)
        : this(predecessorSymbol, predecessorPredicate, contextPredicate, productionMethods, new()) { }

    #endregion

    protected override ProductionMethod<TPredecessor> GetProductionMethod()
    {
        if (_productionMethods.Length == 1)
            return _productionMethods[0].Method;

        // binary search the randomly generated value for the choice of production method
        // adapted code from https://dotzero.blog/weighted-random-simple/
        var randomValue = _random.Next(1, _totalProductionMethodsWeight + 1);
        var highIndex   = _productionMethods.Length - 1;
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
        else if (_productionMethods[lowIndex].Weight >= randomValue || _productionMethods.Length <= lowIndex + 1)
            return _productionMethods[lowIndex].Method;
        else
            return _productionMethods[lowIndex + 1].Method;
    }
}
