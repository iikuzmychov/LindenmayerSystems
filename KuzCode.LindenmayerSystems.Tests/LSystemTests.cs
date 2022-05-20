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
                         new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('F')
                            .SetSuccessors(new Module('F'), new Module('G'))
                            .Build(),

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
                        new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('F')
                            .SetSuccessors(new Module('F'), new Module('G'))
                            .Build(),

                        new RegularProductionBuilder<Module>()
                            .SetPredecessorSymbol('G')
                            .SetSuccessors(new Module('F'))
                            .Build()
                    },
                    LSystem.ParseAsModules("FGFFG"),
                },
                /*new object[]
                {
                    "FF".ParseAsModules(),
                    new IProduction<Module>[]
                    {
                        new ProductionBuilder()
                            .SetContextPreviousModule(new("F"))
                            .ConfigureMethod(builder => builder
                                .SetModule(new("F"))
                                .SetProducedModules(new Module("G")))
                            .Build()
                    },
                    "FG".ParseAsModules(),
                },
                new object[]
                {
                    "FF".ParseAsModules(),
                    new IProduction<Module>[]
                    {
                        new ProductionBuilder()
                            .SetContextNextModule(new("F"))
                            .ConfigureMethod(builder => builder
                                .SetModule(new("F"))
                                .SetProducedModules(new Module("G")))
                            .Build()
                    },
                    "GF".ParseAsModules(),
                },
                new object[]
                {
                    "FFG".ParseAsModules(),
                    new IProduction<Module>[]
                    {
                        new ProductionBuilder()
                            .SetContextPreviousModule(new("F"))
                            .SetContextNextModule(new("G"))
                            .ConfigureMethod(builder => builder
                                .SetModule(new("F"))
                                .SetProducedModules(new Module("G")))
                            .Build()
                    },
                    "FGG".ParseAsModules(),
                },*/

                // parametrical modules
                new object[]
                {
                    new Module[] { new Module<int>('F', 0) },
                    new IProduction<Module>[]
                    {
                        new RegularProductionBuilder<Module<int>>()
                            .SetPredecessorSymbol('F')
                            .SetSuccessors(new Module<int>('F', 1))
                            .Build()
                    },
                    new Module[] { new Module<int>('F', 1) },
                },
                /*new object[]
                {
                    new Module[] { new("D"), new Module<int>("G", 0), new Module<int>("F", 0) },
                    new IProduction<Module>[]
                    {
                        new ProductionBuilder()
                            .SetContextPreviousModule(new("G"))
                            .ConfigureMethod(builder => builder
                                .SetModuleCondition(module => module.Name == "F")
                                .SetProducedModules(new Module<int>("F", 1)))
                            .Build()
                    },
                    new Module[] { new("D"), new Module<int>("G", 0), new Module<int>("F", 1) },
                },*/
                new object[]
                {
                    new Module[] { new Module<int>('F', 0) },
                    new IProduction<Module>[]
                    {
                        new RegularProductionBuilder<Module<int>>()
                            .SetPredecessorSymbol('F')
                            .SetProductionMethod((module, _) =>
                                new List<Module> { new Module<int>(module.Symbol, module.Parameter + 1) })
                            .Build()
                    },
                    new Module[] { new Module<int>('F', 1) },
                },
                new object[]
                {
                    new Module[] { new Module<int>('F', 1) },
                    new IProduction<Module>[]
                    {
                        new RegularProductionBuilder<Module<int>>()
                            .SetPredecessorSymbol('F')
                            .SetProductionMethod((module, _) =>
                                new List<Module> { new Module<int>(module.Symbol, module.Parameter + 1) })
                            .Build()
                    },
                    new Module[] { new Module<int>('F', 2) },
                },
                /*new object[]
                {
                    new Module[] { new Module<int>("F", 0), new("F") },
                    new List<Producer>()
                    {
                        new ProducerBuilder()
                            .SetContextPreviousModule<Module<int>>(module => module.Name == "F")
                            .SetMethod<Module>(builder => builder
                                .SetModule(new("F"))
                                .SetProducedModule(new("G")))
                            .Build()
                    },
                    new Module[] { new Module<int>("F", 0), new("G") },
                },*/
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
