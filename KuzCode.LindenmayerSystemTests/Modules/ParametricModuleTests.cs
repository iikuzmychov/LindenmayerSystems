using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KuzCode.LindenmayerSystem.Tests
{
    [TestClass]
    public class ParametricModuleTests
    {
        #region Equals
        private static object[][] ModulesWithIntParameterPairsAndEquality
        {
            get
            {
                return new object[][]
                {
                    // modules are equals
                    new object[] { null, null, true },
                    new object[] { new ParametricModule<int>("F", 0), new ParametricModule<int>("F", 0), true },
                    new object[] { new ParametricModule<int>("G", 5), new ParametricModule<int>("G", 5), true },

                    // modules are not equals
                    new object[] { null, new ParametricModule<int>("F", 0), false },
                    new object[] { new ParametricModule<int>("F", 0), null, false },
                    new object[] { new ParametricModule<int>("F", 0), new ParametricModule<int>("G", 0), false },
                    new object[] { new ParametricModule<int>("G", 0), new ParametricModule<int>("F", 0), false },
                    new object[] { new ParametricModule<int>("F", 0), new ParametricModule<int>("F", 5), false },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ModulesWithIntParameterPairsAndEquality))]
        public void Equals_ModulesWithIntParameterPairs_ReturnsExpectedResult(
            ParametricModule<int> module1, ParametricModule<int> module2, bool expected)
        {
            var actual = module1 == module2;
            Assert.AreEqual(expected, actual);
        }

        private static object[][] ModulesWithFloatParameterPairsAndEquality
        {
            get
            {
                return new object[][]
                {
                    // modules are equals
                    new object[] { null, null, true },
                    new object[] { new ParametricModule<float>("F", 0), new ParametricModule<float>("F", 0), true },
                    new object[] { new ParametricModule<float>("G", 5), new ParametricModule<float>("G", 5), true },

                    // modules are not equals
                    new object[] { null, new ParametricModule<float>("F", 0), false },
                    new object[] { new ParametricModule<float>("F", 0), null, false },
                    new object[] { new ParametricModule<float>("F", 0), new ParametricModule<float>("G", 0), false },
                    new object[] { new ParametricModule<float>("G", 0), new ParametricModule<float>("F", 0), false },
                    new object[] { new ParametricModule<float>("F", 0), new ParametricModule<float>("F", 5), false },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ModulesWithFloatParameterPairsAndEquality))]
        public void Equals_ModulesPairsWithFloatParameter_ReturnsExpectedResult(
            ParametricModule<float> module1, ParametricModule<float> module2, bool expected)
        {
            var actual = module1 == module2;
            Assert.AreEqual(expected, actual);
        }

        private static object[][] ModulesWithDoubleParameterPairsAndEquality
        {
            get
            {
                return new object[][]
                {
                    // modules are equals
                    new object[] { null, null, true },
                    new object[] { new ParametricModule<double>("F", 0), new ParametricModule<double>("F", 0), true },
                    new object[] { new ParametricModule<double>("G", 5), new ParametricModule<double>("G", 5), true },

                    // modules are not equals
                    new object[] { null, new ParametricModule<double>("F", 0), false },
                    new object[] { new ParametricModule<double>("F", 0), null, false },
                    new object[] { new ParametricModule<double>("F", 0), new ParametricModule<double>("G", 0), false },
                    new object[] { new ParametricModule<double>("G", 0), new ParametricModule<double>("F", 0), false },
                    new object[] { new ParametricModule<double>("F", 0), new ParametricModule<double>("F", 5), false },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ModulesWithDoubleParameterPairsAndEquality))]
        public void Equals_ModulesPairsWithDoubleParameter_ReturnsExpectedResult(
            ParametricModule<double> module1, ParametricModule<double> module2, bool expected)
        {
            var actual = module1 == module2;
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}