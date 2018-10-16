using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace SpanBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<CsvLineReaderBenchmark>(new BenchmarkConfig());
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

    public class CsvLineReaderBenchmark
    {
        private static readonly string s_csvLine = $"1,{Guid.NewGuid()},1.25,true,hogefuga";

        [Benchmark]
        public Record StringSplit()
        {
            string[] split = s_csvLine.Split(',', 5);

            return new Record()
            {
                UserId = int.Parse(split[0]),
                SessionId = Guid.Parse(split[1]),
                Rate = double.Parse(split[2]),
                Enabled = bool.Parse(split[3]),
                Comment = split[4]
            };
        }

        [Benchmark]
        public Record Span()
        {
            var reader = new SpanCsvReader(s_csvLine);
            var record = new Record()
            {
                UserId = int.Parse(reader.ReadNext()),
                SessionId = Guid.Parse(reader.ReadNext()),
                Rate = double.Parse(reader.ReadNext()),
                Enabled = bool.Parse(reader.ReadNext()),
                Comment = new string(reader.ReadNext())
            };

            return record;
        }

        private ref struct SpanCsvReader
        {
            private ReadOnlySpan<char> _chars;

            public SpanCsvReader(string s)
            {
                _chars = s.AsSpan();
            }

            public ReadOnlySpan<char> ReadNext()
            {
                var commaIndex = _chars.IndexOf(',');
                if (commaIndex > 0)
                {
                    var ret = _chars.Slice(0, commaIndex);
                    _chars = _chars.Slice(commaIndex + 1);
                    return ret;
                }
                else
                {
                    var ret = _chars;
                    _chars = ReadOnlySpan<char>.Empty;
                    return ret;
                }
            }
        }
    }

    public class Record
    {
        public int UserId { get; set; }
        
        public Guid SessionId { get; set; }
        
        public double Rate { get; set; }
        
        public bool Enabled { get; set; }
        
        public string Comment { get; set; }
    }
}