using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace ArrayIterationBenchMark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmark>(new BenchmarkConfig());
        }
    }

    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(Job.Default);
            Add(DefaultColumnProviders.Instance);
            Add(MarkdownExporter.GitHub);
            Add(new ConsoleLogger());
            Add(new HtmlExporter());
            Add(MemoryDiagnoser.Default);
        }
    }

    public class Benchmark
    {
        private static readonly int[] s_array = Enumerable.Range(0, 1000).ToArray();

        private static readonly List<int> s_list = Enumerable.Range(0, 1000).ToList();

        [Benchmark(Baseline = true)]
        public bool ForEachArray()
        {
            foreach (var x in s_array)
            {
                if (x == -1) return true;
            }

            return false;
        }

        [Benchmark]
        public bool ForArray()
        {
            for (var i = 0; i < s_array.Length; i++)
            {
                if (s_array[i] == -1) return true;
            }

            return false;
        }

        [Benchmark]
        public bool EnumerableAnyArray() => s_array.Any(x => x == -1);

        [Benchmark]
        public bool EnumerableContainsArray() => s_array.Contains(-1);
        
        [Benchmark]
        public bool ForEachList()
        {
            foreach (var x in s_list)
            {
                if (x == -1) return true;
            }

            return false;
        }

        [Benchmark]
        public bool ForList()
        {
            for (var i = 0; i < s_list.Count; i++)
            {
                if (s_list[i] == -1) return true;
            }

            return false;
        }

        [Benchmark]
        public bool EnumerableAnyList() => s_list.Any(x => x == -1);

        [Benchmark]
        public bool EnumerableContainsList() => s_list.Contains(-1);
    }
}