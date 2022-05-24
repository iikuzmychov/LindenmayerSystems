using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystems;

public class ProductionContext : IEquatable<ProductionContext>
{
    public IEnumerable<Module> Left { get; }
    public IEnumerable<Module> Right { get; }

    public static ProductionContext Empty => new(Enumerable.Empty<Module>(), Enumerable.Empty<Module>());

    public ProductionContext(IEnumerable<Module> left, IEnumerable<Module> right)
    {
        Left = left ?? throw new ArgumentNullException(nameof(left));
        Right        = right ?? throw new ArgumentNullException(nameof(right));
    }

    public static bool IsContextMatchOther(IEnumerable<Module> context, IEnumerable<Module> otherContext)
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

    public bool IsMatchOtherContext(ProductionContext otherContext)
    {
        ArgumentNullException.ThrowIfNull(otherContext);
        
        return IsContextMatchOther(Left, otherContext.Left) && IsContextMatchOther(Right, otherContext.Right);
    }

    #region Comparing

    public bool Equals(ProductionContext? otherContext)
    {
        if (otherContext is null)
            return false;

        if (ReferenceEquals(this, otherContext))
            return true;

        return
            otherContext.Left.SequenceEqual(Left) &&
            otherContext.Right.SequenceEqual(Right);
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

    public override int GetHashCode() => (Left, Right).GetHashCode();

    #endregion

    public override string ToString() =>
        string.Join(", ", Left) + " < X > " + string.Join(", ", Right);
}
