﻿using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Nethermind.Evm.Precompiles;

namespace Nethermind.Precompiles.Benchmark
{
    [HtmlExporter]
    // [NativeMemoryProfiler]
    // [MemoryDiagnoser]
    // [ShortRunJob(RuntimeMoniker.NetCoreApp31)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    public class Blake2fBenchmark : PrecompileBenchmarkBase
    {
        protected override IEnumerable<IPrecompile> Precompiles => new[] {Blake2FPrecompile.Instance};
        protected override string InputsDirectory => "blake2f";
    }
}