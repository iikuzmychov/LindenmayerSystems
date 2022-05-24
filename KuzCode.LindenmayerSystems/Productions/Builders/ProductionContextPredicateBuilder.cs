using KuzCode.LindenmayerSystems.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystems;

public class ProductionContextPredicateBuilder
{
    private Predicate<IEnumerable<Module>> _leftPredicate;
    private Predicate<IEnumerable<Module>> _rightPredicate;

    public ProductionContextPredicateBuilder()
    {
        Reset();
    }

    private static Predicate<IEnumerable<Module>> CreateModulesPredicate(int modulesCount, Predicate<IList<Module>> predicate)
    {
        Predicate<IEnumerable<Module>> modulesPredicate = context =>
        {
            var contextArray = context.TakeToArray(modulesCount);

            if (contextArray.Length == modulesCount)
                return predicate(contextArray);
            else
                return false;
        };

        return modulesPredicate;
    }

    #region SetLeftPredicate

    public ProductionContextPredicateBuilder SetLeftPredicate(Predicate<IEnumerable<Module>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        _leftPredicate = predicate;

        return this;
    }

    public ProductionContextPredicateBuilder SetLeftPredicate(int modulesCount, Predicate<IList<Module>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (modulesCount < 1)
            throw new ArgumentOutOfRangeException(nameof(modulesCount));

        _leftPredicate = CreateModulesPredicate(modulesCount, predicate);

        return this;
    }

    #endregion

    #region SetLeft

    public ProductionContextPredicateBuilder SetLeft(IEnumerable<Module> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);

        modules = modules.Reverse();

        _leftPredicate = actualLeftContext =>
            ProductionContext.IsContextMatchOther(actualLeftContext, modules);

        return this;
    }

    public ProductionContextPredicateBuilder SetLeft(params Module[] modules) =>
        SetLeft((IEnumerable<Module>)modules);

    #endregion

    #region SetRightPredicate

    public ProductionContextPredicateBuilder SetRightPredicate(Predicate<IEnumerable<Module>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        _rightPredicate = predicate;

        return this;
    }

    public ProductionContextPredicateBuilder SetRightPredicate(int modulesCount, Predicate<IList<Module>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (modulesCount < 1)
            throw new ArgumentOutOfRangeException(nameof(modulesCount));

        _rightPredicate = CreateModulesPredicate(modulesCount, predicate);

        return this;
    }

    #endregion

    #region SetRightContext

    public ProductionContextPredicateBuilder SetRightContext(IEnumerable<Module> rightContext)
    {
        ArgumentNullException.ThrowIfNull(rightContext);

        _rightPredicate = actualRightContext => ProductionContext.IsContextMatchOther(actualRightContext, rightContext);

        return this;
    }

    public ProductionContextPredicateBuilder SetRightContext(params Module[] rightContext) =>
        SetRightContext((IEnumerable<Module>)rightContext);

    #endregion

    public void Reset()
    {
        _leftPredicate  = _ => true;
        _rightPredicate = _ => true;
    }

    public Predicate<ProductionContext> Build()
    {
        var leftPredicate  = (Predicate<IEnumerable<Module>>)_leftPredicate.Clone();
        var rightPredicate = (Predicate<IEnumerable<Module>>)_rightPredicate.Clone();

        Predicate<ProductionContext> contextPredicate = context =>
            leftPredicate(context.Left) && rightPredicate(context.Right);

        Reset();

        return contextPredicate;
    }
}
