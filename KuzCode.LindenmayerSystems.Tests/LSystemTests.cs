using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KuzCode.LindenmayerSystems.Tests;

[TestClass]
public class LSystemTests
{
    #region NextStep

    [TestMethod]
    public void NextStep_CorrectConstructorData_StageIncreases()
    {
        var lSystem = new LSystem(Array.Empty<Module>(), Enumerable.Empty<IProduction<Module>>());
        Assert.AreEqual(lSystem.Step, 0);

        lSystem.NextStep();
        Assert.AreEqual(lSystem.Step, 1);

        lSystem.NextStep();
        Assert.AreEqual(lSystem.Step, 2);
    }

    private static object[][] CorrectConstructorDataWithNewStates
    {
        get
        {
            return new object[][]
            {
                // default modules
                new object[]
                {
                    LSystem.ParseAsModules("F"),
                    Enumerable.Empty<IProduction<Module>>(),
                    LSystem.ParseAsModules("F"),
                },
                new object[]
                {
                    LSystem.ParseAsModules("FG"),
                    Enumerable.Empty<IProduction<Module>>(),
                    LSystem.ParseAsModules("FG"),
                },
                new object[]
                {
                    LSystem.ParseAsModules("F"),
                    new IProduction<Module>[]
                    {
                        // F => G
                        new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('F')
                            .SetSuccessors(new Module('G'))
                            .Build()
                    },
                    LSystem.ParseAsModules("G"),
                },
                new object[]
                {
                    LSystem.ParseAsModules("FF"),
                    new IProduction<Module>[]
                    {
                        // F => G
                        new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('F')
                            .SetSuccessors(new Module('G'))
                            .Build()
                    },
                    LSystem.ParseAsModules("GG"),
                },
                new object[]
                {
                    LSystem.ParseAsModules("F"),
                    new IProduction<Module>[]
                    {
                        // F => F, G
                        new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('F')
                            .SetSuccessors(new Module('F'), new Module('G'))
                            .Build()
                    },
                    LSystem.ParseAsModules("FG"),
                },
                new object[]
                {
                    LSystem.ParseAsModules("FG"),
                    new IProduction<Module>[]
                    {
                        // F => F, G
                        new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('F')
                            .SetSuccessors(new Module('F'), new Module('G'))
                            .Build()
                    },
                    LSystem.ParseAsModules("FGG"),
                },
                new object[]
                {
                    LSystem.ParseAsModules("FG"),
                    new IProduction<Module>[]
                    {
                        // F => F, G
                         new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('F')
                            .SetSuccessors(new Module('F'), new Module('G'))
                            .Build(),

                         // G => F
                         new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('G')
                            .SetSuccessors(new Module('F'))
                            .Build()
                    },
                    LSystem.ParseAsModules("FGF"),
                },
                new object[]
                {
                    LSystem.ParseAsModules("FGF"),
                    new IProduction<Module>[]
                    {
                        // F => F, G
                        new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('F')
                            .SetSuccessors(new Module('F'), new Module('G'))
                            .Build(),

                        // G => F
                        new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('G')
                            .SetSuccessors(new Module('F'))
                            .Build()
                    },
                    LSystem.ParseAsModules("FGFFG"),
                },
                new object[]
                {
                    LSystem.ParseAsModules("FF"),
                    new IProduction<Module>[]
                    {
                        // F < F > * => G
                        new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('F')
                            .ConfigureContextPredicate(builder => builder
                                .SetLeft(new Module('F')))
                            .SetSuccessors(new Module('G'))
                            .Build()
                    },
                    LSystem.ParseAsModules("FG"),
                },
                new object[]
                {
                    LSystem.ParseAsModules("FF"),
                    new IProduction<Module>[]
                    {
                        // * < F > F => G
                        new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('F')
                            .ConfigureContextPredicate(builder => builder
                                .SetRightContext(new Module('F')))
                            .SetSuccessors(new Module('G'))
                            .Build()
                    },
                    LSystem.ParseAsModules("GF"),
                },
                new object[]
                {
                    LSystem.ParseAsModules("FFG"),
                    new IProduction<Module>[]
                    {
                        // F < F > G => G
                        new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('F')
                            .ConfigureContextPredicate(builder => builder
                                .SetLeft(new Module('F'))
                                .SetRightContext(new Module('G')))
                            .SetSuccessors(new Module('G'))
                            .Build()
                    },
                    LSystem.ParseAsModules("FGG"),
                },

                // parametrical modules
                new object[]
                {
                    // F(0)
                    new Module[] { new Module<int>('F', 0) },
                    new IProduction<Module>[]
                    {
                        // F(x) => F(1)
                        new RegularProductionBuilder<Module<int>>()
                            .SetPredecessorSymbol('F')
                            .SetSuccessors(new Module<int>('F', 1))
                            .Build()
                    },
                    // F(1)
                    new Module[] { new Module<int>('F', 1) },
                },
                new object[]
                {
                    // F(0)
                    new Module[] { new Module<int>('F', 0) },
                    new IProduction<Module>[]
                    {
                        // F(x) => F(x+1)
                        new RegularProductionBuilder<Module<int>>()
                            .SetPredecessorSymbol('F')
                            .SetProductionMethod((f, _) =>
                                new[] { new Module<int>(f.Symbol, f.Parameter + 1) })
                            .Build()
                    },
                    // F(1)
                    new Module[] { new Module<int>('F', 1) },
                },
                new object[]
                {
                    // F(1)
                    new Module[] { new Module<int>('F', 1) },
                    new IProduction<Module>[]
                    {
                        // F(x) => F(x+1)
                        new RegularProductionBuilder<Module<int>>()
                            .SetPredecessorSymbol('F')
                            .SetProductionMethod((f, _) =>
                                new[] { new Module<int>(f.Symbol, f.Parameter + 1) })
                            .Build()
                    },
                    // F(2)
                    new Module[] { new Module<int>('F', 2) },
                },
                new object[]
                {
                    // H(0), F
                    new Module[] { new Module<int>('H', 0), new('F') },
                    new IProduction<Module>[]
                    {
                        // H(x) < F > * => G
                        new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('F')
                            .ConfigureContextPredicate(builder => builder
                                .SetLeftPredicate(left => (left.FirstOrDefault() as Module<int>)?.Symbol == 'H'))
                            .SetSuccessors(new Module('G'))
                            .Build()
                    },
                    // H(0), G
                    new Module[] { new Module<int>('H', 0), new('G') },
                },
                new object[]
                {
                    // F(1), D, F(1), G(2)
                    new Module[] { new Module<int>('F', 1), new('D'), new Module<int>('F', 1), new Module<int>('G', 2) },
                    new IProduction<Module>[]
                    {
                        // * < F(y) > G(x) => F(y+x)
                        new RegularProductionBuilder<Module<int>>()
                            .SetPredecessorSymbol('F')
                            .ConfigureContextPredicate(builder => builder
                                .SetRightPredicate(context => (context.FirstOrDefault() as Module<int>)?.Symbol == 'G'))
                            .SetProductionMethod((f, context) =>
                            {
                                var g = (Module<int>)context.Right.First();
                                return new[] { new Module<int>(f.Symbol, f.Parameter + g.Parameter) };
                            })
                            .Build()
                    },
                    // F(1), D, F(3), G(2)
                    new Module[] { new Module<int>('F', 1), new('D'), new Module<int>('F', 3), new Module<int>('G', 2) },
                },
            };
        }
    }

    [TestMethod]
    [DynamicData(nameof(CorrectConstructorDataWithNewStates))]
    public void NextStep_CorrectConstructorData_CorrectNewState(Module[] axioms,
        IEnumerable<IProduction<Module>> producers, Module[] expectedNewState)
    {
        var lSystem        = new LSystem(axioms, producers);
        var actualNewState = lSystem.NextStep();

        Console.WriteLine("Expected: " + string.Join(", ", (IEnumerable<Module>)expectedNewState));
        Console.WriteLine("Actual:   " + string.Join(", ", actualNewState));

        Assert.IsTrue(expectedNewState.SequenceEqual(actualNewState));
    }

    #endregion
}
