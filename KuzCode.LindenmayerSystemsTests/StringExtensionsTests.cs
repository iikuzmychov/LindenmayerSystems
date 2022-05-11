using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using KuzCode.LindenmayerSystems.Extensions;

namespace KuzCode.LindenmayerSystems.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        /*#region ParseAsModules
        private static object[][] StringsWithModules
        {
            get
            {
                return new object[][]
                {
                    // ignore white spaces
                    new object[]
                    {
                        "",
                        new List<Module> { },
                        true
                    },
                    new object[]
                    {
                        "F",
                        new List<Module> { new("F") },
                        true
                    },
                    new object[]
                    {
                        "FG0IJ",
                        new List<Module> { new("F"), new("G"), new("0"), new("I"), new("J") },
                        true
                    },
                    new object[]
                    {
                        "F [+F]",
                        new List<Module> { new("F"), new("["), new("+"), new("F"), new("]") },
                        true
                    },
                    new object[]
                    {
                        " F+ [F-] F ",
                        new List<Module> { new("F"), new("+"), new("["), new("F"), new("-"), new("]"), new("F") },
                        true
                    },

                    // do not ignore white spaces
                    new object[]
                    {
                        "F [+F]",
                        new List<Module> { new("F"), new(" "), new("["), new("+"), new("F"), new("]") },
                        false
                    },
                    new object[]
                    {
                        " F+ [F-] ",
                        new List<Module> { new(" "), new("F"), new("+"), new(" "), new("["), new("F"), new("-"), new("]"), new(" ") },
                        false
                    },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(StringsWithModules))]
        public void ParseAsModules_StringWithModules_ReturnsExpectedModules(
            string text, List<Module> expected, bool ignoreWhiteSpaces)
        {
            var actual = text.ParseAsModules(ignoreWhiteSpaces);

            CollectionAssert.AreEqual(expected, actual);
        }
        #endregion*/
    }
}