using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using ZedGraph;
using MathNet.Numerics;

using ThermoFisher.CommonCore.Data.Business;
using ThermoFisher.CommonCore.Data.FilterEnums;
using ThermoFisher.CommonCore.Data.Interfaces;
using ThermoFisher.CommonCore.RawFileReader;

using CSMSL;
using CSMSL.Analysis;
using CSMSL.Chemistry;
using CSMSL.IO;
using CSMSL.Proteomics;
using CSMSL.Spectral;
using CSMSL.Util;

using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace SCPCompanion
{
    public partial class SCPCompanionForm : Form
    {
        BindingList<string> InputFiles;
        BindingList<AnalyzedRun> AnalyzedRuns;
        BindingList<RegressionPoint> RegressionList;
        BindingList<MultiplexingLine> MultiplexingLines;
        List<Modification> TMTIons;
        GraphPane regressionPane;
        GraphPane snPane;
        GraphPane ionFluxPane;
        GraphPane quantsnPane;
        GraphPane quantionFluxPane;
        GraphPane snToCarrierPane;
        GraphPane carrierRatioPane;
        BindingList<string> ParamsFiles;
        bool RunGridInitialized;
        string Path;
        string ParamPath;
        int QuantOrder;

        public SCPCompanionForm()
        {
            InitializeComponent();

            Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SCPCompanionFiles");
            if (!System.IO.Directory.Exists(Path)) { System.IO.Directory.CreateDirectory(Path); }

            ParamPath = Path + "\\params";
            if (!System.IO.Directory.Exists(ParamPath)) { System.IO.Directory.CreateDirectory(ParamPath); }

            ParamsFiles = new BindingList<string>();
            paramFileListBox.DataSource = ParamsFiles;

            InitializeRegressionPlot();
            InitializeSNPlot();
            InitializeIonFluxPlot();
            InitializeSNtoCarrierRatioPlot();
            InitializeCarrierRaioPlot();

            MultiplexingLines = new BindingList<MultiplexingLine>();
            multiplexingChannels.DataSource = MultiplexingLines;

            List<Modification> tmtIons = InitializeTMT();

            regressionDGV.AutoGenerateColumns = false;
            RegressionList = new BindingList<RegressionPoint>();
            regressionDGV.DataSource = RegressionList;
            InitializeRegressionList();
            PlotRegression();

            RunGridInitialized = false;
            InputFiles = new BindingList<string>();
            inputFileBox.DataSource = InputFiles;

            AnalyzedRuns = new BindingList<AnalyzedRun>();



            UpdateParamFileList();
        }

        private void ms2Analysis_Click(object sender, EventArgs e)
        {
            string quantOrderString = quantComboBox.Text;
            int quantOrder = -1;
            if(quantOrderString == "MS2")
            {
                quantOrder = 2;

                if (!RunGridInitialized) { InitializeMS2RunGrid(); }
            }
            if (quantOrderString == "MS3")
            {
                quantOrder = 3;

                if (!RunGridInitialized) { InitializeMS3RunGrid(); }
            }

            QuantOrder = quantOrder;

            runAnalysis(quantOrder);
        }

        private void clearFiles_Click(object sender, EventArgs e)
        {
            InputFiles.Clear();
        }

        private void UpdateParamFileList()
        {
            Directory.CreateDirectory(".\\ModificationFiles");

            ParamsFiles.Clear();
            foreach (string fileName in Directory.GetFiles(ParamPath, "*.csv", SearchOption.TopDirectoryOnly).ToList())
            {
                ParamsFiles.Add(fileName);
            }
        }

        private void runAnalysis(int quantOrder)
        {
            Dictionary<string, string> RawFiles = new Dictionary<string, string>();
            Dictionary<string, string> ResultFiles = new Dictionary<string, string>();

            //For each of the input files
            foreach (string filename in InputFiles)
            {
                if (filename.Contains(".raw"))
                {
                    RawFiles.Add(filename, filename);
                }
                if (filename.Contains(".csv"))
                {
                    ResultFiles.Add(filename, filename);
                }
            }

            List<AnalyzedRun> analyzedRuns = new List<AnalyzedRun>();
            foreach (string rawFileName in RawFiles.Keys)
            {
                AnalyzedRun analyzedRun = AnalyzeRawFile(quantOrder, rawFileName, TMTIons);
                analyzedRuns.Add(analyzedRun);
            }

            //Add the lines that will be added to the grid
            //
            using (StreamWriter writer = new StreamWriter(Path + "\\SCPCompanionSummary_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv"))
            {
                if (QuantOrder == 2) { writer.WriteLine("RawFileName,MS1Scans,MS2Scans,Median MS2 IT,Median Sum SN,Median Ion Flux,Meian Quant SN,Median Quant Ion Flux,Estimated Carrier Ratio,Suggested Quant SN Cutoff,Suggested Max IT,SN Adjustment,Max IT Adjustment,SN per Channel,Number of Blanks"); }
                else if (QuantOrder == 3) { writer.WriteLine("RawFileName,MS1Scans,MS2Scans,MS3Scans,Median MS2 IT,Median MS3 IT,Median Sum SN,Median Ion Flux,Meian Quant SN,Median Quant Ion Flux,Estimated Carrier Ratio,Suggested Quant SN Cutoff,Suggested Max IT,SN Adjustment,Max IT Adjustment,SN per Channel,Number of Blanks"); }

                foreach (AnalyzedRun run in analyzedRuns)
                {
                    //Print the quant results in separate files
                    run.PrintQuantScans(Path, TMTIons);

                    //Write the summary for the analyzed run
                    writer.WriteLine(run.PrintSummary() + "," + snAdjTB.Text + "," + maxITAdjTB.Text + "," + snPerChannelTB.Text + "," + blankTB.Text);

                    //Add the analyzed run to the list
                    AnalyzedRuns.Add(run);        
                }
            }

            //If we analyzed a run then update the TMT plots
            if(AnalyzedRuns.Count > 0)
            {
                UpdateTMTPlots(quantOrder, AnalyzedRuns[0]);
            }
        }

        private AnalyzedRun AnalyzeRawFile(int quantOrder, string rawFileName, List<Modification> tmtIons)
        {
            AnalyzedRun analyzedRun = null;

            //Make sure you can open the raw file
            IRawDataPlus rawFile = RawFileReaderAdapter.FileFactory(rawFileName);
            if (!rawFile.IsOpen || rawFile.IsError)
            {
                Console.WriteLine(" Error: unable to access the RAW file using the RawFileReader class.");
                return analyzedRun;
            }

            //Select the instrument on the raw file
            rawFile.SelectInstrument(Device.MS, 1);

            //Cycle through the scans to extract the data
            int firstSpectrumNumber = rawFile.RunHeaderEx.FirstSpectrum;
            int lastSpectrumNumber = rawFile.RunHeaderEx.LastSpectrum;

            //Map the scans
            Dictionary<int, MSnScan> mappedScans = new Dictionary<int, MSnScan>();
            int MS1scans = 0;
            int MS2scans = 0;
            int MS3scans = 0;
            for (int i = firstSpectrumNumber; i <= lastSpectrumNumber; i++)
            {
                //Console.WriteLine("Spectrum " + i);

                //The trailer data is stored in two different arrays...which is annoying. So here they are combined
                IScanFilter filter = rawFile.GetFilterForScanNumber(i);
                var trailerData = rawFile.GetTrailerExtraInformation(i);
                Dictionary<string, string> trailerDict = new Dictionary<string, string>();
                int l = 0;
                foreach (string label in trailerData.Labels)
                {
                    trailerDict.Add(trailerData.Labels.ElementAt(l), trailerData.Values.ElementAt(l));
                    l++;
                }

                if (filter.MSOrder == MSOrderType.Ms2 || filter.MSOrder == MSOrderType.Ms3)
                {
                    MSnScan addScan = new MSnScan();

                    //Do everything you would for both types
                    if (filter.MSOrder == MSOrderType.Ms2) {
                        addScan.Order = 2;
                        MS2scans++;
                    } else {
                        addScan.Order = 3;
                        MS3scans++;
                    }

                    addScan.ScanNumber = i;
                    addScan.InjectionTime = double.Parse(trailerDict["Ion Injection Time (ms):"]); //TODO: May not work with QE
                    addScan.RetentionTime = rawFile.RetentionTimeFromScanNumber(i);

                    addScan.ParentScanNumber = -1;
                    string outStr = "";
                    if (trailerDict.TryGetValue("Master Scan Number:", out outStr))
                    {
                        addScan.ParentScanNumber = int.Parse(trailerDict["Master Scan Number:"]);
                    }

                    //If it is an MS3 we will need to find the MS2 and then edit the child parameters
                    MSnScan parentScan = null;
                    if(mappedScans.TryGetValue(addScan.ParentScanNumber, out parentScan))
                    {
                        parentScan.ChildScanNumber = addScan.ScanNumber;
                        parentScan.ChildScan = addScan;

                        addScan.ParentScan = addScan;
                        addScan.ParentInjectionTime = parentScan.InjectionTime;
                    }

                    SortedList<double, double> lowMasses = new SortedList<double, double>();
                    var allData = rawFile.GetAdvancedPacketData(i);
                    CentroidStream centroidData = allData.CentroidData;
                    foreach (ICentroidPeak peak in centroidData.GetCentroids())
                    {
                        if (peak.Mass < 135)
                        {
                            lowMasses.Add(peak.Mass, peak.SignalToNoise);
                        }
                    }

                    //Search for the peaks...not a great solution right now. 
                    double sumSN = 0;
                    foreach (Modification tmtion in tmtIons)
                    {
                        MzRange tmtIonRange = new MzRange(tmtion.MonoisotopicMass, new Tolerance(ToleranceUnit.PPM, 15));

                        int index = ~Array.BinarySearch(lowMasses.Keys.ToArray(), tmtIonRange.Minimum);

                        double maxSN = 0;
                        double measuredMZ = 0;
                        if (index != 0 && index < lowMasses.Count)
                        {
                            while (lowMasses.ElementAt(index).Key < tmtIonRange.Maximum)
                            {
                                if (tmtIonRange.Contains(lowMasses.ElementAt(index).Key))
                                {
                                    if(lowMasses.ElementAt(index).Value > maxSN)
                                    {
                                        maxSN = lowMasses.ElementAt(index).Value;
                                        measuredMZ = lowMasses.ElementAt(index).Key;
                                    }
                                }

                                index++;

                                if (index >= lowMasses.Count)
                                {
                                    break;
                                }
                            }
                        }

                        addScan.SNDict.Add(tmtion.Name, maxSN);

                        double massError = (measuredMZ - tmtion.MonoisotopicMass) / tmtion.MonoisotopicMass * 1000000;
                        addScan.MassErrorDict.Add(tmtion.Name, massError);

                        sumSN += maxSN;
                    }

                    addScan.SumSN = sumSN;
                    addScan.IonFlux = sumSN / addScan.InjectionTime;

                    if(onlyScansWithSN.Checked && addScan.Order == quantOrder)
                    {
                        if (addScan.SumSN > double.Parse(sncutoffTB.Text))
                        {
                            mappedScans.Add(addScan.ScanNumber, addScan);
                        }
                    }
                    else
                    {
                        mappedScans.Add(addScan.ScanNumber, addScan);
                    }
                }
                else if(filter.MSOrder == MSOrderType.Ms)
                {
                    MS1scans++;
                }
            }


            //Using the user inputs calculate the suggested values
            analyzedRun = new AnalyzedRun(quantOrder, rawFileName, MS1scans, MS2scans, MS3scans, mappedScans.Values.ToList(), MultiplexingLines.ToList(), carrierRatioCutoffTypeComboBox.Text, carrierRatioCutoff.Text, blankTB.Text, histCarrierRatioCutoffTB.Text, snPerChannelTB.Text);
            analyzedRun.CalculateSuggestedValues(double.Parse(carrierRatioTB.Text), double.Parse(slopeTB.Text), double.Parse(yInterTB.Text), double.Parse(maxITAdjTB.Text), calculateCarrierRatioCB.Checked);
            analyzedRun.CalculateQuantSNSuggestedValues(double.Parse(maxITAdjTB.Text), double.Parse(snAdjTB.Text));

            return analyzedRun;
        }

        #region User Interface

        private void tmtFileGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (AnalyzedRuns.Count != 0)
            {
                int index = 0;
                if (tmtFileGrid.CurrentCell != null)
                {
                    index = tmtFileGrid.CurrentCell.RowIndex;
                }

                AnalyzedRun run = AnalyzedRuns[index];

                UpdateTMTPlots(QuantOrder, run);
            }
        }

        private void updateAndSave_Click(object sender, EventArgs e)
        {
            List<RegressionPoint> newPoints = new List<RegressionPoint>();
            foreach (DataGridViewRow row in regressionDGV.Rows)
            {
                double carrierLevel = double.Parse(row.Cells[0].Value.ToString());
                double snValue = double.Parse(row.Cells[1].Value.ToString());

                newPoints.Add(new RegressionPoint(carrierLevel, snValue, double.Parse(snAdjTB.Text)));
            }

            RegressionList.Clear();
            foreach (RegressionPoint point in newPoints)
            {
                RegressionList.Add(point);
            }

            PlotRegression();

            //When we update we will want to write out a new summary file
            using (StreamWriter writer = new StreamWriter(Path + "\\SCPCompanionSummary_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv"))
            {
                if (QuantOrder == 2) { writer.WriteLine("RawFileName,MS1Scans,MS2Scans,Median MS2 IT,Median Sum SN,Median Ion Flux,User Defined Carrier Ratio,Calculated Carrier Ratio,Used Calculated Carrier Rato,Suggested SN Cutoff,Suggested Max IT,SN Adjustment,Max IT Adjustment,Slope,yIntercept"); }
                else if (QuantOrder == 3) { writer.WriteLine("RawFileName,MS1Scans,MS2Scans,MS3Scans,Median MS2 IT,Median MS3 IT,Median Sum SN,Median Ion Flux,User Defined Carrier Ratio,Calculated Carrier Ratio,Used Calculated Carrier Rato,Suggested SN Cutoff,Suggested Max IT,SN Adjustment,Max IT Adjustment,Slope,yIntercept"); }

                foreach (AnalyzedRun analyzedRun in AnalyzedRuns)
                {
                    //Re-calculate the settings based on the new values
                    analyzedRun.CalculateSuggestedValues(double.Parse(carrierRatioTB.Text), double.Parse(slopeTB.Text), double.Parse(yInterTB.Text), double.Parse(maxITAdjTB.Text));

                    //Write the results to a file
                    writer.WriteLine(analyzedRun.PrintSummary() + "," + snAdjTB.Text + "," + maxITAdjTB.Text + "," + slopeTB.Text + "," + yInterTB.Text);
                }
            }

            tmtFileGrid.Refresh();

            SaveParams();

            UpdateParamFileList();
        }

        private void loadParamButton_Click(object sender, EventArgs e)
        {
            LoadParams();

            PlotRegression();

            //When we update we will want to write out a new summary file
            using (StreamWriter writer = new StreamWriter(Path + "\\SCPCompanionSummary_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv"))
            {
                if (QuantOrder == 2) { writer.WriteLine("RawFileName,MS1Scans,MS2Scans,Median MS2 IT,Median Sum SN,Median Ion Flux,User Defined Carrier Ratio,Calculated Carrier Ratio,Used Calculated Carrier Rato,Suggested SN Cutoff,Suggested Max IT,SN Adjustment,Max IT Adjustment,Slope,yIntercept"); }
                else if (QuantOrder == 3) { writer.WriteLine("RawFileName,MS1Scans,MS2Scans,MS3Scans,Median MS2 IT,Median MS3 IT,Median Sum SN,Median Ion Flux,User Defined Carrier Ratio,Calculated Carrier Ratio,Used Calculated Carrier Rato,Suggested SN Cutoff,Suggested Max IT,SN Adjustment,Max IT Adjustment,Slope,yIntercept"); }

                foreach (AnalyzedRun analyzedRun in AnalyzedRuns)
                {
                    //Re-calculate the settings based on the new values
                    analyzedRun.CalculateSuggestedValues(double.Parse(carrierRatioTB.Text), double.Parse(slopeTB.Text), double.Parse(yInterTB.Text), double.Parse(maxITAdjTB.Text), calculateCarrierRatio: calculateCarrierRatioCB.Checked);

                    //Write the results to a file
                    writer.WriteLine(analyzedRun.PrintSummary() + "," + snAdjTB.Text + "," + maxITAdjTB.Text + "," + slopeTB.Text + "," + yInterTB.Text);
                }
            }

            tmtFileGrid.Refresh();
            multiplexingChannels.Refresh();
        }

        #endregion

        #region Plotting Code
        private void UpdateTMTPlots(int quantOrder, AnalyzedRun run)
        {
            PointPairList tmtIonFlux = null;
            PointPairList tmtSN = null;

            PointPairList tmtQuantIonFlux = null;
            PointPairList tmtQuantSN = null;

            double width = Math.Floor(run.MedianCarrierRatio / 10);
            if(width == 0)
            {
                width = 1;
            }
            PointPairList carrierRatio = Histogram(0, run.MedianCarrierRatio * 5, width, run.CarrierHistogram); ;

            if(quantOrder == 2)
            {
                tmtIonFlux = Histogram(0, 20, 0.5, run.MS2IonFluxs);
                tmtSN = Histogram(0, 10000, 100, run.MS2SumSNs);

                double max = run.MedianMS2QuantIonFlux * 10;
                double multiple = 10;
                while(Math.Floor(run.MedianMS2QuantIonFlux * multiple) == 0)
                {
                    multiple *= 10;
                }
                double bin_width = Math.Floor(run.MedianMS2QuantIonFlux * multiple) / (multiple * 10) * 2;
                tmtQuantIonFlux = Histogram(0, max, bin_width, run.MS2QuantIonFluxs);



                max = run.MedianMS2QuantSN * 10;
                multiple = 10;
                while (Math.Floor(run.MedianMS2QuantSN * multiple) == 0)
                {
                    multiple *= 10;
                }
                bin_width = Math.Floor(run.MedianMS2QuantSN * multiple) / (multiple * 10) * 2;
                tmtQuantSN = Histogram(0, max, bin_width, run.MS2QuantSNs);
            }
            else if (quantOrder == 3)
            {
                tmtIonFlux = Histogram(0, 20, 0.5, run.MS3IonFluxs);
                tmtSN = Histogram(0, 10000, 100, run.MS3SumSNs);

                tmtQuantIonFlux = Histogram(0, 10, 0.1, run.MS3QuantIonFluxs);
                tmtQuantSN = Histogram(0, 1000, 10, run.MS3QuantSNs);
            }
          
            ionFluxPane.CurveList.Clear();
            ionFluxPane.AddBar("Ion Flux", tmtIonFlux, Color.Blue);

            snPane.CurveList.Clear();
            snPane.AddBar("Signal to Noise", tmtSN, Color.Blue);

            quantionFluxPane.CurveList.Clear();
            quantionFluxPane.AddBar("Ion Flux", tmtQuantIonFlux, Color.Blue);

            quantsnPane.CurveList.Clear();
            quantsnPane.AddBar("Signal to Noise", tmtQuantSN, Color.Blue);

            carrierRatioPane.CurveList.Clear();
            carrierRatioPane.AddBar("Carrer Ratio", carrierRatio, Color.Blue);

            snToCarrierPane.CurveList.Clear();
            double[] snArray = run.SNtoCarrierRatio.Keys.ToArray();
            double[] carrierRatios = run.SNtoCarrierRatio.Values.ToArray();
            LineItem scatter = snToCarrierPane.AddCurve("Carrier Ratio", snArray, carrierRatios, Color.Blue);
            scatter.Line.IsVisible = false;
            scatter.Symbol.Fill = new Fill(Color.Blue, Color.FromArgb(200));
            scatter.Symbol.Fill.IsVisible = true;

            tmtIonFluxgraphControl.AxisChange();
            tmtIonFluxgraphControl.Refresh();

            tmtSumSNgraphControl.AxisChange();
            tmtSumSNgraphControl.Refresh();

            tmtQuantIonFluxgraphControl.AxisChange();
            tmtQuantIonFluxgraphControl.Refresh();

            tmtQuantSNgraphControl.AxisChange();
            tmtQuantSNgraphControl.Refresh();

            carrierRatioGraphControl.AxisChange();
            carrierRatioGraphControl.Refresh();

            snToCarrierRatioGraphControl.AxisChange();
            snToCarrierRatioGraphControl.Refresh();

            if(outputPlotDataCB.Checked)
            {
                PrintPlots(run.RawFileName, tmtSN, tmtIonFlux, carrierRatio, run.SNtoCarrierRatio);
            }
        }

        private void PrintPlots(string runName, PointPairList tmtSN, PointPairList tmtIonFlux, PointPairList carrierRatioHist, SortedList<double, double> scatterPlot)
        {
            string plotFolder = Path + "\\PlotData";
            if (!System.IO.Directory.Exists(plotFolder)) { System.IO.Directory.CreateDirectory(plotFolder); }

            using (StreamWriter writer = new StreamWriter(plotFolder + "\\" + runName + "-Plot_SignalToNoise.csv"))
            {
                writer.WriteLine("SN, Count");
                foreach(PointPair pointPair in tmtSN)
                {
                    writer.WriteLine(pointPair.X + "," + pointPair.Y);
                }
            }

            using (StreamWriter writer = new StreamWriter(plotFolder + "\\" + runName + "-Plot_TMTIonFlux.csv"))
            {
                writer.WriteLine("IonFlux, Count");
                foreach (PointPair pointPair in tmtIonFlux)
                {
                    writer.WriteLine(pointPair.X + "," + pointPair.Y);
                }
            }

            using (StreamWriter writer = new StreamWriter(plotFolder + "\\" + runName + "-Plot_CarrierRaioHist.csv"))
            {
                writer.WriteLine("CarrierRatio, Count");
                foreach (PointPair pointPair in carrierRatioHist)
                {
                    writer.WriteLine(pointPair.X + "," + pointPair.Y);
                }
            }

            using (StreamWriter writer = new StreamWriter(plotFolder + "\\" + runName + "-Plot_SNvsCarrierRatio.csv"))
            {
                writer.WriteLine("SN, CarrierRatio");
                foreach (KeyValuePair<double, double> pointPair in scatterPlot)
                {
                    writer.WriteLine(pointPair.Key + "," + pointPair.Value);
                }
            }
        }

        private PointPairList Histogram(double min, double max, double width, List<double> values)
        {
            PointPairList retList = new PointPairList();

            List<double> bins = new List<double>();
            Dictionary<double, int> binDict = new Dictionary<double, int>();
            for (double i = min; i <= max; i += width)
            {
                bins.Add(i);
                binDict.Add(i, 0);
            }

            foreach (double value in values)
            {
                foreach (double bin in bins)
                {
                    if (value >= bin && value <= bin + width)
                    {
                        binDict[bin] += 1;
                        break;
                    }
                }
            }


            foreach (KeyValuePair<double, int> kvp in binDict)
            {
                retList.Add(kvp.Key, kvp.Value);
            }

            return retList;
        }
        #endregion

        #region Regression Code
        private void InitializeRegressionList()
        {
            DataGridViewTextBoxColumn carrierLevelCol = new DataGridViewTextBoxColumn();
            carrierLevelCol.DataPropertyName = "CarrierLevel";
            carrierLevelCol.HeaderText = "Carrier Level";
            carrierLevelCol.Width = 75;
            regressionDGV.Columns.Add(carrierLevelCol);

            DataGridViewTextBoxColumn SNCol = new DataGridViewTextBoxColumn();
            SNCol.DataPropertyName = "SNCutoff";
            SNCol.HeaderText = "Median SN 20% CV";
            SNCol.Width = 75;
            regressionDGV.Columns.Add(SNCol);

            DataGridViewTextBoxColumn suggestSNCol = new DataGridViewTextBoxColumn();
            suggestSNCol.DataPropertyName = "SuggestedSNCutoff";
            suggestSNCol.HeaderText = "Suggested SN";
            suggestSNCol.Width = 80;
            suggestSNCol.ReadOnly = true;
            regressionDGV.Columns.Add(suggestSNCol);

            if(File.Exists(ParamPath + "\\defaultParams.csv"))
            {
                LoadParams();
            }
            else
            {
                double snAdjustment = double.Parse(snAdjTB.Text);
                RegressionPoint point1 = new RegressionPoint(5, 135, snAdjustment);
                RegressionPoint point2 = new RegressionPoint(20, 250, snAdjustment);
                RegressionPoint point3 = new RegressionPoint(75, 600, snAdjustment);
                RegressionPoint point4 = new RegressionPoint(300, 3000, snAdjustment);
                RegressionPoint point5 = new RegressionPoint(900, 8000, snAdjustment);

                RegressionList.Add(point1);
                RegressionList.Add(point2);
                RegressionList.Add(point3);
                RegressionList.Add(point4);
                RegressionList.Add(point5);

                SaveParams();
            }
        }

        private void InitializeRegressionPlot()
        {
            regressionPane = regressionGC.GraphPane;
            regressionPane.Title.Text = "Carrier Proteome vs. Suggested s/n";
            regressionPane.Title.FontSpec.Size = 25f;
            regressionPane.XAxis.Title.Text = "carrier proteome level";
            regressionPane.XAxis.Title.FontSpec.Size = 20f;
            regressionPane.YAxis.Title.Text = "suggested s/n cutoff";
            regressionPane.YAxis.Title.FontSpec.Size = 20f;
        }

        private void PlotRegression()
        {
            List<double> xList = new List<double>();
            List<double> yList = new List<double>();

            PointPairList points = new PointPairList();
            foreach (RegressionPoint point in RegressionList)
            {
                points.Add(new PointPair(double.Parse(point.CarrierLevel), double.Parse(point.SuggestedSNCutoff)));
                xList.Add(double.Parse(point.CarrierLevel));
                yList.Add(double.Parse(point.SuggestedSNCutoff));
            }

            double[] xdata = xList.ToArray();
            double[] ydata = yList.ToArray();

            Tuple<double, double> output = Fit.Line(xdata, ydata);
            double intercept = output.Item1;
            double slope = output.Item2;

            slopeTB.Text = Math.Round(slope, 2).ToString();
            yInterTB.Text = Math.Round(intercept, 2).ToString();

            regressionPane.CurveList.Clear();
            LineItem myCurve = regressionPane.AddCurve("", points, Color.Black, SymbolType.Circle);
            myCurve.Line.IsVisible = true;
            regressionGC.AxisChange();
            regressionGC.Refresh();
        }

        private void SaveParams()
        {
            using (StreamWriter writer = new StreamWriter(ParamPath + "\\" + paramNameTB.Text + ".csv"))
            {
                writer.WriteLine("Type,Val1,Val2");

                writer.WriteLine("CarrierLevel," + carrierRatioTB.Text);
                writer.WriteLine("SNAdjustment," + snAdjTB.Text);
                writer.WriteLine("MaxITAdjustment," + maxITAdjTB.Text);

                foreach (DataGridViewRow row in regressionDGV.Rows)
                {
                    double carrierLevel = double.Parse(row.Cells[0].Value.ToString());
                    double snValue = double.Parse(row.Cells[1].Value.ToString());

                    writer.WriteLine("RegressionPoint," + carrierLevel + "," + snValue);
                }

                foreach(MultiplexingLine line in MultiplexingLines)
                {
                    writer.WriteLine("MultiplexingChannel," + line.Name + "," + line.Type);
                }

                writer.WriteLine("CalculateCarrierBool," + calculateCarrierRatioCB.Checked.ToString());

                writer.WriteLine("CalculateCarrierType," + carrierRatioCutoffTypeComboBox.Text);

                writer.WriteLine("CalculateCarrierValue," + carrierRatioCutoff.Text);
            }
        }

        private void LoadParams()
        {
            RegressionList.Clear();

            string file = null;
            if(ParamsFiles.Count == 0)
            {
                file = ParamPath + "\\" + paramNameTB.Text + ".csv";
            }
            else
            {
                file = ParamsFiles[paramFileListBox.SelectedIndex];

                paramNameTB.Text = System.IO.Path.GetFileNameWithoutExtension(file);
            }

            using (CsvReader reader = new CsvReader(new StreamReader(file), true))
            {
                Dictionary<double, double> regressionPoints = new Dictionary<double, double>();
                while(reader.ReadNextRecord())
                {
                    if(reader["Type"] == "RegressionPoint")
                    {
                        double carrierLevel = double.Parse(reader["Val1"]);
                        double sn = double.Parse(reader["Val2"]);

                        regressionPoints.Add(carrierLevel, sn);
                    }
                    else if(reader["Type"] == "MaxITAdjustment")
                    {
                        maxITAdjTB.Text = reader["Val1"];
                    }
                    else if (reader["Type"] == "SNAdjustment")
                    {
                        snAdjTB.Text = reader["Val1"];
                    }
                    else if (reader["Type"] == "CarrierLevel")
                    {
                        carrierRatioTB.Text = reader["Val1"];
                    }
                    else if (reader["Type"] == "MultiplexingChannel")
                    {
                        if (MultiplexingLines != null)
                        {
                            foreach (MultiplexingLine line in MultiplexingLines)
                            {
                                if(line.Name == reader["Val1"])
                                {
                                    line.Type = reader["Val2"];
                                }
                            }
                        }
                    }
                    else if(reader["Type"] == "CalculateCarrierBool")
                    {
                        calculateCarrierRatioCB.Checked = bool.Parse(reader["Val1"]);
                    }
                    else if (reader["Type"] == "CalculateCarrierType")
                    {
                        carrierRatioCutoffTypeComboBox.Text = reader["Val1"];
                    }
                    else if (reader["Type"] == "CalculateCarrierValue")
                    {
                        carrierRatioCutoff.Text = reader["Val1"];
                    }
                }

                foreach(KeyValuePair<double, double> kvp in regressionPoints)
                {
                    RegressionList.Add(new RegressionPoint(kvp.Key, kvp.Value, double.Parse(snAdjTB.Text)));
                }
            }

            regressionDGV.Refresh();
        }
        #endregion

        #region Program Intializations
        private void InitializeSNPlot()
        {
            snPane = tmtSumSNgraphControl.GraphPane;
            snPane.Title.Text = "Sum s/n";
            snPane.Title.FontSpec.Size = 25f;
            snPane.XAxis.Title.Text = "sum s/n";
            snPane.XAxis.Title.FontSpec.Size = 20f;
            snPane.YAxis.Title.Text = "count";
            snPane.YAxis.Title.FontSpec.Size = 20f;

            quantsnPane = tmtQuantSNgraphControl.GraphPane;
            quantsnPane.Title.Text = "Quant s/n";
            quantsnPane.Title.FontSpec.Size = 25f;
            quantsnPane.XAxis.Title.Text = "quant s/n";
            quantsnPane.XAxis.Title.FontSpec.Size = 20f;
            quantsnPane.YAxis.Title.Text = "count";
            quantsnPane.YAxis.Title.FontSpec.Size = 20f;
        }

        private void InitializeIonFluxPlot()
        {
            ionFluxPane = tmtIonFluxgraphControl.GraphPane;
            ionFluxPane.Title.Text = "Ion Flux";
            ionFluxPane.Title.FontSpec.Size = 25f;
            ionFluxPane.XAxis.Title.Text = "ion flux (s/n per ms)";
            ionFluxPane.XAxis.Title.FontSpec.Size = 20f;
            ionFluxPane.YAxis.Title.Text = "count";
            ionFluxPane.YAxis.Title.FontSpec.Size = 20f;

            quantionFluxPane = tmtQuantIonFluxgraphControl.GraphPane;
            quantionFluxPane.Title.Text = "Quant Ion Flux";
            quantionFluxPane.Title.FontSpec.Size = 25f;
            quantionFluxPane.XAxis.Title.Text = "quant ion flux (quant s/n per ms)";
            quantionFluxPane.XAxis.Title.FontSpec.Size = 20f;
            quantionFluxPane.YAxis.Title.Text = "count";
            quantionFluxPane.YAxis.Title.FontSpec.Size = 20f;
        }

        private void InitializeSNtoCarrierRatioPlot()
        {
            snToCarrierPane = snToCarrierRatioGraphControl.GraphPane;
            snToCarrierPane.Title.Text = "s/n to carrier ratio";
            snToCarrierPane.Title.FontSpec.Size = 25f;
            snToCarrierPane.XAxis.Title.Text = "sum s/n";
            snToCarrierPane.XAxis.Title.FontSpec.Size = 20f;
            snToCarrierPane.YAxis.Title.Text = "carrier ratio";
            snToCarrierPane.YAxis.Title.FontSpec.Size = 20f;
        }

        private void InitializeCarrierRaioPlot()
        {
            carrierRatioPane = carrierRatioGraphControl.GraphPane;
            carrierRatioPane.Title.Text = "carrier ratio histogram";
            carrierRatioPane.Title.FontSpec.Size = 25f;
            carrierRatioPane.XAxis.Title.Text = "carrier ratio";
            carrierRatioPane.XAxis.Title.FontSpec.Size = 20f;
            carrierRatioPane.YAxis.Title.Text = "count";
            carrierRatioPane.YAxis.Title.FontSpec.Size = 20f;
        }

        private void InitializeMS2RunGrid()
        {
            tmtFileGrid.DataSource = AnalyzedRuns;
            RunGridInitialized = true;

            tmtFileGrid.AutoGenerateColumns = false;

            tmtFileGrid.Columns.Clear();
            DataGridViewTextBoxColumn rawNameCol = new DataGridViewTextBoxColumn();
            rawNameCol.DataPropertyName = "RawFileName";
            rawNameCol.HeaderText = "Raw File";
            rawNameCol.ReadOnly = true;
            rawNameCol.Width = 600;
            tmtFileGrid.Columns.Add(rawNameCol);

            DataGridViewTextBoxColumn methMaxITCol = new DataGridViewTextBoxColumn();
            methMaxITCol.DataPropertyName = "MaxInjectionTime";
            methMaxITCol.HeaderText = "Method Max IT";
            methMaxITCol.ReadOnly = true;
            methMaxITCol.Width = 150;
            tmtFileGrid.Columns.Add(methMaxITCol);

            DataGridViewTextBoxColumn ms2ITCol = new DataGridViewTextBoxColumn();
            ms2ITCol.DataPropertyName = "MedianMS2IT";
            ms2ITCol.HeaderText = "Median MS2 IT";
            ms2ITCol.ReadOnly = true;
            ms2ITCol.Width = 150;
            tmtFileGrid.Columns.Add(ms2ITCol);

            DataGridViewTextBoxColumn ms2SNCol = new DataGridViewTextBoxColumn();
            ms2SNCol.DataPropertyName = "MedianMS2QuantSN";
            ms2SNCol.HeaderText = "Median Quant SN";
            ms2SNCol.ReadOnly = true;
            ms2SNCol.Width = 150;
            tmtFileGrid.Columns.Add(ms2SNCol);

            DataGridViewTextBoxColumn ms2IonFluxCol = new DataGridViewTextBoxColumn();
            ms2IonFluxCol.DataPropertyName = "MedianMS2QuantIonFlux";
            ms2IonFluxCol.HeaderText = "Median Quant Ion Flux";
            ms2IonFluxCol.ReadOnly = true;
            ms2IonFluxCol.Width = 150;
            tmtFileGrid.Columns.Add(ms2IonFluxCol);

            DataGridViewTextBoxColumn ms2SuggSNCutoffCol = new DataGridViewTextBoxColumn();
            ms2SuggSNCutoffCol.DataPropertyName = "SuggestedQuantSNCutoff";
            ms2SuggSNCutoffCol.HeaderText = "Suggested Quant SN Cutoff";
            ms2SuggSNCutoffCol.ReadOnly = true;
            ms2SuggSNCutoffCol.Width = 180;
            tmtFileGrid.Columns.Add(ms2SuggSNCutoffCol);

            DataGridViewTextBoxColumn ms2SuggMaxITCol = new DataGridViewTextBoxColumn();
            ms2SuggMaxITCol.DataPropertyName = "SuggestedQuantMaxIT";
            ms2SuggMaxITCol.HeaderText = "Suggested Max IT";
            ms2SuggMaxITCol.ReadOnly = true;
            ms2SuggMaxITCol.Width = 150;
            tmtFileGrid.Columns.Add(ms2SuggMaxITCol);

            DataGridViewTextBoxColumn quantpercentCol = new DataGridViewTextBoxColumn();
            quantpercentCol.DataPropertyName = "QuatSNQuantPercent";
            quantpercentCol.HeaderText = "Percent > Cutoff";
            quantpercentCol.ReadOnly = true;
            quantpercentCol.Width = 150;
            tmtFileGrid.Columns.Add(quantpercentCol);

            ////////////////////////////////////////////////////////////////////////////
            carrierEstimateGrid.DataSource = AnalyzedRuns;
            carrierEstimateGrid.AutoGenerateColumns = false;

            carrierEstimateGrid.Columns.Clear();
            rawNameCol = new DataGridViewTextBoxColumn();
            rawNameCol.DataPropertyName = "RawFileName";
            rawNameCol.HeaderText = "Raw File";
            rawNameCol.ReadOnly = true;
            rawNameCol.Width = 600;
            carrierEstimateGrid.Columns.Add(rawNameCol);

            ms2SNCol = new DataGridViewTextBoxColumn();
            ms2SNCol.DataPropertyName = "MedianMS2SN";
            ms2SNCol.HeaderText = "Median Sum SN";
            ms2SNCol.ReadOnly = true;
            ms2SNCol.Width = 120;
            carrierEstimateGrid.Columns.Add(ms2SNCol);

            ms2IonFluxCol = new DataGridViewTextBoxColumn();
            ms2IonFluxCol.DataPropertyName = "MedianMS2IonFlux";
            ms2IonFluxCol.HeaderText = "Median Ion Flux";
            ms2IonFluxCol.ReadOnly = true;
            ms2IonFluxCol.Width = 120;
            carrierEstimateGrid.Columns.Add(ms2IonFluxCol);

            //DataGridViewTextBoxColumn userCarrierRatio = new DataGridViewTextBoxColumn();
            //userCarrierRatio.DataPropertyName = "UserDefinedCarrierRatio";
            //userCarrierRatio.HeaderText = "User Carrier Ratio";
            //userCarrierRatio.ReadOnly = true;
            //userCarrierRatio.Width = 120;
            //carrierEstimateGrid.Columns.Add(userCarrierRatio);

            DataGridViewTextBoxColumn calculatedCarrierRatio = new DataGridViewTextBoxColumn();
            calculatedCarrierRatio.DataPropertyName = "CalculatedCarrierRatio";
            calculatedCarrierRatio.HeaderText = "Calc. Carrier Ratio";
            calculatedCarrierRatio.ReadOnly = true;
            calculatedCarrierRatio.Width = 120;
            carrierEstimateGrid.Columns.Add(calculatedCarrierRatio);

            //DataGridViewTextBoxColumn useCalcRatio = new DataGridViewTextBoxColumn();
            //useCalcRatio.DataPropertyName = "UseCalculatedRatio";
            //useCalcRatio.HeaderText = "Use Cal. Carrier Ratio";
            //useCalcRatio.ReadOnly = true;
            //useCalcRatio.Width = 120;
            //carrierEstimateGrid.Columns.Add(useCalcRatio);

            //ms2SuggSNCutoffCol = new DataGridViewTextBoxColumn();
            //ms2SuggSNCutoffCol.DataPropertyName = "SuggestedSNCutoff";
            //ms2SuggSNCutoffCol.HeaderText = "Suggested Total SN Cutoff";
            //ms2SuggSNCutoffCol.ReadOnly = true;
            //ms2SuggSNCutoffCol.Width = 130;
            //carrierEstimateGrid.Columns.Add(ms2SuggSNCutoffCol);

            //ms2SuggMaxITCol = new DataGridViewTextBoxColumn();
            //ms2SuggMaxITCol.DataPropertyName = "SuggestedMaxIT";
            //ms2SuggMaxITCol.HeaderText = "Suggested Max IT";
            //ms2SuggMaxITCol.ReadOnly = true;
            //ms2SuggMaxITCol.Width = 120;
            //carrierEstimateGrid.Columns.Add(ms2SuggMaxITCol);

            //quantpercentCol = new DataGridViewTextBoxColumn();
            //quantpercentCol.DataPropertyName = "QuantPercent";
            //quantpercentCol.HeaderText = "Percent > Cutoff";
            //quantpercentCol.ReadOnly = true;
            //quantpercentCol.Width = 110;
            //carrierEstimateGrid.Columns.Add(quantpercentCol);
        }

        private void InitializeMS3RunGrid()
        {
            tmtFileGrid.DataSource = AnalyzedRuns;
            RunGridInitialized = true;

            tmtFileGrid.AutoGenerateColumns = true;

            tmtFileGrid.Columns.Clear();
            DataGridViewTextBoxColumn rawNameCol = new DataGridViewTextBoxColumn();
            rawNameCol.DataPropertyName = "RawFileName";
            rawNameCol.HeaderText = "Raw File";
            rawNameCol.ReadOnly = true;
            rawNameCol.Width = 600;
            tmtFileGrid.Columns.Add(rawNameCol);

            DataGridViewTextBoxColumn methMaxITCol = new DataGridViewTextBoxColumn();
            methMaxITCol.DataPropertyName = "MaxInjectionTime";
            methMaxITCol.HeaderText = "Method Max IT";
            methMaxITCol.ReadOnly = true;
            methMaxITCol.Width = 150;
            tmtFileGrid.Columns.Add(methMaxITCol);

            DataGridViewTextBoxColumn ms2ITCol = new DataGridViewTextBoxColumn();
            ms2ITCol.DataPropertyName = "MedianMS2IT";
            ms2ITCol.HeaderText = "Median MS2 IT";
            ms2ITCol.ReadOnly = true;
            ms2ITCol.Width = 150;
            tmtFileGrid.Columns.Add(ms2ITCol);

            DataGridViewTextBoxColumn ms3ITCol = new DataGridViewTextBoxColumn();
            ms3ITCol.DataPropertyName = "MedianMS3IT";
            ms3ITCol.HeaderText = "Median MS3 IT";
            ms3ITCol.ReadOnly = true;
            ms3ITCol.Width = 120;
            tmtFileGrid.Columns.Add(ms3ITCol);

            DataGridViewTextBoxColumn ms2SNCol = new DataGridViewTextBoxColumn();
            ms2SNCol.DataPropertyName = "MedianMS3QuantSN";
            ms2SNCol.HeaderText = "Median Quant SN";
            ms2SNCol.ReadOnly = true;
            ms2SNCol.Width = 150;
            tmtFileGrid.Columns.Add(ms2SNCol);

            DataGridViewTextBoxColumn ms2IonFluxCol = new DataGridViewTextBoxColumn();
            ms2IonFluxCol.DataPropertyName = "MedianMS3QuantIonFlux";
            ms2IonFluxCol.HeaderText = "Median Quant Ion Flux";
            ms2IonFluxCol.ReadOnly = true;
            ms2IonFluxCol.Width = 150;
            tmtFileGrid.Columns.Add(ms2IonFluxCol);

            DataGridViewTextBoxColumn ms2SuggSNCutoffCol = new DataGridViewTextBoxColumn();
            ms2SuggSNCutoffCol.DataPropertyName = "SuggestedQuantSNCutoff";
            ms2SuggSNCutoffCol.HeaderText = "Suggested Quant SN Cutoff";
            ms2SuggSNCutoffCol.ReadOnly = true;
            ms2SuggSNCutoffCol.Width = 180;
            tmtFileGrid.Columns.Add(ms2SuggSNCutoffCol);

            DataGridViewTextBoxColumn ms2SuggMaxITCol = new DataGridViewTextBoxColumn();
            ms2SuggMaxITCol.DataPropertyName = "SuggestedQuantMaxIT";
            ms2SuggMaxITCol.HeaderText = "Suggested Max IT";
            ms2SuggMaxITCol.ReadOnly = true;
            ms2SuggMaxITCol.Width = 150;
            tmtFileGrid.Columns.Add(ms2SuggMaxITCol);

            DataGridViewTextBoxColumn quantpercentCol = new DataGridViewTextBoxColumn();
            quantpercentCol.DataPropertyName = "QuatSNQuantPercent";
            quantpercentCol.HeaderText = "Percent > Cutoff";
            quantpercentCol.ReadOnly = true;
            quantpercentCol.Width = 150;
            tmtFileGrid.Columns.Add(quantpercentCol);

            ///////////////////////////////////////
            carrierEstimateGrid.DataSource = AnalyzedRuns;
            carrierEstimateGrid.AutoGenerateColumns = false;

            carrierEstimateGrid.Columns.Clear();
            rawNameCol = new DataGridViewTextBoxColumn();
            rawNameCol.DataPropertyName = "RawFileName";
            rawNameCol.HeaderText = "Raw File";
            rawNameCol.ReadOnly = true;
            rawNameCol.Width = 600;
            carrierEstimateGrid.Columns.Add(rawNameCol);

            ms2SNCol = new DataGridViewTextBoxColumn();
            ms2SNCol.DataPropertyName = "MedianMS3SN";
            ms2SNCol.HeaderText = "Median Sum SN";
            ms2SNCol.ReadOnly = true;
            ms2SNCol.Width = 120;
            carrierEstimateGrid.Columns.Add(ms2SNCol);

            ms2IonFluxCol = new DataGridViewTextBoxColumn();
            ms2IonFluxCol.DataPropertyName = "MedianMS3IonFlux";
            ms2IonFluxCol.HeaderText = "Median Ion Flux";
            ms2IonFluxCol.ReadOnly = true;
            ms2IonFluxCol.Width = 120;
            carrierEstimateGrid.Columns.Add(ms2IonFluxCol);

            DataGridViewTextBoxColumn calculatedCarrierRatio = new DataGridViewTextBoxColumn();
            calculatedCarrierRatio.DataPropertyName = "CalculatedCarrierRatio";
            calculatedCarrierRatio.HeaderText = "Calc. Carrier Ratio";
            calculatedCarrierRatio.ReadOnly = true;
            calculatedCarrierRatio.Width = 120;
            carrierEstimateGrid.Columns.Add(calculatedCarrierRatio);
        }

        private List<Modification> InitializeTMT()
        {
            List<Modification> tmt = new List<Modification>();

            Modification tmt126 = new Modification(monoMass: 126.127726, name: "TMT126", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt127n = new Modification(monoMass: 127.124761, name: "TMT127n", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt127c = new Modification(monoMass: 127.131081, name: "TMT127c", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt128n = new Modification(monoMass: 128.128116, name: "TMT128n", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt128c = new Modification(monoMass: 128.134436, name: "TMT128c", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt129n = new Modification(monoMass: 129.131471, name: "TMT129n", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt129c = new Modification(monoMass: 129.13779, name: "TMT129c", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt130n = new Modification(monoMass: 130.134825, name: "TMT130n", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt130c = new Modification(monoMass: 130.141145, name: "TMT130c", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt131n = new Modification(monoMass: 131.13818, name: "TMT131n", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt131c = new Modification(monoMass: 131.1445, name: "TMT131c", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt132n = new Modification(monoMass: 132.141535, name: "TMT132n", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt132c = new Modification(monoMass: 132.147855, name: "TMT132c", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt133n = new Modification(monoMass: 133.14489, name: "TMT133n", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt133c = new Modification(monoMass: 133.15121, name: "TMT133c", sites: ModificationSites.K | ModificationSites.NPep);
            Modification tmt134n = new Modification(monoMass: 134.148245, name: "TMT134n", sites: ModificationSites.K | ModificationSites.NPep);

            tmt.Add(tmt126);
            tmt.Add(tmt127n);
            tmt.Add(tmt127c);
            tmt.Add(tmt128n);
            tmt.Add(tmt128c);
            tmt.Add(tmt129n);
            tmt.Add(tmt129c);
            tmt.Add(tmt130n);
            tmt.Add(tmt130c);
            tmt.Add(tmt131n);
            tmt.Add(tmt131c);
            tmt.Add(tmt132n);
            tmt.Add(tmt132c);
            tmt.Add(tmt133n);
            tmt.Add(tmt133c);
            tmt.Add(tmt134n);

            TMTIons = tmt;

            MultiplexingLines.Add(new MultiplexingLine(tmt126));
            MultiplexingLines.Add(new MultiplexingLine(tmt127n));
            MultiplexingLines.Add(new MultiplexingLine(tmt127c));
            MultiplexingLines.Add(new MultiplexingLine(tmt128n));
            MultiplexingLines.Add(new MultiplexingLine(tmt128c));
            MultiplexingLines.Add(new MultiplexingLine(tmt129n));
            MultiplexingLines.Add(new MultiplexingLine(tmt129c));
            MultiplexingLines.Add(new MultiplexingLine(tmt130n));
            MultiplexingLines.Add(new MultiplexingLine(tmt130c));
            MultiplexingLines.Add(new MultiplexingLine(tmt131n));
            MultiplexingLines.Add(new MultiplexingLine(tmt131c));
            MultiplexingLines.Add(new MultiplexingLine(tmt132n));
            MultiplexingLines.Add(new MultiplexingLine(tmt132c));
            MultiplexingLines.Add(new MultiplexingLine(tmt133n));
            MultiplexingLines.Add(new MultiplexingLine(tmt133c));
            MultiplexingLines.Add(new MultiplexingLine(tmt134n));

            return tmt;
        }
        #endregion

        #region Input Code
        private void AddFiles(List<string> files)
        {
            foreach (string file in files)
            {
                InputFiles.Add(file);
            }
        }

        private void inputFileBox_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                AddFiles(files.ToList());
            }
        }

        private void inputFileBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }
        #endregion

        #region Unused
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tmtSumSNgraphControl_Load(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
