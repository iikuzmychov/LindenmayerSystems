using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace KuzCode.LindenmayerSystem.Tests;

[TestClass]
public class LSystemTests
{
    #region NextStep
    [TestMethod]
    public void NextStep_CorrectConstructorData_StepIncreases()
    {
        var lSystem = new LSystem(new() { new Module("F") }, new List<IProducer<Module>>());
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
                        new List<IProducer<Module>>(),
                        "F".ParseAsModules(),
                    },
                    new object[]
                    {
                        "FG".ParseAsModules(),
                        new List<IProducer<Module>>(),
                        "FG".ParseAsModules(),
                    },
                    new object[]
                    {
                        "F".ParseAsModules(),
                        new List<IProducer<Module>>()
                        {
                            new Producer<Module>(
                                new ProductionMethodBuilder<Module>()
                                    .AddCase(new Module("F"), new Module("G"))
                                    .Build())
                        },
                        "G".ParseAsModules()
                    },
                    new object[]
                    {
                        "FF".ParseAsModules(),
                        new List<IProducer<Module>>()
                        {
                            new Producer<Module>(
                                new ProductionMethodBuilder<Module>()
                                    .AddCase(new Module("F"), new Module("G"))
                                    .Build())
                        },
                        "GG".ParseAsModules()
                    },
                    new object[]
                    {
                        "F".ParseAsModules(),
                        new List<IProducer<Module>>()
                        {
                            new Producer<Module>(
                                new ProductionMethodBuilder<Module>()
                                    .AddCase(new Module("F"), "FG".ParseAsModules())
                                    .Build())
                        },
                        "FG".ParseAsModules(),
                    },
                    new object[]
                    {
                        "FG".ParseAsModules(),
                        new List<IProducer<Module>>()
                        {
                            new Producer<Module>(
                                new ProductionMethodBuilder<Module>()
                                    .AddCase(new Module("F"), "FG".ParseAsModules())
                                    .Build())
                        },
                        "FGG".ParseAsModules(),
                    },
                    new object[]
                    {
                        "FG".ParseAsModules(),
                        new List<IProducer<Module>>()
                        {
                            new Producer<Module>(
                                new ProductionMethodBuilder<Module>()
                                    .AddCase(new Module("F"), "FG".ParseAsModules())
                                    .AddCase(new Module("G"), new Module("F"))
                                    .Build())
                        },
                        "FGF".ParseAsModules(),
                    },
                    new object[]
                    {
                        "FGF".ParseAsModules(),
                        new List<IProducer<Module>>()
                        {
                            new Producer<Module>(
                                new ProductionMethodBuilder<Module>()
                                    .AddCase(new Module("F"), "FG".ParseAsModules())
                                    .AddCase(new Module("G"), new Module("F"))
                                    .Build())
                        },
                        "FGFFG".ParseAsModules(),
                    },
                    new object[]
                    {
                        "FF".ParseAsModules(),
                        new List<IProducer<Module>>()
                        {
                            new Producer<Module>(
                                new ProductionMethodBuilder<Module>()
                                    .SetContextPreviousModule(new Module("F"))
                                    .AddCase(new Module("F"), new Module("G"))
                                    .Build())
                        },
                        "FG".ParseAsModules(),
                    },
                    new object[]
                    {
                        "FF".ParseAsModules(),
                        new List<IProducer<Module>>()
                        {
                            new Producer<Module>(
                                new ProductionMethodBuilder<Module>()
                                    .SetContextNextModule(new Module("F"))
                                    .AddCase(new Module("F"), new Module("G"))
                                    .Build())
                        },
                        "GF".ParseAsModules(),
                    },
                    new object[]
                    {
                        "FFG".ParseAsModules(),
                        new List<IProducer<Module>>()
                        {
                            new Producer<Module>(
                                new ProductionMethodBuilder<Module>()
                                    .SetContextPreviousModule(new Module("F"))
                                    .SetContextNextModule(new Module("G"))
                                    .AddCase(new Module("F"), new Module("G"))
                                    .Build())
                        },
                        "FGG".ParseAsModules(),
                    },

                    // parametrical modules
                    new object[]
                    {
                        new List<Module>() { new ParametricModule<int>("F", 0) },
                        new List<IProducer<Module>>()
                        {
                            new Producer<ParametricModule<int>>((a, b) => null
                                //new ProductionMethodBuilder().
                                    )
                                //moduleTemplate: ParametricModule<int>.CreateTemplate("F", null),
                                //successors: new() { new ParametricModule<int>("F", 1) })
                        },
                        new List<Module>() { new ParametricModule<int>("F", 1) },
                    },
                /*new object[]
                {
                    new List<Module>() { new Module('D'), new Module("G"), new ParametricModule<int>("F", 0) },
                    new List<IProducer<Module>>()
                    {
                        new Producer<Module>(
                            moduleTemplate: ParametricModule<int>.CreateTemplate("F", null),
                            successors: new() { new ParametricModule<int>("F", 1) },
                            productionContext: new Producer.Context(
                                previousModules: new() { new Module("G") },
                                nextModules: new()))
                    },
                    new List<Module>() { new Module('D'), new Module("G"), new ParametricModule<int>("F", 1) },
                },
                new object[]
                {
                    new List<Module>() { new ParametricModule<int>("F", 0) },
                    new List<IProducer<Module>>()
                    {
                        new Producer<Module>(
                            moduleTemplate: ParametricModule<int>.CreateTemplate("F", null),
                            produceMethod: (module, context)
                                => new() { new ParametricModule<int>(module.Symbol, (module as ParametricModule<int>).Parameter.Value + 1) })
                    },
                    new List<Module>() { new ParametricModule<int>("F", 1) },
                },
                new object[]
                {
                    new List<Module>() { new ParametricModule<int>("F", 1) },
                    new List<IProducer<Module>>()
                    {
                        new Producer<Module>(
                            moduleTemplate: ParametricModule<int>.CreateTemplate("F", null),
                            produceMethod: (module, context)
                                => new() { new ParametricModule<int>(module.Symbol, (module as ParametricModule<int>).Parameter.Value + 1) })
                    },
                    new List<Module>() { new ParametricModule<int>("F", 2) },
                },
                new object[]
                {
                    new List<Module>() { new ParametricModule<int>("F", 0), new Module("F") },
                    new List<IProducer<Module>>()
                    {
                        new Producer<Module>(
                            moduleTemplate: new Module("F"),
                            successor: new Module("G"),
                            productionContext: new(
                                previousModules: new() { ParametricModule<int>.CreateTemplate("F", null) },
                                nextModules: new()))
                    },
                    new List<Module>() { new ParametricModule<int>("F", 0), new Module("G") },
                },*/
            };
        }
    }

    [TestMethod]
    [DynamicData(nameof(CorrectConstructorDataWithNewStates))]
    public void NextStep_CorrectConstructorData_CorrectNewState(
        List<Module> axioms, List<IProducer<Module>> producers, List<Module> expectedNewState)
    {
        var lSystem = new LSystem(axioms, producers);
        var actualNewState = lSystem.NextStep();

        Console.WriteLine("Expected: " + string.Join(", ", expectedNewState));
        Console.WriteLine("Actual:   " + string.Join(", ", actualNewState));

        CollectionAssert.AreEqual(expectedNewState, (ICollection)actualNewState);
    }
    #endregion
}
