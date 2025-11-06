using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using Yass.Benchmark;

var config = DefaultConfig.Instance.AddLogger(ConsoleLogger.Default)
                                   .AddExporter(MarkdownExporter.GitHub);

BenchmarkRunner.Run<StandaloneBenchmark>(config);