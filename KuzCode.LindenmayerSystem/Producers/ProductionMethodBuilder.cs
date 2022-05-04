using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystem;

public delegate List<Module> ProductionMethod<TModule>(TModule module, ProductionContext context)
        where TModule : Module;

public class ProductionMethodBuilder<TModule>
    where TModule : Module
{
    private Dictionary<TModule, List<Module>> _productionCases = new();
    private ProductionContext _productionContext = ProductionContext.NoContext;

    public void Reset()
    {
        _productionCases.Clear();
        _productionContext = ProductionContext.NoContext;
    }

    #region SetContext
    public ProductionMethodBuilder<TModule> SetContext(ProductionContext context)
    {
        _productionContext = context ?? throw new ArgumentNullException(nameof(context));

        return this;
    }

    public ProductionMethodBuilder<TModule> SetContextPreviousModules(IEnumerable<Module> previousModules)
    {
        ArgumentNullException.ThrowIfNull(previousModules);

        _productionContext = new(previousModules, _productionContext.NextModules);

        return this;
    }

    public ProductionMethodBuilder<TModule> SetContextPreviousModule(Module previousModule)
    {
        ArgumentNullException.ThrowIfNull(previousModule);

        return SetContextPreviousModules(new List<Module> { previousModule });
    }

    public ProductionMethodBuilder<TModule> SetContextNextModules(IEnumerable<Module> nextModules)
    {
        ArgumentNullException.ThrowIfNull(nextModules);

        _productionContext = new(_productionContext.PreviousModules, nextModules);

        return this;
    }

    public ProductionMethodBuilder<TModule> SetContextNextModule(Module nextModule)
    {
        ArgumentNullException.ThrowIfNull(nextModule);

        return SetContextNextModules(new List<Module> { nextModule });
    }
    #endregion

    #region AddCase
    public ProductionMethodBuilder<TModule> AddCase(TModule module, List<Module> producedModules)
    {
        ArgumentNullException.ThrowIfNull(module);
        ArgumentNullException.ThrowIfNull(producedModules);

        if (_productionCases.ContainsKey(module))
            throw new ArgumentException("Case with the same module already exists.", nameof(module));

        if (producedModules.Any(module => module is null))
            throw new ArgumentException($"'{nameof(producedModules)}' sequence contains a null module.");

        _productionCases.Add(module, producedModules);

        return this;
    }

    public ProductionMethodBuilder<TModule> AddCase(TModule module, Module producedModule)
    {
        ArgumentNullException.ThrowIfNull(module);
        ArgumentNullException.ThrowIfNull(producedModule);

        return AddCase(module, new List<Module> { producedModule });
    }
    #endregion

    public ProductionMethod<TModule> Build()
    {
        var productionCases   = _productionCases.ToDictionary(@case => @case.Key, @case => @case.Value);
        var productionContext = (ProductionContext)_productionContext.Clone();

        ProductionMethod<TModule> method = (module, context) =>
        {
            if (productionCases.ContainsKey(module) && context.IsMatchContext(productionContext))
            {
                return productionCases[module];
            }
            else
            {
                //return new List<Module> { (Module)module.Clone() };
                return new List<Module> { module };
            }
        };

        Reset();

        return method;
    }
}
