``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.15063.1088 (1703/CreatorsUpdate/Redstone2)
Intel Core i5-7300U CPU 2.60GHz (Kaby Lake), 1 CPU, 4 logical and 2 physical cores
Frequency=2648442 Hz, Resolution=377.5805 ns, Timer=TSC
.NET Core SDK=2.1.300
  [Host]     : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.0 (CoreCLR 4.6.26515.07, CoreFX 4.6.26515.06), 64bit RyuJIT


```
|              Method |      Mean |      Error |     StdDev | Scaled | ScaledSD |  Gen 0 | Allocated |
|-------------------- |----------:|-----------:|-----------:|-------:|---------:|-------:|----------:|
|   ThreadLocalRandom |  27.81 ns |  0.3560 ns |  0.3156 ns |   1.00 |     0.00 |      - |       0 B |
|           NewRandom | 528.38 ns | 10.3996 ns | 17.3754 ns |  19.00 |     0.65 | 0.1774 |     280 B |
|      ThreadLocalRng | 147.17 ns |  2.2205 ns |  1.9684 ns |   5.29 |     0.09 |      - |       0 B |
|              NewRng | 148.68 ns |  2.9651 ns |  2.9121 ns |   5.35 |     0.12 | 0.0303 |      48 B |
| ThreadLocalXorShift |  18.03 ns |  0.5063 ns |  0.4489 ns |   0.65 |     0.02 |      - |       0 B |
|         NewXorShift | 100.85 ns |  1.3842 ns |  1.2948 ns |   3.63 |     0.06 | 0.0203 |      32 B |
