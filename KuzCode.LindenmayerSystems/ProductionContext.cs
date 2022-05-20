using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystems;

public class ProductionContext : IEquatable<ProductionContext>
{
    public static ProductionContext NoContext =>
        new(Enumerable.Empty<Module>(), Enumerable.Empty<Module>());

    public IEnumerable<Module> ReversedLeftContext { get; }
    public IEnumerable<Module> RightContext { get; }

    public ProductionContext(IEnumerable<Module> reversedLeftContext, IEnumerable<Module> rightContext)
    {
        ReversedLeftContext = reversedLeftContext ?? throw new ArgumentNullException(nameof(reversedLeftContext));
        RightContext        = rightContext ?? throw new ArgumentNullException(nameof(rightContext));
    }

    private static bool IsContextMatchOther(IEnumerable<Module> context, IEnumerable<Module> otherContext)
    {
        using (var currentPreviousModulesEnumerator = context.GetEnumerator())
        using (var otherPreviousModulesEnumerator   = otherContext.GetEnumerator())
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

        return true;
    }

    public bool IsMatchContext(ProductionContext otherContext)
    {
        ArgumentNullException.ThrowIfNull(otherContext);
        
        return IsContextMatchOther(ReversedLeftContext, otherContext.ReversedLeftContext) &&
            IsContextMatchOther(RightContext, otherContext.RightContext);
    }

    #region Comparing

    public bool Equals(ProductionContext? otherContext)
    {
        if (otherContext is null)
            return false;

        if (ReferenceEquals(this, otherContext))
            return true;

        return
            otherContext.ReversedLeftContext.SequenceEqual(ReversedLeftContext) &&
            otherContext.RightContext.SequenceEqual(RightContext);
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

    public override int GetHashCode() => (ReversedLeftContext, RightContext).GetHashCode();

    #endregion

    public override string ToString() =>
        string.Join(", ", ReversedLeftContext) + " < X > " + string.Join(", ", RightContext);
}
