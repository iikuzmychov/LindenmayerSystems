using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using KuzCode.LindenmayerSystems.Extensions;
using System.Linq;

namespace KuzCode.LindenmayerSystems.Tests;

[TestClass]
public class LSystemTests
{
    #region NextStep

    /*[TestMethod]
    public void NextStep_CorrectConstructorData_StepIncreases()
    {
        var lSystem = new LSystem(new Module[] { new("F") }, Array.Empty<IProduction<Module>>());
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
                    "F".ParseAsModules(),
                    Array.Empty<IProduction<Module>>(),
                    "F".ParseAsModules(),
                },
                new object[]
                {
                    "FG".ParseAsModules(),
                    Array.Empty<IProduction<Module>>(),
                    "FG".ParseAsModules(),
                },
                new object[]
                {
                    "F".ParseAsModules(),
                    new IProduction<Module>[]
                    {
                        new RegularProductionBuilder<Module>()
                            .SetPredecessor(new Module("F"))
                            .SetSuccessors(new Module("G"))
                            .Build()
                    },
                    "G".ParseAsModules()
                },
                new object[]
                {
                    "FF".ParseAsModules(),
                    new IProduction<Module>[]
                    {
                        new RegularProductionBuilder<Module>()
                            .SetPredecessor(new Module("F"))
                            .SetSuccessors(new Module("G"))
                            .Build()
                    },
                    "GG".ParseAsModules()
                },
                new object[]
                {
                    "F".ParseAsModules(),
                    new IProduction<Module>[]
                    {
                        new RegularProductionBuilder<Module>()
                            .SetPredecessor(new Module("F"))
                            .SetSuccessors(new Module("F"), new Module("G"))
                            .Build()
                    },
                    "FG".ParseAsModules(),
                },
                new object[]
                {
                    "FG".ParseAsModules(),
                    new IProduction<Module>[]
                    {
                        new RegularProductionBuilder<Module>()
                            .SetPredecessor(new Module("F"))
                            .SetSuccessors(new Module("F"), new Module("G"))
                            .Build()
                    },
                    "FGG".ParseAsModules(),
                },
                new object[]
                {
                    "FG".ParseAsModules(),
                    new IProduction<Module>[]
                    {
                        new RegularProductionBuilder<Module>()
                            .SetPredecessor(new Module("F"))
                            .SetSuccessors(new Module("F"), new Module("G"))
                            .Build(),

                        new RegularProductionBuilder<Module>()
                            .SetPredecessor(new Module("G"))
                            .SetSuccessors(new Module("F"))
                            .Build(),
                    },
                    "FGF".ParseAsModules(),
                },
                new object[]
                {
                    "FGF".ParseAsModules(),
                    new IProduction<Module>[]
                    {
                        new RegularProductionBuilder<Module>()
                            .SetPredecessor(new Module("F"))
                            .SetSuccessors(new Module("F"), new Module("G"))
                            .Build(),

                        new RegularProductionBuilder<Module>()
                            .SetPredecessor(new Module("G"))
                            .SetSuccessors(new Module("F"))
                            .Build(),
                    },
                    "FGFFG".ParseAsModules(),
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
                },

                // parametrical modules
                new object[]
                {
                    new List<Module>() { new ParametricModule<int>("F", 0) },
                    new IProduction<Module>[]
                    {
                        new RegularProductionBuilder<ParametricModule<int>>()
                            .SetSuccessors(new ParametricModule<int>("F", 1))
                            .Build()
                    },
                    new List<Module>() { new ParametricModule<int>("F", 1) },
                },
                /*new object[]
                {
                    new List<Module>() { new("D"), new ParametricModule<int>("G", 0), new ParametricModule<int>("F", 0) },
                    new IProduction<Module>[]
                    {
                        new ProductionBuilder()
                            .SetContextPreviousModule(new("G"))
                            .ConfigureMethod(builder => builder
                                .SetModuleCondition(module => module.Name == "F")
                                .SetProducedModules(new ParametricModule<int>("F", 1)))
                            .Build()
                    },
                    new List<Module>() { new("D"), new ParametricModule<int>("G", 0), new ParametricModule<int>("F", 1) },
                },
                new object[]
                {
                    new List<Module>() { new ParametricModule<int>("F", 0) },
                    new IProduction<Module>[]
                    {
                        new ProductionBuilder()
                            .ConfigureMethod(builder => builder
                                .SetModuleCondition(module => module.Name == "F")
                                .SetBaseMethod<ParametricModule<int>>((module, context) =>
                                    new List<Module> { new ParametricModule<int>(module.Name, module.Parameter + 1) }))
                            .Build()
                    },
                    new List<Module>() { new ParametricModule<int>("F", 1) },
                },
                new object[]
                {
                    new List<Module>() { new ParametricModule<int>("F", 1) },
                    new IProduction<Module>[]
                    {
                        new ProductionBuilder()
                            .ConfigureMethod(builder => builder
                                .SetModuleCondition(module => module.Name == "F")
                                .SetBaseMethod<ParametricModule<int>>((module, context) =>
                                    new List<Module> { new ParametricModule<int>(module.Name, module.Parameter + 1) }))
                            .Build()
                    },
                    new List<Module>() { new ParametricModule<int>("F", 2) },
                },
                /*new object[]
                {
                    new List<Module>() { new ParametricModule<int>("F", 0), new("F") },
                    new List<Producer>()
                    {
                        new ProducerBuilder()
                            .SetContextPreviousModule<ParametricModule<int>>(module => module.Name == "F")
                            .SetMethod<Module>(builder => builder
                                .SetModule(new("F"))
                                .SetProducedModule(new("G")))
                            .Build()
                    },
                    new List<Module>() { new ParametricModule<int>("F", 0), new("G") },
                },
            };
        }
    }

    [TestMethod]
    [DynamicData(nameof(CorrectConstructorDataWithNewStates))]
    public void NextStep_CorrectConstructorData_CorrectNewState(
        List<Module> axioms, IProduction<Module>[] producers, List<Module> expectedNewState)
    {
        var lSystem        = new LSystem(axioms.ToArray(), producers);
        var actualNewState = lSystem.NextStep();

        Console.WriteLine("Expected: " + string.Join(", ", expectedNewState));
        Console.WriteLine("Actual:   " + string.Join(", ", actualNewState));

        CollectionAssert.AreEqual(expectedNewState, (ICollection)actualNewState);
    }*/

    private static readonly LSystem s_perfomanceTestLSystem = new LSystem(
        axiom:       new Module[] { new('F') },
        productions: new IProduction<Module>[]
        {
            new RegularProductionBuilder<Module>()
                .SetPredecessor(new('F'))
                .SetSuccessors(new('F'), new('+'), new('G'))
                .Build(),

            new RegularProductionBuilder<Module>()
                .SetPredecessor(new('G'))
                .SetSuccessors(new('F'), new('-'), new('G'))
                .Build(),
        });

    [TestMethod]
    public void NextSteps_PerfomanceTest()
    {
        s_perfomanceTestLSystem.NextSteps(15);
    }
    #endregion
}
