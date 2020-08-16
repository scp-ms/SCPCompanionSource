namespace SCPCompanion
{
    partial class SCPCompanionForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.inputFileBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tmtFileGrid = new System.Windows.Forms.DataGridView();
            this.clearFiles = new System.Windows.Forms.Button();
            this.ms2Analysis = new System.Windows.Forms.Button();
            this.onlyScansWithSN = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.quantComboBox = new System.Windows.Forms.ComboBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.settingsTab = new System.Windows.Forms.TabPage();
            this.carrierRatioCutoffTypeComboBox = new System.Windows.Forms.ComboBox();
            this.carrierRatioCutoff = new System.Windows.Forms.TextBox();
            this.calculateCarrierRatioCB = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.regressionDGV = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.maxITAdjTB = new System.Windows.Forms.TextBox();
            this.snAdjTB = new System.Windows.Forms.TextBox();
            this.carrierRatioTB = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.slopeTB = new System.Windows.Forms.TextBox();
            this.yInterTB = new System.Windows.Forms.TextBox();
            this.regressionGC = new ZedGraph.ZedGraphControl();
            this.graphTab = new System.Windows.Forms.TabPage();
            this.carrierRatioGraphControl = new ZedGraph.ZedGraphControl();
            this.tmtIonFluxgraphControl = new ZedGraph.ZedGraphControl();
            this.tmtSumSNgraphControl = new ZedGraph.ZedGraphControl();
            this.outputPlotDataCB = new System.Windows.Forms.CheckBox();
            this.sncutoffTB = new System.Windows.Forms.TextBox();
            this.histCarrierRatioCutoffTB = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.paramsTab = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.blankTB = new System.Windows.Forms.TextBox();
            this.multiplexingChannels = new System.Windows.Forms.DataGridView();
            this.loadParamButton = new System.Windows.Forms.Button();
            this.paramNameTB = new System.Windows.Forms.TextBox();
            this.updateAndSave = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tmtQuantSNgraphControl = new ZedGraph.ZedGraphControl();
            this.tmtQuantIonFluxgraphControl = new ZedGraph.ZedGraphControl();
            this.label12 = new System.Windows.Forms.Label();
            this.snPerChannelTB = new System.Windows.Forms.TextBox();
            this.paramFileListBox = new System.Windows.Forms.ListBox();
            this.carrierEstimateGrid = new System.Windows.Forms.DataGridView();
            this.snToCarrierRatioGraphControl = new ZedGraph.ZedGraphControl();
            ((System.ComponentModel.ISupportInitialize)(this.tmtFileGrid)).BeginInit();
            this.tabControl.SuspendLayout();
            this.settingsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.regressionDGV)).BeginInit();
            this.graphTab.SuspendLayout();
            this.paramsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.multiplexingChannels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierEstimateGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // inputFileBox
            // 
            this.inputFileBox.AllowDrop = true;
            this.inputFileBox.FormattingEnabled = true;
            this.inputFileBox.Location = new System.Drawing.Point(10, 25);
            this.inputFileBox.Name = "inputFileBox";
            this.inputFileBox.Size = new System.Drawing.Size(1727, 69);
            this.inputFileBox.TabIndex = 3;
            this.inputFileBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.inputFileBox_DragDrop);
            this.inputFileBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.inputFileBox_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Input Files";
            // 
            // tmtFileGrid
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tmtFileGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.tmtFileGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tmtFileGrid.DefaultCellStyle = dataGridViewCellStyle14;
            this.tmtFileGrid.Location = new System.Drawing.Point(12, 605);
            this.tmtFileGrid.Name = "tmtFileGrid";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tmtFileGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.tmtFileGrid.Size = new System.Drawing.Size(1725, 137);
            this.tmtFileGrid.TabIndex = 9;
            this.tmtFileGrid.SelectionChanged += new System.EventHandler(this.tmtFileGrid_SelectionChanged);
            // 
            // clearFiles
            // 
            this.clearFiles.Location = new System.Drawing.Point(1537, 100);
            this.clearFiles.Margin = new System.Windows.Forms.Padding(2);
            this.clearFiles.Name = "clearFiles";
            this.clearFiles.Size = new System.Drawing.Size(94, 25);
            this.clearFiles.TabIndex = 27;
            this.clearFiles.Text = "Clear Files";
            this.clearFiles.UseVisualStyleBackColor = true;
            this.clearFiles.Click += new System.EventHandler(this.clearFiles_Click);
            // 
            // ms2Analysis
            // 
            this.ms2Analysis.Location = new System.Drawing.Point(1641, 100);
            this.ms2Analysis.Margin = new System.Windows.Forms.Padding(2);
            this.ms2Analysis.Name = "ms2Analysis";
            this.ms2Analysis.Size = new System.Drawing.Size(94, 25);
            this.ms2Analysis.TabIndex = 28;
            this.ms2Analysis.Text = "Analyze";
            this.ms2Analysis.UseVisualStyleBackColor = true;
            this.ms2Analysis.Click += new System.EventHandler(this.ms2Analysis_Click);
            // 
            // onlyScansWithSN
            // 
            this.onlyScansWithSN.AutoSize = true;
            this.onlyScansWithSN.Checked = true;
            this.onlyScansWithSN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.onlyScansWithSN.Location = new System.Drawing.Point(976, 103);
            this.onlyScansWithSN.Margin = new System.Windows.Forms.Padding(2);
            this.onlyScansWithSN.Name = "onlyScansWithSN";
            this.onlyScansWithSN.Size = new System.Drawing.Size(114, 17);
            this.onlyScansWithSN.TabIndex = 36;
            this.onlyScansWithSN.Text = "Only Analyze SN >";
            this.onlyScansWithSN.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(761, 104);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 13);
            this.label8.TabIndex = 38;
            this.label8.Text = "Quantitation MS Order";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // quantComboBox
            // 
            this.quantComboBox.FormattingEnabled = true;
            this.quantComboBox.Items.AddRange(new object[] {
            "MS2",
            "MS3"});
            this.quantComboBox.Location = new System.Drawing.Point(880, 101);
            this.quantComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.quantComboBox.Name = "quantComboBox";
            this.quantComboBox.Size = new System.Drawing.Size(62, 21);
            this.quantComboBox.TabIndex = 39;
            this.quantComboBox.Text = "MS2";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.paramsTab);
            this.tabControl.Controls.Add(this.settingsTab);
            this.tabControl.Controls.Add(this.graphTab);
            this.tabControl.Location = new System.Drawing.Point(10, 124);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1731, 475);
            this.tabControl.TabIndex = 41;
            // 
            // settingsTab
            // 
            this.settingsTab.Controls.Add(this.carrierRatioGraphControl);
            this.settingsTab.Controls.Add(this.carrierEstimateGrid);
            this.settingsTab.Controls.Add(this.snToCarrierRatioGraphControl);
            this.settingsTab.Controls.Add(this.carrierRatioCutoffTypeComboBox);
            this.settingsTab.Controls.Add(this.carrierRatioCutoff);
            this.settingsTab.Controls.Add(this.calculateCarrierRatioCB);
            this.settingsTab.Controls.Add(this.label9);
            this.settingsTab.Controls.Add(this.regressionDGV);
            this.settingsTab.Controls.Add(this.label2);
            this.settingsTab.Controls.Add(this.carrierRatioTB);
            this.settingsTab.Controls.Add(this.label7);
            this.settingsTab.Controls.Add(this.label6);
            this.settingsTab.Controls.Add(this.slopeTB);
            this.settingsTab.Controls.Add(this.yInterTB);
            this.settingsTab.Controls.Add(this.regressionGC);
            this.settingsTab.Location = new System.Drawing.Point(4, 22);
            this.settingsTab.Name = "settingsTab";
            this.settingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.settingsTab.Size = new System.Drawing.Size(1723, 449);
            this.settingsTab.TabIndex = 1;
            this.settingsTab.Text = "carrier proteome estimation";
            this.settingsTab.UseVisualStyleBackColor = true;
            // 
            // carrierRatioCutoffTypeComboBox
            // 
            this.carrierRatioCutoffTypeComboBox.FormattingEnabled = true;
            this.carrierRatioCutoffTypeComboBox.Items.AddRange(new object[] {
            "Percent",
            "Signal to Noise"});
            this.carrierRatioCutoffTypeComboBox.Location = new System.Drawing.Point(144, 62);
            this.carrierRatioCutoffTypeComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.carrierRatioCutoffTypeComboBox.Name = "carrierRatioCutoffTypeComboBox";
            this.carrierRatioCutoffTypeComboBox.Size = new System.Drawing.Size(99, 21);
            this.carrierRatioCutoffTypeComboBox.TabIndex = 55;
            this.carrierRatioCutoffTypeComboBox.Text = "Percent";
            // 
            // carrierRatioCutoff
            // 
            this.carrierRatioCutoff.Location = new System.Drawing.Point(248, 63);
            this.carrierRatioCutoff.Name = "carrierRatioCutoff";
            this.carrierRatioCutoff.Size = new System.Drawing.Size(71, 20);
            this.carrierRatioCutoff.TabIndex = 54;
            this.carrierRatioCutoff.Text = "15";
            this.carrierRatioCutoff.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // calculateCarrierRatioCB
            // 
            this.calculateCarrierRatioCB.AutoSize = true;
            this.calculateCarrierRatioCB.Checked = true;
            this.calculateCarrierRatioCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.calculateCarrierRatioCB.Location = new System.Drawing.Point(9, 66);
            this.calculateCarrierRatioCB.Margin = new System.Windows.Forms.Padding(2);
            this.calculateCarrierRatioCB.Name = "calculateCarrierRatioCB";
            this.calculateCarrierRatioCB.Size = new System.Drawing.Size(131, 17);
            this.calculateCarrierRatioCB.TabIndex = 42;
            this.calculateCarrierRatioCB.Text = "Calculate Carrier Ratio";
            this.calculateCarrierRatioCB.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(227, 18);
            this.label9.TabIndex = 53;
            this.label9.Text = "Carrier Proteome Adjustment";
            // 
            // regressionDGV
            // 
            this.regressionDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.regressionDGV.Location = new System.Drawing.Point(9, 88);
            this.regressionDGV.Margin = new System.Windows.Forms.Padding(2);
            this.regressionDGV.Name = "regressionDGV";
            this.regressionDGV.RowTemplate.Height = 24;
            this.regressionDGV.Size = new System.Drawing.Size(310, 236);
            this.regressionDGV.TabIndex = 50;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 346);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 46;
            this.label4.Text = "Max IT Adjustment";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 308);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 45;
            this.label3.Text = "s/n Cuttoff Adjustment";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 44;
            this.label2.Text = "Carrier Proteome Level";
            // 
            // maxITAdjTB
            // 
            this.maxITAdjTB.Location = new System.Drawing.Point(146, 343);
            this.maxITAdjTB.Name = "maxITAdjTB";
            this.maxITAdjTB.Size = new System.Drawing.Size(71, 20);
            this.maxITAdjTB.TabIndex = 43;
            this.maxITAdjTB.Text = "2.00";
            this.maxITAdjTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // snAdjTB
            // 
            this.snAdjTB.Location = new System.Drawing.Point(146, 305);
            this.snAdjTB.Name = "snAdjTB";
            this.snAdjTB.Size = new System.Drawing.Size(71, 20);
            this.snAdjTB.TabIndex = 42;
            this.snAdjTB.Text = "1.05";
            this.snAdjTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // carrierRatioTB
            // 
            this.carrierRatioTB.Location = new System.Drawing.Point(248, 35);
            this.carrierRatioTB.Name = "carrierRatioTB";
            this.carrierRatioTB.Size = new System.Drawing.Size(71, 20);
            this.carrierRatioTB.TabIndex = 41;
            this.carrierRatioTB.Text = "0";
            this.carrierRatioTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(553, 296);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "y-intercept";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(425, 296);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 37;
            this.label6.Text = "slope";
            // 
            // slopeTB
            // 
            this.slopeTB.Location = new System.Drawing.Point(460, 294);
            this.slopeTB.Name = "slopeTB";
            this.slopeTB.Size = new System.Drawing.Size(80, 20);
            this.slopeTB.TabIndex = 36;
            // 
            // yInterTB
            // 
            this.yInterTB.Location = new System.Drawing.Point(613, 294);
            this.yInterTB.Name = "yInterTB";
            this.yInterTB.Size = new System.Drawing.Size(80, 20);
            this.yInterTB.TabIndex = 35;
            // 
            // regressionGC
            // 
            this.regressionGC.Location = new System.Drawing.Point(337, 10);
            this.regressionGC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.regressionGC.Name = "regressionGC";
            this.regressionGC.ScrollGrace = 0D;
            this.regressionGC.ScrollMaxX = 0D;
            this.regressionGC.ScrollMaxY = 0D;
            this.regressionGC.ScrollMaxY2 = 0D;
            this.regressionGC.ScrollMinX = 0D;
            this.regressionGC.ScrollMinY = 0D;
            this.regressionGC.ScrollMinY2 = 0D;
            this.regressionGC.Size = new System.Drawing.Size(439, 267);
            this.regressionGC.TabIndex = 26;
            this.regressionGC.UseExtendedPrintDialog = true;
            // 
            // graphTab
            // 
            this.graphTab.Controls.Add(this.tmtIonFluxgraphControl);
            this.graphTab.Controls.Add(this.tmtSumSNgraphControl);
            this.graphTab.Location = new System.Drawing.Point(4, 22);
            this.graphTab.Name = "graphTab";
            this.graphTab.Padding = new System.Windows.Forms.Padding(3);
            this.graphTab.Size = new System.Drawing.Size(1723, 449);
            this.graphTab.TabIndex = 0;
            this.graphTab.Text = "sum s/n graphs";
            this.graphTab.UseVisualStyleBackColor = true;
            // 
            // carrierRatioGraphControl
            // 
            this.carrierRatioGraphControl.Location = new System.Drawing.Point(1379, 10);
            this.carrierRatioGraphControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.carrierRatioGraphControl.Name = "carrierRatioGraphControl";
            this.carrierRatioGraphControl.ScrollGrace = 0D;
            this.carrierRatioGraphControl.ScrollMaxX = 0D;
            this.carrierRatioGraphControl.ScrollMaxY = 0D;
            this.carrierRatioGraphControl.ScrollMaxY2 = 0D;
            this.carrierRatioGraphControl.ScrollMinX = 0D;
            this.carrierRatioGraphControl.ScrollMinY = 0D;
            this.carrierRatioGraphControl.ScrollMinY2 = 0D;
            this.carrierRatioGraphControl.Size = new System.Drawing.Size(330, 314);
            this.carrierRatioGraphControl.TabIndex = 15;
            this.carrierRatioGraphControl.UseExtendedPrintDialog = true;
            // 
            // tmtIonFluxgraphControl
            // 
            this.tmtIonFluxgraphControl.Location = new System.Drawing.Point(876, 11);
            this.tmtIonFluxgraphControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tmtIonFluxgraphControl.Name = "tmtIonFluxgraphControl";
            this.tmtIonFluxgraphControl.ScrollGrace = 0D;
            this.tmtIonFluxgraphControl.ScrollMaxX = 0D;
            this.tmtIonFluxgraphControl.ScrollMaxY = 0D;
            this.tmtIonFluxgraphControl.ScrollMaxY2 = 0D;
            this.tmtIonFluxgraphControl.ScrollMinX = 0D;
            this.tmtIonFluxgraphControl.ScrollMinY = 0D;
            this.tmtIonFluxgraphControl.ScrollMinY2 = 0D;
            this.tmtIonFluxgraphControl.Size = new System.Drawing.Size(830, 407);
            this.tmtIonFluxgraphControl.TabIndex = 13;
            this.tmtIonFluxgraphControl.UseExtendedPrintDialog = true;
            // 
            // tmtSumSNgraphControl
            // 
            this.tmtSumSNgraphControl.Location = new System.Drawing.Point(7, 8);
            this.tmtSumSNgraphControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tmtSumSNgraphControl.Name = "tmtSumSNgraphControl";
            this.tmtSumSNgraphControl.ScrollGrace = 0D;
            this.tmtSumSNgraphControl.ScrollMaxX = 0D;
            this.tmtSumSNgraphControl.ScrollMaxY = 0D;
            this.tmtSumSNgraphControl.ScrollMaxY2 = 0D;
            this.tmtSumSNgraphControl.ScrollMinX = 0D;
            this.tmtSumSNgraphControl.ScrollMinY = 0D;
            this.tmtSumSNgraphControl.ScrollMinY2 = 0D;
            this.tmtSumSNgraphControl.Size = new System.Drawing.Size(861, 407);
            this.tmtSumSNgraphControl.TabIndex = 12;
            this.tmtSumSNgraphControl.UseExtendedPrintDialog = true;
            // 
            // outputPlotDataCB
            // 
            this.outputPlotDataCB.AutoSize = true;
            this.outputPlotDataCB.Checked = true;
            this.outputPlotDataCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.outputPlotDataCB.Location = new System.Drawing.Point(1411, 104);
            this.outputPlotDataCB.Margin = new System.Windows.Forms.Padding(2);
            this.outputPlotDataCB.Name = "outputPlotDataCB";
            this.outputPlotDataCB.Size = new System.Drawing.Size(105, 17);
            this.outputPlotDataCB.TabIndex = 42;
            this.outputPlotDataCB.Text = "Output Plot Data";
            this.outputPlotDataCB.UseVisualStyleBackColor = true;
            // 
            // sncutoffTB
            // 
            this.sncutoffTB.Location = new System.Drawing.Point(1095, 101);
            this.sncutoffTB.Name = "sncutoffTB";
            this.sncutoffTB.Size = new System.Drawing.Size(56, 20);
            this.sncutoffTB.TabIndex = 57;
            this.sncutoffTB.Text = "0";
            this.sncutoffTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // histCarrierRatioCutoffTB
            // 
            this.histCarrierRatioCutoffTB.Location = new System.Drawing.Point(1328, 102);
            this.histCarrierRatioCutoffTB.Name = "histCarrierRatioCutoffTB";
            this.histCarrierRatioCutoffTB.Size = new System.Drawing.Size(56, 20);
            this.histCarrierRatioCutoffTB.TabIndex = 59;
            this.histCarrierRatioCutoffTB.Text = "0";
            this.histCarrierRatioCutoffTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(1183, 103);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(146, 17);
            this.checkBox1.TabIndex = 58;
            this.checkBox1.Text = "Histogram Carrier Ratio  >";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // paramsTab
            // 
            this.paramsTab.Controls.Add(this.paramFileListBox);
            this.paramsTab.Controls.Add(this.label12);
            this.paramsTab.Controls.Add(this.snPerChannelTB);
            this.paramsTab.Controls.Add(this.tmtQuantIonFluxgraphControl);
            this.paramsTab.Controls.Add(this.tmtQuantSNgraphControl);
            this.paramsTab.Controls.Add(this.label11);
            this.paramsTab.Controls.Add(this.label10);
            this.paramsTab.Controls.Add(this.blankTB);
            this.paramsTab.Controls.Add(this.label4);
            this.paramsTab.Controls.Add(this.multiplexingChannels);
            this.paramsTab.Controls.Add(this.loadParamButton);
            this.paramsTab.Controls.Add(this.label3);
            this.paramsTab.Controls.Add(this.paramNameTB);
            this.paramsTab.Controls.Add(this.updateAndSave);
            this.paramsTab.Controls.Add(this.label5);
            this.paramsTab.Controls.Add(this.maxITAdjTB);
            this.paramsTab.Controls.Add(this.snAdjTB);
            this.paramsTab.Location = new System.Drawing.Point(4, 22);
            this.paramsTab.Name = "paramsTab";
            this.paramsTab.Padding = new System.Windows.Forms.Padding(3);
            this.paramsTab.Size = new System.Drawing.Size(1723, 449);
            this.paramsTab.TabIndex = 2;
            this.paramsTab.Text = "suggested instrument params";
            this.paramsTab.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 420);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(191, 13);
            this.label10.TabIndex = 64;
            this.label10.Text = "Number of Blanks in \'Sample\' Channels";
            // 
            // blankTB
            // 
            this.blankTB.Location = new System.Drawing.Point(204, 417);
            this.blankTB.Name = "blankTB";
            this.blankTB.Size = new System.Drawing.Size(104, 20);
            this.blankTB.TabIndex = 63;
            this.blankTB.Text = "0";
            this.blankTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // multiplexingChannels
            // 
            this.multiplexingChannels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.multiplexingChannels.Location = new System.Drawing.Point(395, 37);
            this.multiplexingChannels.Margin = new System.Windows.Forms.Padding(2);
            this.multiplexingChannels.Name = "multiplexingChannels";
            this.multiplexingChannels.RowTemplate.Height = 24;
            this.multiplexingChannels.Size = new System.Drawing.Size(272, 407);
            this.multiplexingChannels.TabIndex = 62;
            // 
            // loadParamButton
            // 
            this.loadParamButton.Location = new System.Drawing.Point(304, 255);
            this.loadParamButton.Margin = new System.Windows.Forms.Padding(2);
            this.loadParamButton.Name = "loadParamButton";
            this.loadParamButton.Size = new System.Drawing.Size(76, 24);
            this.loadParamButton.TabIndex = 61;
            this.loadParamButton.Text = "Load";
            this.loadParamButton.UseVisualStyleBackColor = true;
            this.loadParamButton.Click += new System.EventHandler(this.loadParamButton_Click);
            // 
            // paramNameTB
            // 
            this.paramNameTB.Location = new System.Drawing.Point(6, 37);
            this.paramNameTB.Name = "paramNameTB";
            this.paramNameTB.Size = new System.Drawing.Size(269, 20);
            this.paramNameTB.TabIndex = 60;
            this.paramNameTB.Text = "defaultParams";
            // 
            // updateAndSave
            // 
            this.updateAndSave.Location = new System.Drawing.Point(280, 34);
            this.updateAndSave.Margin = new System.Windows.Forms.Padding(2);
            this.updateAndSave.Name = "updateAndSave";
            this.updateAndSave.Size = new System.Drawing.Size(100, 25);
            this.updateAndSave.TabIndex = 59;
            this.updateAndSave.Text = "Update and Save";
            this.updateAndSave.UseVisualStyleBackColor = true;
            this.updateAndSave.Click += new System.EventHandler(this.updateAndSave_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(392, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(171, 18);
            this.label5.TabIndex = 58;
            this.label5.Text = "Multiplexing Channels";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(6, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(191, 18);
            this.label11.TabIndex = 65;
            this.label11.Text = "SCPCompanion Params";
            // 
            // tmtQuantSNgraphControl
            // 
            this.tmtQuantSNgraphControl.Location = new System.Drawing.Point(686, 37);
            this.tmtQuantSNgraphControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tmtQuantSNgraphControl.Name = "tmtQuantSNgraphControl";
            this.tmtQuantSNgraphControl.ScrollGrace = 0D;
            this.tmtQuantSNgraphControl.ScrollMaxX = 0D;
            this.tmtQuantSNgraphControl.ScrollMaxY = 0D;
            this.tmtQuantSNgraphControl.ScrollMaxY2 = 0D;
            this.tmtQuantSNgraphControl.ScrollMinX = 0D;
            this.tmtQuantSNgraphControl.ScrollMinY = 0D;
            this.tmtQuantSNgraphControl.ScrollMinY2 = 0D;
            this.tmtQuantSNgraphControl.Size = new System.Drawing.Size(511, 407);
            this.tmtQuantSNgraphControl.TabIndex = 66;
            this.tmtQuantSNgraphControl.UseExtendedPrintDialog = true;
            // 
            // tmtQuantIonFluxgraphControl
            // 
            this.tmtQuantIonFluxgraphControl.Location = new System.Drawing.Point(1205, 37);
            this.tmtQuantIonFluxgraphControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tmtQuantIonFluxgraphControl.Name = "tmtQuantIonFluxgraphControl";
            this.tmtQuantIonFluxgraphControl.ScrollGrace = 0D;
            this.tmtQuantIonFluxgraphControl.ScrollMaxX = 0D;
            this.tmtQuantIonFluxgraphControl.ScrollMaxY = 0D;
            this.tmtQuantIonFluxgraphControl.ScrollMaxY2 = 0D;
            this.tmtQuantIonFluxgraphControl.ScrollMinX = 0D;
            this.tmtQuantIonFluxgraphControl.ScrollMinY = 0D;
            this.tmtQuantIonFluxgraphControl.ScrollMinY2 = 0D;
            this.tmtQuantIonFluxgraphControl.Size = new System.Drawing.Size(511, 407);
            this.tmtQuantIonFluxgraphControl.TabIndex = 67;
            this.tmtQuantIonFluxgraphControl.UseExtendedPrintDialog = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 384);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(180, 13);
            this.label12.TabIndex = 69;
            this.label12.Text = "Signal to Noise per \'Sample\' Channel";
            // 
            // snPerChannelTB
            // 
            this.snPerChannelTB.Location = new System.Drawing.Point(204, 381);
            this.snPerChannelTB.Name = "snPerChannelTB";
            this.snPerChannelTB.Size = new System.Drawing.Size(104, 20);
            this.snPerChannelTB.TabIndex = 68;
            this.snPerChannelTB.Text = "15";
            this.snPerChannelTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // paramFileListBox
            // 
            this.paramFileListBox.AllowDrop = true;
            this.paramFileListBox.FormattingEnabled = true;
            this.paramFileListBox.Location = new System.Drawing.Point(6, 64);
            this.paramFileListBox.Name = "paramFileListBox";
            this.paramFileListBox.Size = new System.Drawing.Size(374, 186);
            this.paramFileListBox.TabIndex = 60;
            // 
            // carrierEstimateGrid
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.carrierEstimateGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.carrierEstimateGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.carrierEstimateGrid.DefaultCellStyle = dataGridViewCellStyle17;
            this.carrierEstimateGrid.Location = new System.Drawing.Point(9, 332);
            this.carrierEstimateGrid.Name = "carrierEstimateGrid";
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.carrierEstimateGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.carrierEstimateGrid.Size = new System.Drawing.Size(1700, 85);
            this.carrierEstimateGrid.TabIndex = 60;
            // 
            // snToCarrierRatioGraphControl
            // 
            this.snToCarrierRatioGraphControl.Location = new System.Drawing.Point(784, 10);
            this.snToCarrierRatioGraphControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.snToCarrierRatioGraphControl.Name = "snToCarrierRatioGraphControl";
            this.snToCarrierRatioGraphControl.ScrollGrace = 0D;
            this.snToCarrierRatioGraphControl.ScrollMaxX = 0D;
            this.snToCarrierRatioGraphControl.ScrollMaxY = 0D;
            this.snToCarrierRatioGraphControl.ScrollMaxY2 = 0D;
            this.snToCarrierRatioGraphControl.ScrollMinX = 0D;
            this.snToCarrierRatioGraphControl.ScrollMinY = 0D;
            this.snToCarrierRatioGraphControl.ScrollMinY2 = 0D;
            this.snToCarrierRatioGraphControl.Size = new System.Drawing.Size(577, 314);
            this.snToCarrierRatioGraphControl.TabIndex = 14;
            this.snToCarrierRatioGraphControl.UseExtendedPrintDialog = true;
            // 
            // SCPCompanionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1753, 752);
            this.Controls.Add(this.histCarrierRatioCutoffTB);
            this.Controls.Add(this.sncutoffTB);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.outputPlotDataCB);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.quantComboBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.onlyScansWithSN);
            this.Controls.Add(this.ms2Analysis);
            this.Controls.Add(this.clearFiles);
            this.Controls.Add(this.tmtFileGrid);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inputFileBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SCPCompanionForm";
            this.Text = "SCPCompanion";
            ((System.ComponentModel.ISupportInitialize)(this.tmtFileGrid)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.settingsTab.ResumeLayout(false);
            this.settingsTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.regressionDGV)).EndInit();
            this.graphTab.ResumeLayout(false);
            this.paramsTab.ResumeLayout(false);
            this.paramsTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.multiplexingChannels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.carrierEstimateGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox inputFileBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView tmtFileGrid;
        private System.Windows.Forms.Button clearFiles;
        private System.Windows.Forms.Button ms2Analysis;
        private System.Windows.Forms.CheckBox onlyScansWithSN;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox quantComboBox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage graphTab;
        private ZedGraph.ZedGraphControl tmtIonFluxgraphControl;
        private ZedGraph.ZedGraphControl tmtSumSNgraphControl;
        private System.Windows.Forms.TabPage settingsTab;
        private System.Windows.Forms.DataGridView regressionDGV;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox maxITAdjTB;
        private System.Windows.Forms.TextBox snAdjTB;
        private System.Windows.Forms.TextBox carrierRatioTB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox slopeTB;
        private System.Windows.Forms.TextBox yInterTB;
        private ZedGraph.ZedGraphControl regressionGC;
        private ZedGraph.ZedGraphControl carrierRatioGraphControl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox calculateCarrierRatioCB;
        private System.Windows.Forms.ComboBox carrierRatioCutoffTypeComboBox;
        private System.Windows.Forms.TextBox carrierRatioCutoff;
        private System.Windows.Forms.CheckBox outputPlotDataCB;
        private System.Windows.Forms.TextBox sncutoffTB;
        private System.Windows.Forms.TextBox histCarrierRatioCutoffTB;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TabPage paramsTab;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox blankTB;
        private System.Windows.Forms.DataGridView multiplexingChannels;
        private System.Windows.Forms.Button loadParamButton;
        private System.Windows.Forms.TextBox paramNameTB;
        private System.Windows.Forms.Button updateAndSave;
        private System.Windows.Forms.Label label5;
        private ZedGraph.ZedGraphControl tmtQuantIonFluxgraphControl;
        private ZedGraph.ZedGraphControl tmtQuantSNgraphControl;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ListBox paramFileListBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox snPerChannelTB;
        private System.Windows.Forms.DataGridView carrierEstimateGrid;
        private ZedGraph.ZedGraphControl snToCarrierRatioGraphControl;
    }
}

