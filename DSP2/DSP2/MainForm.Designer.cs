namespace DSP2
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.mainFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.paramsPanel = new System.Windows.Forms.GroupBox();
            this.paramsTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.paramKLabel = new System.Windows.Forms.Label();
            this.paramPhiLabel = new System.Windows.Forms.Label();
            this.paramKInput = new System.Windows.Forms.NumericUpDown();
            this.paramPhiInput = new System.Windows.Forms.NumericUpDown();
            this.mainFlowPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.paramsPanel.SuspendLayout();
            this.paramsTablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paramKInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paramPhiInput)).BeginInit();
            this.SuspendLayout();
            // 
            // mainFlowPanel
            // 
            this.mainFlowPanel.AutoSize = true;
            this.mainFlowPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainFlowPanel.Controls.Add(this.chart);
            this.mainFlowPanel.Controls.Add(this.paramsPanel);
            this.mainFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainFlowPanel.Location = new System.Drawing.Point(0, 0);
            this.mainFlowPanel.Name = "mainFlowPanel";
            this.mainFlowPanel.Padding = new System.Windows.Forms.Padding(3);
            this.mainFlowPanel.Size = new System.Drawing.Size(1182, 613);
            this.mainFlowPanel.TabIndex = 0;
            // 
            // chart
            // 
            chartArea1.Name = "ChartArea";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Location = new System.Drawing.Point(6, 6);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Name = "Series2";
            this.chart.Series.Add(series1);
            this.chart.Series.Add(series2);
            this.chart.Size = new System.Drawing.Size(900, 600);
            this.chart.TabIndex = 0;
            this.chart.Text = "Chart";
            // 
            // paramsPanel
            // 
            this.paramsPanel.AutoSize = true;
            this.paramsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.paramsPanel.Controls.Add(this.paramsTablePanel);
            this.paramsPanel.Location = new System.Drawing.Point(912, 6);
            this.paramsPanel.Name = "paramsPanel";
            this.paramsPanel.Size = new System.Drawing.Size(264, 77);
            this.paramsPanel.TabIndex = 0;
            this.paramsPanel.TabStop = false;
            this.paramsPanel.Text = "Params";
            // 
            // paramsTablePanel
            // 
            this.paramsTablePanel.AutoSize = true;
            this.paramsTablePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.paramsTablePanel.ColumnCount = 2;
            this.paramsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.paramsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.paramsTablePanel.Controls.Add(this.paramKLabel, 0, 1);
            this.paramsTablePanel.Controls.Add(this.paramPhiLabel, 0, 2);
            this.paramsTablePanel.Controls.Add(this.paramKInput, 1, 1);
            this.paramsTablePanel.Controls.Add(this.paramPhiInput, 1, 2);
            this.paramsTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramsTablePanel.Location = new System.Drawing.Point(3, 18);
            this.paramsTablePanel.Name = "paramsTablePanel";
            this.paramsTablePanel.RowCount = 3;
            this.paramsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.paramsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.paramsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.paramsTablePanel.Size = new System.Drawing.Size(258, 56);
            this.paramsTablePanel.TabIndex = 0;
            // 
            // paramKLabel
            // 
            this.paramKLabel.AutoSize = true;
            this.paramKLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramKLabel.Location = new System.Drawing.Point(3, 3);
            this.paramKLabel.Margin = new System.Windows.Forms.Padding(3);
            this.paramKLabel.Name = "paramKLabel";
            this.paramKLabel.Size = new System.Drawing.Size(97, 22);
            this.paramKLabel.TabIndex = 1;
            this.paramKLabel.Text = "K";
            this.paramKLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // paramPhiLabel
            // 
            this.paramPhiLabel.AutoSize = true;
            this.paramPhiLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramPhiLabel.Location = new System.Drawing.Point(3, 31);
            this.paramPhiLabel.Margin = new System.Windows.Forms.Padding(3);
            this.paramPhiLabel.Name = "paramPhiLabel";
            this.paramPhiLabel.Size = new System.Drawing.Size(97, 22);
            this.paramPhiLabel.TabIndex = 2;
            this.paramPhiLabel.Text = "φ";
            this.paramPhiLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // paramKInput
            // 
            this.paramKInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramKInput.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.paramKInput.Location = new System.Drawing.Point(106, 3);
            this.paramKInput.Maximum = new decimal(new int[] {
            1023,
            0,
            0,
            0});
            this.paramKInput.Name = "paramKInput";
            this.paramKInput.Size = new System.Drawing.Size(149, 22);
            this.paramKInput.TabIndex = 4;
            this.paramKInput.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.paramKInput.ValueChanged += new System.EventHandler(this.ParamChanged);
            // 
            // paramPhiInput
            // 
            this.paramPhiInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramPhiInput.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.paramPhiInput.Location = new System.Drawing.Point(106, 31);
            this.paramPhiInput.Maximum = new decimal(new int[] {
            359,
            0,
            0,
            0});
            this.paramPhiInput.Name = "paramPhiInput";
            this.paramPhiInput.Size = new System.Drawing.Size(149, 22);
            this.paramPhiInput.TabIndex = 5;
            this.paramPhiInput.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.paramPhiInput.ValueChanged += new System.EventHandler(this.ParamChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1182, 613);
            this.Controls.Add(this.mainFlowPanel);
            this.Name = "MainForm";
            this.Text = "Digial signal processing";
            this.mainFlowPanel.ResumeLayout(false);
            this.mainFlowPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.paramsPanel.ResumeLayout(false);
            this.paramsPanel.PerformLayout();
            this.paramsTablePanel.ResumeLayout(false);
            this.paramsTablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paramKInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paramPhiInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel mainFlowPanel;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.GroupBox paramsPanel;
        private System.Windows.Forms.TableLayoutPanel paramsTablePanel;
        private System.Windows.Forms.Label paramKLabel;
        private System.Windows.Forms.Label paramPhiLabel;
        private System.Windows.Forms.NumericUpDown paramKInput;
        private System.Windows.Forms.NumericUpDown paramPhiInput;
    }
}

