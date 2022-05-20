using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Perfolizer.Horology;
using System.Globalization;

var config = DefaultConfig
    .Instance
    .WithSummaryStyle(
        new SummaryStyle(CultureInfo.CurrentCulture, true, SizeUnit.KB, TimeUnit.Millisecond, maxParameterColumnWidth: 50));

BenchmarkRunner.Run(typeof(Program).Assembly, config);