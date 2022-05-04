using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace KuzCode.LindenmayerSystem
{
    public static class StringExtensions
    {
        /// <summary>
        /// Parse each symbol from <paramref name="source"/> as new <see cref="Module"/>
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="ignoreWhiteSpaces">Should the method ignore white spaces</param>
        /// <returns></returns>
        public static List<Module> ParseAsModules(this string source, bool ignoreWhiteSpaces = true)
        {
            if (ignoreWhiteSpaces)
                source = source.Replace(" ", "");

            return source.Select(symbol => new Module(symbol.ToString())).ToList();
        }

        /// <summary>
        /// Parse source string as production context
        /// </summary>
        /// <param name="source">
        /// Source string.<br/>
        /// Must contains '&lt;' and '&gt;' symbols and match next template: "<c>...AB &lt; X &gt; CD...</c>".<br/>
        /// 'X' can be replaced by any other symbol besides '&lt;' and '&gt;'
        /// </param>
        /// <param name="ignoreWhiteSpaces">Should the method ignore white spaces</param>
        /// <returns></returns>
        public static ProductionContext ParseAsProductionContext(this string source, bool ignoreWhiteSpaces = true)
        {
            if (source.Count(symbol => symbol == '<') != 1 ||
                source.Count(symbol => symbol == '>') != 1 ||
                source.IndexOf('<') > source.IndexOf('>'))
            {
                throw new ArgumentException("Incorrect source string");
            }

            if (ignoreWhiteSpaces)
                source = source.Replace(" ", "");

            var splited         = source.Split(new char[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
            var previousModules = splited.First().ParseAsModules(ignoreWhiteSpaces);
            var nextModules     = splited.Last().ParseAsModules(ignoreWhiteSpaces);
            var context         = new ProductionContext(previousModules, nextModules);

            return context;
        }

        /// <summary>
        /// Parse source string as producer
        /// </summary>
        /// <param name="source">
        /// Source string.<br/>
        /// Must contains '=>' symbol and match one of next templates:<br/>
        /// "<c>X => C D E</c>" or <br/> 
        /// "<c>..., A &lt; X &gt; B, ... => C D E</c>".<br/>
        /// 'X' can be replaced by any other symbol besides '&lt;', '&gt;' and '='
        /// </param>
        /// <param name="ignoreWhiteSpaces">Should the method ignore white spaces</param>
        /// <returns></returns>
        /*public static Producer ParseAsProducer(this string source, bool ignoreWhiteSpaces = true)
        {
            if (Regex.Matches(source, "=>").Count != 1)
                throw new ArgumentException("Incorrect source string");

            if (ignoreWhiteSpaces)
                source = source.Replace(" ", "");

            Module module;
            Producer.Context productionContext;

            var splited    = source.Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
            var successors = splited.Last().ParseAsModules(ignoreWhiteSpaces);

            if (splited.First().Contains('<') && splited.First().Contains('>'))
            {
                productionContext = splited.First().ParseAsProductionContext(ignoreWhiteSpaces);
                
                var moduleSymbol = splited
                    .First()
                    .Split(new char[] { '<', '>' }, StringSplitOptions.RemoveEmptyEntries)
                    [1]
                    .Single();

                module = new Module(moduleSymbol);
            }
            else
            {
                productionContext = Producer.Context.NoContext;
                module            = new Module(splited.First().Single());
            }

            var producer = new Producer(module, successors, productionContext);

            return producer;
        }*/
    }
}
