using System;
using System.Collections.Generic;
using System.Linq;

namespace DSP4.Model
{
    public static class Signal
    {
        private static readonly double PI2 = Math.PI * 2;

        public static IEnumerable<double> CreateSignals(int n, Func<bool> random, int b1, int b2)
        {
            return Enumerable.Range(0, n)
                .Select(i =>
                {
                    var a = b1 * Math.Sin(PI2 * i / n);
                    var b = Enumerable
                        .Range(50, 20)
                        .Select(j => (random() ? b2 : -b2) * Math.Sin(PI2 * i * j / n))
                        .Sum();
                    return a + b;
                });
        }

        public static IEnumerable<double> CalcSineSpectrums(int harmonicsCount, IEnumerable<double> signals)
        {
            var n = signals.Count();
            var num = 2.0d / n;
            return Enumerable
                .Range(0, harmonicsCount)
                .Select(j => signals
                    .SelectWithIndex((i, v) => v * Math.Sin(PI2 * i * j / n))
                    .Sum())
                .Select(v => v * num);
        }

        public static IEnumerable<double> CalcCosineSpectrums(int harmonicsCount, IEnumerable<double> signals)
        {
            var n = signals.Count();
            var num = 2.0d / n;
            return Enumerable
                .Range(0, harmonicsCount)
                .Select(j => signals
                    .SelectWithIndex((i, v) => v * Math.Cos(PI2 * i * j / n))
                    .Sum())
                .Select(v => v * num);
        }

        public static IEnumerable<double> CalcAmplitudeSpectrums(IEnumerable<double> sineSpectrums, IEnumerable<double> cosineSpectrums)
        {
            return sineSpectrums.Zip(cosineSpectrums, (sin, cos) => Math.Sqrt(sin * sin + cos * cos));
        }

        public static IEnumerable<double> CalcPhaseSpectrums(IEnumerable<double> sineSpectrums, IEnumerable<double> cosineSpectrums)
        {
            return sineSpectrums.Zip(cosineSpectrums, (sin, cos) => Math.Atan(sin / cos));
        }

        public static IEnumerable<double> ParabolicSmoothing(IEnumerable<double> signals, IEnumerable<double> factors, double k)
        {
            var windowSize = factors.Count();
            var window = new Queue<double>(windowSize);
            window.Enqueue(0);
            foreach (var item in signals.Take(windowSize - 1))
            {
                window.Enqueue(item);
            }
            return signals
                .Skip(windowSize - 1)
                .Select(v =>
                {
                    window.Dequeue();
                    window.Enqueue(v);
                    return window
                        .Zip(factors, (a, b) => a * b)
                        .Sum()
                        / k;
                });
        }

        public static IEnumerable<double> SlidingSmoothing(IEnumerable<double> signals, int n)
        {
            var window = new Queue<double>(n);
            window.Enqueue(0);
            foreach (var item in signals.Take(n - 1))
            {
                window.Enqueue(item);
            }
            return signals
                .Skip(n - 1)
                .Select(v =>
                {
                    window.Dequeue();
                    window.Enqueue(v);
                    return window.Average();
                });
        }

        public static IEnumerable<double> MedianSmoothing(IEnumerable<double> signals, int n, int k)
        {
            var window = new Queue<double>(n);
            window.Enqueue(0);
            foreach (var item in signals.Take(n - 1))
            {
                window.Enqueue(item);
            }
            var num = n - 2 * k;
            return signals
                .Skip(n - 1)
                .Select(v =>
                {
                    window.Dequeue();
                    window.Enqueue(v);
                    var sortedWindow = new List<double>(window);
                    sortedWindow.Sort();
                    var range = sortedWindow
                        .Skip(k)
                        .Take(sortedWindow.Count - k);
                    return range.Sum() / num;
                });
        }
    }
}
