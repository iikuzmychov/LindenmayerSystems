using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystem;

/*// TODO: finalize class
public class ProbabilisticProducer : Producer 
{
    private Dictionary<ProductionMethod, int> _produceMethodsWithWeights;
    private Random _random;

    #region Constructors
    protected ProbabilisticProducer(Module templateModule,
        Dictionary<ProductionMethod, int> produceMethodsWithWeights,
        Random random, ProductionContext productionContext, int noProduceMethodWeight) : base(templateModule, productionContext)
    {

        if (produceMethodsWithWeights.Count == 0)
            throw new ArgumentException("Produce methods count equals zero");

        if (produceMethodsWithWeights.Values.Any(value => value < 0))
            throw new ArgumentException("Weight can not be less than zero");

        _produceMethodsWithWeights = produceMethodsWithWeights
            ?? throw new ArgumentNullException(nameof(produceMethodsWithWeights)); ;

        _random = random ?? throw new ArgumentNullException(nameof(random));
    }
    #endregion

    protected override ProductionMethod GetProduceMethod()
        => _produceMethodsWithWeights.GetRandomKeyByWeight(_random.Next());
}*/
