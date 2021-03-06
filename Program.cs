﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;


namespace EnumHasFlagMethodBenchmarking
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<EnumsBenchmark>();

        }
    }

    [Flags]
    public enum Beverage
    {
        Water = 1,
        Beer = 2,
        Tea = 4,
        Wine = 8
    }

    [SimpleJob(RuntimeMoniker.Net461)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    public class EnumsBenchmark
    {
        private List<Beverage> tests;

        [Params(50, 100, 200)]
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            tests = new List<Beverage>();
            int[] values = new int[] { 1, 2, 4, 8 };


            for (int i = 0; i < Size; i++)
            {
                Beverage firstElm = (Beverage)values.GetRandomFromArray<int>();
                Beverage secondElm = (Beverage)values.GetRandomFromArray<int>();
                tests.Add(firstElm | secondElm);
            }
        }


        [Benchmark]
        public void RunWithHasFlag()
        {
            tests.Select(x => x.HasFlag(Beverage.Tea)).ToList();
        }


        [Benchmark]
        public void RunWithBitOperator()
        {
            tests.Select(x => (x & Beverage.Tea) == Beverage.Tea).ToList();
        }

    }

    public static class Extensions
    {
        public static T GetRandomFromArray<T>(this IEnumerable<T> values)
        {
            var length = values.Count();
            Random rd = new Random();
            return values.ElementAt(rd.Next(length));
        }
    }

}
