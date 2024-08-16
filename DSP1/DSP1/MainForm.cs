using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DSP1
{
    public partial class MainForm : Form
    {
        private const int N = 1024;
        private bool CanRebuild { get; set; } = true;
        private readonly (NumericUpDown, NumericUpDown, NumericUpDown)[] harmonics;
        private readonly Timer timer;

        public MainForm()
        {
            InitializeComponent();
            harmonics = new[] {
                (hInputA1, hInputF1, hInputP1),
                (hInputA2, hInputF2, hInputP2),
                (hInputA3, hInputF3, hInputP3),
                (hInputA4, hInputF4, hInputP4),
                (hInputA5, hInputF5, hInputP5)
            };
            timer = new Timer()
            {
                Interval = 1000,
            };
            timer.Tick += new EventHandler(TimerEventProcessor);
            stop.Checked = true;

            variantA.Checked = true;
            ResetPolyharmonic(this, EventArgs.Empty);
        }


        private void HValueChanged(object sender, EventArgs e)
        {
            if (CanRebuild)
            {
                CalcHarmonic();
            }
        }

        private void HValueChanged2(object sender, EventArgs e)
        {
            if (CanRebuild)
            {
                CalcPolyharmonic();
            }
        }

        private static double Function(double A, double f, double p, int n)
        {
            return A * Math.Sin(2 * Math.PI * f * n / N + p);
        }

        private void CalcHarmonic()
        {
            double A = (double)hInputA.Value;
            double f = (double)hInputF.Value;
            double p = (double)hInputP.Value / 180 * Math.PI;

            Series series = new Series
            {
                Color = System.Drawing.Color.Red,
                ChartType = SeriesChartType.Spline
            };
            for (int n = 1; n < N; n++)
            {
                double y = Function(A, f, p, n);
                series.Points.AddXY(n, y);
            }
            chart1.Series.Clear();
            chart1.Series.Add(series);
            chart1.ResetAutoValues();
        }

        private void CalcPolyharmonic()
        {
            Series series = new Series
            {
                Color = System.Drawing.Color.Red,
                ChartType = SeriesChartType.Spline
            };
            for (int n = 1; n < N; n++)
            {
                double y = 0;
                foreach (var harmonic in harmonics)
                {
                    double A = (double)harmonic.Item1.Value;
                    double f = (double)harmonic.Item2.Value;
                    double p = (double)harmonic.Item3.Value / 180 * Math.PI;

                    y += Function(A, f, p, n);
                }
                series.Points.AddXY(n, y);
            }

            chart2.Series.Clear();
            chart2.Series.Add(series);
            chart2.ResetAutoValues();
        }

        private void SetParams(double a, double f, double p)
        {
            hInputA.Value = (decimal)a;
            hInputF.Value = (decimal)f;
            hInputP.Value = (decimal)p;
        }

        private void UseVariantA(object sender, EventArgs e)
        {
            CanRebuild = false;
            SetParams(7, 5, 180);
            CanRebuild = true;
            CalcHarmonic();
        }

        private void UseVariantB(object sender, EventArgs e)
        {
            CanRebuild = false;
            SetParams(5, 1, 135);
            CanRebuild = true;
            CalcHarmonic();
        }

        private void UseVariantC(object sender, EventArgs e)
        {
            CanRebuild = false;
            SetParams(1, 3, 135);
            CanRebuild = true;
            CalcHarmonic();
        }

        private void ResetPolyharmonic(object sender, EventArgs e)
        {
            CanRebuild = false;

            hInputA1.Value = 9;
            hInputF1.Value = 1;
            hInputP1.Value = 90;

            hInputA2.Value = 9;
            hInputF2.Value = 2;
            hInputP2.Value = 0;

            hInputA3.Value = 9;
            hInputF3.Value = 3;
            hInputP3.Value = 45;

            hInputA4.Value = 9;
            hInputF4.Value = 4;
            hInputP4.Value = 60;

            hInputA5.Value = 9;
            hInputF5.Value = 5;
            hInputP5.Value = 30;

            CanRebuild = true;
            CalcPolyharmonic();
        }

        private void AminationStateChanged(object sender, EventArgs e)
        {
            if (start.Checked)
            {
                CanRebuild = false;
                Random random = new Random();
                foreach (var harmonic in harmonics)
                {
                    harmonic.Item1.Value = random.Next(0, 100);
                    harmonic.Item2.Value = random.Next(0, 100);
                    harmonic.Item3.Value = random.Next(0, 360);
                }
                CalcPolyharmonic();
                timer.Start();
            }
            else
            {
                CanRebuild = true;
                timer.Stop();
            }
        }

        private decimal CycleClaim(decimal v, decimal min, decimal max)
        {
            return v < min ? max : (v > max ? min : v);
        }

        private void TimerEventProcessor(object sender, EventArgs e)
        {
            hInputA1.Value = CycleClaim(hInputA1.Value + 2, 0, 100);
            hInputF1.Value = CycleClaim(hInputF1.Value - 3, 0, 100);
            hInputP1.Value = CycleClaim(hInputF1.Value + 5, 0, 360);

            hInputA2.Value = CycleClaim(hInputA2.Value + 5, 0, 100);
            hInputF2.Value = CycleClaim(hInputF2.Value + 2, 0, 100);
            hInputP2.Value = CycleClaim(hInputF2.Value - 3, 0, 360);

            hInputA3.Value = CycleClaim(hInputA3.Value - 3, 0, 100);
            hInputF3.Value = CycleClaim(hInputF3.Value - 2, 0, 100);
            hInputP3.Value = CycleClaim(hInputF3.Value + 3, 0, 360);

            hInputA4.Value = CycleClaim(hInputA4.Value - 2, 0, 100);
            hInputF4.Value = CycleClaim(hInputF4.Value - 3, 0, 100);
            hInputP4.Value = CycleClaim(hInputF4.Value - 4, 0, 360);

            hInputA5.Value = CycleClaim(hInputA5.Value + 1, 0, 100);
            hInputF5.Value = CycleClaim(hInputF5.Value + 3, 0, 100);
            hInputP5.Value = CycleClaim(hInputF5.Value - 5, 0, 360);

            CalcPolyharmonic();
        }
    }
}
