using Core;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DSP2
{
    public partial class MainForm : Form
    {
        private const int N = 1024;
        private const string SEIRES1 = "Series1";
        private const string SERIES2 = "Series2";

        public MainForm()
        {
            InitializeComponent();
            Series1 = chart.Series.FindByName(SEIRES1);
            Series2 = chart.Series.FindByName(SERIES2);
            ParamChanged(this, EventArgs.Empty);
        }

        public Series Series1 { get; set; }
        public Series Series2 { get; set; }

        private void ParamChanged(object sender, EventArgs e)
        {
            var k = decimal.ToInt32(paramKInput.Value);
            var phi = decimal.ToDouble(paramPhiInput.Value);


            var len = 2 * N - k + 1;
            var values1 = new double[len];
            var values2 = new double[len];

            void function1(int m, double v) => values1[m - k] = v;
            void function2(int m, double v) => values2[m - k] = v;

            SignalUtils.Run(k, N, SignalUtils.ToRadians(phi), function1, function2);


            Series1.Points.Clear();
            Series2.Points.Clear();
            for (int i = 0; i < values1.Length; i++)
            {
                Series1.Points.AddXY(k + i, values1[i]);
            }
            for (int i = 0; i < values2.Length; i++)
            {
                Series2.Points.AddXY(k + i, values2[i]);
            }

            chart.ResetAutoValues();
        }
    }
}
