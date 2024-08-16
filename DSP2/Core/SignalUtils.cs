using System;
using System.Threading.Tasks;

namespace Core
{
    public static class SignalUtils
    {
        public static double ToRadians(double angle)
        {
            return angle / 180 * Math.PI;
        }

        public static double HarmonicSignalFunction(int n, int N, double phiRadians)
        {
            return Math.Sin(2 * Math.PI * n / N + phiRadians);
        }

        public static (double, double) GetCalculationError(int M, int N, double phiRadians)
        {
            double rms1 = 0;
            double rms2 = 0;

            Parallel.For(0, M, n =>
            {
                var v = HarmonicSignalFunction(n, N, phiRadians);
                InterlockedExtensions.Add(ref rms1, v * v);
                InterlockedExtensions.Add(ref rms2, v);
            });

            var t1 = rms1 / (M + 1);
            var v1 = 0.707 - Math.Sqrt(t1);

            var t2 = rms2 / (M + 1);
            var v2 = 0.707 - Math.Sqrt(t1 - t2 * t2);
            return (v1, v2);
        }

        public static void Run(int K, int N, double phiRadians, Action<int, double> fun1, Action<int, double> fun2)
        {
            Parallel.For(K, 2 * N, M =>
            {
                (double v1, double v2) = GetCalculationError(M, N, phiRadians);
                fun1.Invoke(M, v1);
                fun2.Invoke(M, v2);
            });
        }
    }
}
