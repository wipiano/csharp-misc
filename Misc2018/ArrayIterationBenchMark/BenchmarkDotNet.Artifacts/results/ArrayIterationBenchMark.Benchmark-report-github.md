``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.15063.1324 (1703/CreatorsUpdate/Redstone2)
Intel Core i5-7300U CPU 2.60GHz (Max: 2.70GHz) (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
Frequency=2648438 Hz, Resolution=377.5810 ns, Timer=TSC
.NET Core SDK=2.1.402
  [Host]     : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.4 (CoreCLR 4.6.26814.03, CoreFX 4.6.26814.02), 64bit RyuJIT


```
|                  Method |       Mean |     Error |    StdDev | Scaled | ScaledSD |  Gen 0 | Allocated |
|------------------------ |-----------:|----------:|----------:|-------:|---------:|-------:|----------:|
|            ForEachArray |   605.3 ns | 12.055 ns | 11.276 ns |   1.00 |     0.00 |      - |       0 B |
|                ForArray |   618.9 ns | 10.288 ns |  9.120 ns |   1.02 |     0.02 |      - |       0 B |
|      EnumerableAnyArray | 6,487.3 ns | 21.818 ns | 19.341 ns |  10.72 |     0.19 | 0.0153 |      32 B |
| EnumerableContainsArray |   647.3 ns | 12.866 ns | 23.849 ns |   1.07 |     0.04 |      - |       0 B |
|             ForEachList | 2,113.2 ns | 21.788 ns | 20.380 ns |   3.49 |     0.07 |      - |       0 B |
|                 ForList | 1,671.0 ns | 29.085 ns | 25.783 ns |   2.76 |     0.06 |      - |       0 B |
|       EnumerableAnyList | 8,524.6 ns | 77.879 ns | 69.038 ns |  14.09 |     0.27 | 0.0153 |      40 B |
|  EnumerableContainsList |   615.0 ns |  7.903 ns |  7.393 ns |   1.02 |     0.02 |      - |       0 B |
