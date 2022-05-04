using KuzCode.LindenmayerSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KuzCode.LindenmayerSystem.Tests
{
    [TestClass]
    public class ModuleTests
    {
        private static object[][] ModulesPairsAndEquality
        {
            get
            {
                return new object[][]
                {
                    // modules are equals
                    new object[] { null, null, true },
                    new object[] { new Module("F"), new Module("F"), true },
                    new object[] { new Module("G"), new Module("G"), true },

                    // modules are not equals
                    new object[] { null, new Module("F"), false },
                    new object[] { new Module("F"), null, false },
                    new object[] { new Module("F"), new Module("G"), false },
                    new object[] { new Module("G"), new Module("F"), false },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ModulesPairsAndEquality))]
        public void Equals_ModulesPairs_ReturnsExpectedResult(Module module1, Module module2, bool expected)
        {
            var actual = module1 == module2;
            Assert.AreEqual(expected, actual);
        }
    }
}