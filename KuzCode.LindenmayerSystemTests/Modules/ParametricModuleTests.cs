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
                    new object[] { new ParametricModule<int>('F', 0), new ParametricModule<int>('F', 0), true },
                    new object[] { new ParametricModule<int>('G', 5), new ParametricModule<int>('G', 5), true },

                    // modules are not equals
                    new object[] { null, new ParametricModule<int>('F', 0), false },
                    new object[] { new ParametricModule<int>('F', 0), null, false },
                    new object[] { new ParametricModule<int>('F', 0), new ParametricModule<int>('G', 0), false },
                    new object[] { new ParametricModule<int>('G', 0), new ParametricModule<int>('F', 0), false },
                    new object[] { new ParametricModule<int>('F', 0), new ParametricModule<int>('F', 5), false },
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
                    new object[] { new ParametricModule<float>('F', 0), new ParametricModule<float>('F', 0), true },
                    new object[] { new ParametricModule<float>('G', 5), new ParametricModule<float>('G', 5), true },

                    // modules are not equals
                    new object[] { null, new ParametricModule<float>('F', 0), false },
                    new object[] { new ParametricModule<float>('F', 0), null, false },
                    new object[] { new ParametricModule<float>('F', 0), new ParametricModule<float>('G', 0), false },
                    new object[] { new ParametricModule<float>('G', 0), new ParametricModule<float>('F', 0), false },
                    new object[] { new ParametricModule<float>('F', 0), new ParametricModule<float>('F', 5), false },
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
                    new object[] { new ParametricModule<double>('F', 0), new ParametricModule<double>('F', 0), true },
                    new object[] { new ParametricModule<double>('G', 5), new ParametricModule<double>('G', 5), true },

                    // modules are not equals
                    new object[] { null, new ParametricModule<double>('F', 0), false },
                    new object[] { new ParametricModule<double>('F', 0), null, false },
                    new object[] { new ParametricModule<double>('F', 0), new ParametricModule<double>('G', 0), false },
                    new object[] { new ParametricModule<double>('G', 0), new ParametricModule<double>('F', 0), false },
                    new object[] { new ParametricModule<double>('F', 0), new ParametricModule<double>('F', 5), false },
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

        #region GetTemplate
        private static object[][] NotNullModulesWithIntParameter
        {
            get
            {
                return new object[][]
                {
                    new object[] { new ParametricModule<int>('F', 0), new ParametricModuleTemplate<int>('F') },
                    new object[] { new ParametricModule<int>('F', 5), new ParametricModuleTemplate<int>('F') },
                };
            }
        }        

        [TestMethod]
        [DynamicData(nameof(NotNullModulesWithIntParameter))]
        public void GetTemplate_ModuleWithIntPatameterAndTemplate_ReturnsExpectedTemplate(
            ParametricModule<int> module, ParametricModuleTemplate<int> expected)
        {
            var actual = module.GetTemplate();
            Assert.AreEqual(expected, actual);
        }

        private static object[][] NotNullModulesWithFloatParameter
        {
            get
            {
                return new object[][]
                {
                    new object[] { new ParametricModule<float>('F', 0f), new ParametricModuleTemplate<float>('F') },
                    new object[] { new ParametricModule<float>('F', 5f), new ParametricModuleTemplate<float>('F') },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(NotNullModulesWithFloatParameter))]
        public void GetTemplate_ModuleWithFloatParameterAndTemplate_ReturnsExpectedTemplate(
            ParametricModule<float> module, ParametricModuleTemplate<float> expected)
        {
            var actual = module.GetTemplate();
            Assert.AreEqual(expected, actual);
        }

        private static object[][] NotNullModulesWithDoubleParameter
        {
            get
            {
                return new object[][]
                {
                    new object[] { new ParametricModule<double>('F', 0d), new ParametricModuleTemplate<double>('F') },
                    new object[] { new ParametricModule<double>('F', 5d), new ParametricModuleTemplate<double>('F') },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(NotNullModulesWithDoubleParameter))]
        public void GetTemplate_ModuleWithDoubleParameterAndTemplate_ReturnsExpectedTemplate(
            ParametricModule<double> module, ParametricModuleTemplate<double> expected)
        {
            var actual = module.GetTemplate();
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}