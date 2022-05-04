using System;
using System.Collections.Generic;

namespace KuzCode.LindenmayerSystem;

/// <summary>
/// Class representing <seealso cref="Module"/> transformation rules
/// </summary>
public class Producer<TModule> : IProducer<TModule>, ICloneable
    where TModule : Module
{
    private readonly ProductionMethod<TModule> _produceMethod;

    public Producer(ProductionMethod<TModule> produceMethod)
    {
        _produceMethod = produceMethod ?? throw new ArgumentNullException(nameof(produceMethod));
    }

    protected virtual ProductionMethod<TModule> GetProduceMethod() => _produceMethod;

    public List<Module> Produce(TModule module, ProductionContext context)
    {
        ArgumentNullException.ThrowIfNull(module);
        ArgumentNullException.ThrowIfNull(context);

        var produceMethod = GetProduceMethod();
        var modules       = produceMethod.Invoke(module, context);

        return modules;
    }

    List<Module> IProducer<TModule>.Produce(Module module, ProductionContext context)
    {
        ArgumentNullException.ThrowIfNull(module);
        ArgumentNullException.ThrowIfNull(context);

        if (module is not TModule)
            throw new ArgumentException("");

        return Produce((TModule)module, context);
    }

    object ICloneable.Clone() => new Producer<TModule>((ProductionMethod<TModule>)_produceMethod.Clone());
}
