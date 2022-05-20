using BenchmarkDotNet.Attributes;

namespace KuzCode.LindenmayerSystems.Benchmarks;

public class LSystemBenchmakrs
{
    [ParamsSource(nameof(GetLSystems))]
    public NamedLSystem CurrentLSystem { get; set; }

    [Params(6, 7, 8)]
    public int StepsCount { get; set; }

    public static IEnumerable<NamedLSystem> GetLSystems() =>
        new NamedLSystem[]
        {
            new NamedLSystem(
                name:        "Dragon curve, O(2n)",
                axiom:       LSystem.ParseAsModules("F"),
                productions: new IProduction<Module>[]
                {
                    new RegularProductionBuilder<Module>()
                        .SetPredecessorSymbol('F')
                        .SetSuccessors(new('F'), new('+'), new('G'))
                        .Build(),

                    new RegularProductionBuilder<Module>()
                        .SetPredecessorSymbol('G')
                        .SetSuccessors(new('F'), new('-'), new('G'))
                        .Build(),
                }),

            new NamedLSystem(
                name:        "Koch curve, O(5n)",
                axiom:       LSystem.ParseAsModules("-F"),
                productions: new IProduction<Module>[]
                {
                    new RegularProductionBuilder<Module>()
                        .SetPredecessorSymbol('F')
                        .SetSuccessors(LSystem.ParseAsModules("F+F-F-F+F"))
                        .Build(),
                }),

            new NamedLSystem(
                name:        "Sierpinski`s carpet, O(8n)",
                axiom:       LSystem.ParseAsModules("F"),
                productions: new IProduction<Module>[]
                {
                    new RegularProductionBuilder<Module>()
                        .SetPredecessorSymbol('F')
                        .SetSuccessors(LSystem.ParseAsModules("F+F-F-F-G+F+F+F-F"))
                        .Build(),

                    new RegularProductionBuilder<Module>()
                        .SetPredecessorSymbol('G')
                        .SetSuccessors(LSystem.ParseAsModules("GGG"))
                        .Build(),
                })
        };

    [IterationCleanup]
    public void ResetLSystem() => CurrentLSystem.Reset();

    [Benchmark]
    public void NextSteps() => CurrentLSystem.NextSteps(StepsCount);
}