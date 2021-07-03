using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KuzCode.LindenmayerSystem.Tests
{
    [TestClass]
    public class LSystemTests
    {
        #region Constructors
        private static object[][] IncorrectConstructorData
        {
            get
            {
                return new object[][]
                {
                    new object[] { null, null },
                    new object[] { null, new List<Producer>() },
                    new object[] { new List<Module>(), null },
                    new object[]
                    {
                        new List<Module> { null },
                        new List<Producer>()
                    },
                    new object[]
                    {
                        new List<Module>(),
                        new List<Producer>() { null }
                    },
                    new object[]
                    {
                        new List<Module>(),
                        new List<Producer>()
                        {
                            new Producer(new Module('F'), new List<Module>()),
                            new Producer(new Module('F'), new List<Module>()), // same template
                        } 
                    },
                    new object[]
                    {
                        new List<Module>(),
                        new List<Producer>()
                        {
                            new Producer(new ParametricModule<int>('F', null), new List<Module>()),
                            new Producer(new ParametricModule<int>('F', 0), new List<Module>()), // same more stringent template
                        }
                    },
                };
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
        [DynamicData(nameof(IncorrectConstructorData))]
        public void LSystem_IncorrectConstructorData_ThrowsArgumentException(
            List<Module> axioms, List<Producer> producers)
        {
            var lSystem = new LSystem(axioms, producers);
        }
        #endregion

        #region NextStep
        [TestMethod]
        public void NextStep_CorrectConstructorData_StepIncreases()
        {
            var lSystem = new LSystem(new List<Module>(), new List<Producer>());
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
                        new List<Module>(),
                        new List<Producer>(),
                        new List<Module>()
                    },
                    new object[]
                    {
                        new List<Module>() { new Module('F') },
                        new List<Producer>(),
                        new List<Module>() { new Module('F') },
                    },
                    new object[]
                    {
                        new List<Module>() { new Module('F'), new Module('G') },
                        new List<Producer>(),
                        new List<Module>() { new Module('F'), new Module('G') },
                    },
                    new object[]
                    {
                        new List<Module>() { new Module('F') },
                        new List<Producer>() { new Producer(new Module('F'), new Module('G')) },
                        new List<Module>() { new Module('G') },
                    },
                    new object[]
                    {
                        new List<Module>() { new Module('F'), new Module('F') },
                        new List<Producer>() { new Producer(new Module('F'), new Module('G')) },
                        new List<Module>() { new Module('G'), new Module('G') },
                    },
                    new object[]
                    {
                        new List<Module>() { new Module('F') },
                        new List<Producer>()
                        {
                            new Producer(
                                moduleTemplate: new Module('F'),
                                successors: new() { new Module('F'), new Module('G') })
                        },
                        new List<Module>() { new Module('F'), new Module('G') },
                    },
                    new object[]
                    {
                        new List<Module>() { new Module('F'), new Module('G'), },
                        new List<Producer>()
                        {
                            new Producer(
                                moduleTemplate: new Module('F'),
                                successors: new() { new Module('F'), new Module('G') })
                        },
                        new List<Module>() { new Module('F'), new Module('G'), new Module('G') },
                    },
                    new object[]
                    {
                        new List<Module>() { new Module('F'), new Module('G'), },
                        new List<Producer>()
                        {
                            new Producer(
                                moduleTemplate: new Module('F'),
                                successors: new() { new Module('F'), new Module('G') }),

                            new Producer(
                                moduleTemplate: new Module('G'),
                                successors: new() { new Module('F') })
                        },
                        new List<Module>() { new Module('F'), new Module('G'), new Module('F') },
                    },
                    new object[]
                    {
                        new List<Module>() { new Module('F'), new Module('G'), new Module('F') },
                        new List<Producer>()
                        {
                            new Producer(
                                moduleTemplate: new Module('F'),
                                successors: new() { new Module('F'), new Module('G') }),

                            new Producer(
                                moduleTemplate: new Module('G'),
                                successors: new() { new Module('F') })
                        },
                        new List<Module>() { new Module('F'), new Module('G'), new Module('F'), new Module('F'), new Module('G'), },
                    },
                    new object[]
                    {
                        new List<Module>() { new Module('F'), new Module('F'), },
                        new List<Producer>()
                        {
                            new Producer(
                                moduleTemplate: new Module('F'),
                                successor: new Module('G'),
                                productionContext: new Producer.Context(
                                    previousModules: new() { new Module('F') },
                                    nextModules: new()))
                        },
                        new List<Module>() { new Module('F'), new Module('G') },
                    },
                    new object[]
                    {
                        new List<Module>() { new Module('F'), new Module('F'), },
                        new List<Producer>()
                        {
                            new Producer(
                                moduleTemplate: new Module('F'),
                                successor: new Module('G'),
                                productionContext: new Producer.Context(
                                    previousModules: new(),
                                    nextModules: new() { new Module('F') }))
                        },
                        new List<Module>() { new Module('G'), new Module('F') },
                    },
                    new object[]
                    {
                        new List<Module>() { new Module('F'), new Module('F'), new Module('G') },
                        new List<Producer>()
                        {
                            new Producer(
                                moduleTemplate: new Module('F'),
                                successor: new Module('G'),
                                productionContext: new Producer.Context(
                                    previousModules: new() { new Module('F') },
                                    nextModules: new() { new Module('G') }))
                        },
                        new List<Module>() { new Module('F'), new Module('G'), new Module('G') },
                    },

                    // parametrical modules
                    new object[]
                    {
                        new List<Module>() { new ParametricModule<int>('F', 0) },
                        new List<Producer>()
                        {
                            new Producer(
                                moduleTemplate: new ParametricModule<int>('F', null),
                                successors: new() { new ParametricModule<int>('F', 1) })
                        },
                        new List<Module>() { new ParametricModule<int>('F', 1) },
                    },
                    new object[]
                    {
                        new List<Module>() { new Module('A'), new Module('B'), new ParametricModule<int>('F', 0) },
                        new List<Producer>()
                        {
                            new Producer(
                                moduleTemplate: new ParametricModule<int>('F', null),
                                successors: new() { new ParametricModule<int>('F', 1) },
                                productionContext: new Producer.Context(
                                    previousModules: new() { new Module('B') },
                                    nextModules: new()))
                        },
                        new List<Module>() { new Module('A'), new Module('B'), new ParametricModule<int>('F', 1) },
                    },
                    new object[]
                    {
                        new List<Module>() { new ParametricModule<int>('F', 0) },
                        new List<Producer>()
                        {
                            new Producer(
                                moduleTemplate: new ParametricModule<int>('F', null),
                                produceMethod: module
                                    => new() { new ParametricModule<int>(module.Symbol, (module as ParametricModule<int>).Parameter + 1) })
                        },
                        new List<Module>() { new ParametricModule<int>('F', 1) },
                    },
                    new object[]
                    {
                        new List<Module>() { new ParametricModule<int>('F', 1) },
                        new List<Producer>()
                        {
                            new Producer(
                                moduleTemplate: new ParametricModule<int>('F', null),
                                produceMethod: module
                                    => new() { new ParametricModule<int>(module.Symbol, (module as ParametricModule<int>).Parameter + 1) })
                        },
                        new List<Module>() { new ParametricModule<int>('F', 2) },
                    },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(CorrectConstructorDataWithNewStates))]
        public void NextStep_CorrectConstructorData_CorrectNewState(
            List<Module> axioms, List<Producer> producers, List<Module> expectedNewState)
        {
            var lSystem        = new LSystem(axioms, producers);
            var actualNewState = lSystem.NextStep();

            Console.WriteLine("Expected: " + string.Join(", ", expectedNewState));
            Console.WriteLine("Actual:   " + string.Join(", ", actualNewState));

            CollectionAssert.AreEqual(expectedNewState, (ICollection)actualNewState);
        }
        #endregion
    }
}