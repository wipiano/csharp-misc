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
using BenchmarkDotNet.Toolchains.CsProj;

// Qiita: https://qiita.com/yniji/items/6585011633289a257888
// 「C#のLinqでGroupByを使っているのならPythonの方が2倍速くなる」 の検証
namespace LinqCsv
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Test>(new BenchmarkConfig());
        }
    }

    // config は こちらのを参考に: https://github.com/ufcpp/UfcppSample/blob/master/Demo/2018/IOPerformance/App/Program.cs
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(SetRun(Job.Default.UnfreezeCopy()));

            Add(DefaultColumnProviders.Instance);
            Add(MarkdownExporter.GitHub);
            Add(new ConsoleLogger());
            Add(new HtmlExporter());
            Add(MemoryDiagnoser.Default);
        }
        
        private static Job SetRun(Job job)
        {
            // さすがに重いんで1回限り
            job.Run.UnrollFactor = 5;
            job.Run.InvocationCount = 5;
            job.Run.WarmupCount = 1;
            job.Run.TargetCount = 1;
            job.Run.LaunchCount = 1;
            return job;
        }
    }
    
    public class Test
    {
        private readonly TestData[] _testDatas;
        
        // CSV の読み込みはちゃんと最適化しないと遅くてノイズになるので、あらかじめ適当に 100 万件オブジェクトを作っておく。
        // キーが 1000 個、ひとつのキーに対して 1000 個ずつのオブジェクトがあるようなデータ
        // 今回は集計して結果を列挙するまでの「LINQ の部分」だけの高速化をこころみる
        public Test()
        {
            // ベンチマークのたびにつねに同じ数列が得られても問題ない
            var random = new Random(1);
    
            _testDatas = Enumerable.Repeat(Enumerable.Range(1, 1000), 1000)
                .SelectMany(keys => keys.Select(k => new TestData
                {
                    // key 以外は最悪つねに同じ値でも今回の計測には影響しない
                    A = k.ToString(),
                    B = "hogeB",
                    X = random.NextDouble() * 100,
                    Y = random.NextDouble() * 100,
                }))
                .ToArray();
        }
        
        [Benchmark]
        // まず qiita の元記事のコード (4/20)
        public Result[] SlowLinq()
        {
            var testData0 = _testDatas.Select(d => new {d.A, d.B, d.X, d.Y, z = MultiplyToInt(d.X, d.Y)}).ToList();
    
            var testData1 = testData0.GroupBy(d => d.A)
                .Select(g => new Result {Key = g.Key, Sum = g.Sum(d => d.z)}).ToList();
            
            // JSON シリアライズのかわりに ToArray() して返す
            return testData1.ToArray();
        }
    
        [Benchmark]
        // 自分で書いてみたコード (LINQ 標準のもののみ)
        public Result[] NormalLinq()
        {
            return _testDatas.GroupBy(d => d.A, d => MultiplyToInt(d.X, d.Y))
                .Select(g => new Result {Key = g.Key, Sum = g.Sum()})
                .ToArray(); // JSON シリアライズのかわりに ToArray()
        }
    
        [Benchmark]
        // GroupBy の代わりに集計用の拡張メソッドをつくってみる
        // 参考: https://qiita.com/Akira_Kido_N/items/d9519b05ccee6a67158f
        public Result[] UseGroupSum()
        {
            return _testDatas.GroupSum(d => d.A, d => MultiplyToInt(d.X, d.Y))
                .Select(pair => new Result { Key = pair.Key, Sum = pair.Value })
                .ToArray(); // JSON シリアライズのかわりに ToArray()
        }
    
        private static int MultiplyToInt(double x, double y)
            => x > 0 ? (int) (x * y + 0.0000001) : (int) (x * y - 0.0000001);
        
        private sealed class TestData
        {
            public string A { get; set; }
            public string B { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
        }
    
        public sealed class Result
        {
            public string Key { get; set; }
            
            public int Sum { get; set; }
        }
    }

    public static class LinqExtensions
    {
        public static IEnumerable<KeyValuePair<TKey, int>> GroupSum<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, int> valueSelector)
        {
            IEnumerable<KeyValuePair<TKey, int>> Inner()
            {
                var dic = new Dictionary<TKey, int>();

                foreach (var item in source)
                {
                    var key = keySelector(item);
                    
                    if (dic.TryGetValue(key, out var x))
                    {
                        dic[key] = x + valueSelector(item);
                    }
                    else
                    {
                        dic[key] = valueSelector(item);
                    }
                }

                return dic;
            }
            
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }
            
            if (valueSelector == null)
            {
                throw new ArgumentNullException(nameof(valueSelector));
            }

            return Inner();
        }
    }
}

