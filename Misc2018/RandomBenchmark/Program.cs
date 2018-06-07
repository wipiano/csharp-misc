using System;
using System.Security.Cryptography;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace RandomBenchmark
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
        private static readonly ThreadLocal<Random> s_random = new ThreadLocal<Random>(() => new Random(Environment.TickCount));
        private static readonly ThreadLocal<RNGCryptoServiceProvider> s_rng = new ThreadLocal<RNGCryptoServiceProvider>(() => new RNGCryptoServiceProvider());
        private static readonly ThreadLocal<XorShift> s_xor = new ThreadLocal<XorShift>(() => new XorShift(Environment.TickCount, (int)DateTime.Now.Ticks, Environment.TickCount.GetHashCode(), DateTime.Now.Ticks.GetHashCode()));

        [Benchmark(Baseline = true)]
        public int ThreadLocalRandom()
        {
            return s_random.Value.Next();
        }

        [Benchmark]
        public int NewRandom()
        {
            return new Random(Environment.TickCount).Next();
        }

        [Benchmark]
        public byte[] ThreadLocalRng()
        {
            s_rng.Value.GetBytes(s_rngBuffer1.Value);
            return s_rngBuffer1.Value;
        }
        private static readonly ThreadLocal<byte[]> s_rngBuffer1 = new ThreadLocal<byte[]>(() => new byte[4]);

        [Benchmark]
        public byte[] NewRng()
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(s_rngBuffer2.Value);
            }

            return s_rngBuffer2.Value;
        }
        private static readonly ThreadLocal<byte[]> s_rngBuffer2 = new ThreadLocal<byte[]>(() => new byte[4]);

        [Benchmark]
        public int ThreadLocalXorShift()
        {
            return s_xor.Value.Next();
        }

        [Benchmark]
        public int NewXorShift()
        {
            return new XorShift(Environment.TickCount, (int) DateTime.Now.Ticks, Environment.TickCount.GetHashCode(),
                DateTime.Now.Ticks.GetHashCode()).Next();
        }
    }
}
