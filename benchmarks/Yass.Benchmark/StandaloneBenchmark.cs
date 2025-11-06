using System;
using System.Buffers;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using Yass;

namespace Yass.Benchmark;

[MemoryDiagnoser]
public class StandaloneBenchmark
{
    private Data _plain;
    private byte[] _bytes;

    [GlobalSetup]
    public void Setup()
    {
        _plain = new Data(1, 12.34m, DateTime.UtcNow);

        _bytes   = Serialize(_plain);
    }

    [Benchmark] 
    public byte[] Serialize() => Serialize(_plain);

    [Benchmark] 
    public object Deserialize() => DeserializeBin(_bytes);

    private static byte[] Serialize(Data data)
    {
        const int size = Data.Size;
        var buffer = ArrayPool<byte>.Shared.Rent(size);
        try
        {
            var span = buffer.AsSpan(0, size);
            Data.Serialize(span, data);
            return span.ToArray();
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private Data DeserializeBin(byte[] data)
    {
        return Data.Deserialize(data);
    }
}

[Contract]
public partial class Data
{
    public Data(int id, decimal total, DateTime createdUtc)
    {
        Id = id;
        Total = total;
        CreatedUtc = createdUtc;
    }

    [Member(1)] 
    public int Id { get; set; }

    [Member(2)] 
    public decimal Total { get; set; }
    
    [Member(3)] 
    public DateTime CreatedUtc { get; set; }
}