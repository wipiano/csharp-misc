``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 8.1 (6.3.9600.0)
Intel Xeon CPU E5-2420 v2 2.20GHz, 2 CPU, 24 logical and 12 physical cores
Frequency=2148441 Hz, Resolution=465.4538 ns, Timer=TSC
  [Host]     : .NET Framework 4.6 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2117.0
  DefaultJob : .NET Framework 4.6 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2117.0


```
|              Method |        Mean |      Error |     StdDev |      Median | Scaled | ScaledSD |  Gen 0 | Allocated |
|-------------------- |------------:|-----------:|-----------:|------------:|-------:|---------:|-------:|----------:|
|   ThreadLocalRandom |    83.95 ns |  0.2809 ns |  0.2346 ns |    83.98 ns |   1.00 |     0.00 |      - |       0 B |
|           NewRandom | 2,537.63 ns | 31.4374 ns | 26.2516 ns | 2,549.96 ns |  30.23 |     0.31 | 0.0420 |     280 B |
|      ThreadLocalRng |   994.76 ns | 19.8864 ns | 30.3686 ns | 1,010.22 ns |  11.85 |     0.36 |      - |       0 B |
|              NewRng |   995.92 ns | 20.0157 ns | 43.9350 ns | 1,020.86 ns |  11.86 |     0.52 | 0.0038 |      32 B |
| ThreadLocalXorShift |    51.66 ns |  1.0909 ns |  1.3397 ns |    52.43 ns |   0.62 |     0.02 |      - |       0 B |
|         NewXorShift |   218.59 ns |  1.5784 ns |  1.4765 ns |   218.62 ns |   2.60 |     0.02 | 0.0050 |      32 B |
