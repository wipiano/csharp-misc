``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.15063.1324 (1703/CreatorsUpdate/Redstone2)
Intel Core i5-7300U CPU 2.60GHz (Max: 2.70GHz) (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
Frequency=2648438 Hz, Resolution=377.5810 ns, Timer=TSC
.NET Core SDK=2.1.402
  [Host]     : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT


```
|      Method |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------ |---------:|----------:|----------:|-------:|----------:|
| StringSplit | 677.7 ns | 14.031 ns | 19.206 ns | 0.2432 |     384 B |
|        Span | 538.0 ns |  7.979 ns |  7.463 ns | 0.0658 |     104 B |
