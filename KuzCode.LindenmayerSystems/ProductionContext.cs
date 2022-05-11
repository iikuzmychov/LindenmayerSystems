using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystems;

public class ProductionContext : /*ICloneable,*/ IEquatable<ProductionContext>
{
    public static ProductionContext NoContext =>
        new(Enumerable.Empty<Module>(), Enumerable.Empty<Module>());

    public IEnumerable<Module> PreviousModules { get; }
    public IEnumerable<Module> NextModules { get; }

    public ProductionContext(IEnumerable<Module> previousModules, IEnumerable<Module> nextModules)
    {
        PreviousModules = previousModules ?? throw new ArgumentNullException(nameof(previousModules));
        NextModules     = nextModules ?? throw new ArgumentNullException(nameof(nextModules));
    }

    public bool IsMatchContext(ProductionContext otherContext)
    {
        ArgumentNullException.ThrowIfNull(otherContext);

        using (var currentPreviousModulesEnumerator = PreviousModules.Reverse().GetEnumerator())
        using (var otherPreviousModulesEnumerator   = otherContext.PreviousModules.Reverse().GetEnumerator())
        {
            while (otherPreviousModulesEnumerator.MoveNext())
            {
                if (!currentPreviousModulesEnumerator.MoveNext() ||
                    !currentPreviousModulesEnumerator.Current.Equals(otherPreviousModulesEnumerator.Current))
                {
                    return false;
                }
            }
        }

        using (var currentNextModulesEnumerator = NextModules.GetEnumerator())
        using (var otherNextModulesEnumerator   = otherContext.NextModules.GetEnumerator())
        {
            while (otherNextModulesEnumerator.MoveNext())
            {
                if (!currentNextModulesEnumerator.MoveNext() ||
                    !currentNextModulesEnumerator.Current.Equals(otherNextModulesEnumerator.Current))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /*#region Cloning
    public object Clone() =>
        new ProductionContext(
            PreviousModules.Select(module => (Module)module.Clone()),
            NextModules.Select(module => (Module)module.Clone()));

    object ICloneable.Clone() => Clone();
    #endregion*/

    #region Comparing
    public bool Equals(ProductionContext? otherContext)
    {
        if (otherContext is null)
            return false;

        if (ReferenceEquals(this, otherContext))
            return true;

        return
            otherContext.PreviousModules.SequenceEqual(PreviousModules) &&
            otherContext.NextModules.SequenceEqual(NextModules);
    }

    public override bool Equals(object? obj) => Equals(obj as ProductionContext);

    public static bool operator ==(ProductionContext context1, ProductionContext context2)
    {
        if (context1 is null)
            return context2 is null;
        else
            return context1.Equals(context2);
    }

    public static bool operator !=(ProductionContext context1, ProductionContext context2) => !(context1 == context2);

    public override int GetHashCode() => (PreviousModules, NextModules).GetHashCode();
    #endregion

    public override string ToString() =>
        string.Join(", ", PreviousModules) + " < X > " + string.Join(", ", NextModules);
}
