using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using System.IO;

using CSMSL;
using CSMSL.Analysis;
using CSMSL.Chemistry;
using CSMSL.IO;
using CSMSL.Proteomics;
using CSMSL.Spectral;
using CSMSL.Util;


namespace SCPCompanion
{
    public class AnalyzedRun
    {
        public int QuantOrder { get; set; }
        public string RawFileName { get; set; }

        int NumMS1Scans { get; set; }
        int NumMS2Scans { get; set; }
        int NumMS3Scans { get; set; }

        public List<double> MS2ITs { get; set; }
        public List<double> MS3ITs { get; set; }
        public List<double> MS2IonFluxs { get; set; }
        public List<double> MS2SumSNs { get; set; }
        public List<double> MS3IonFluxs { get; set; }
        public List<double> MS3SumSNs { get; set; }
        public List<double> CarrierHistogram { get; set; }

        //New Params - Start
        public List<double> MS2QuantIonFluxs { get; set; }
        public List<double> MS2QuantSNs { get; set; }
        public List<double> MS3QuantIonFluxs { get; set; }
        public List<double> MS3QuantSNs { get; set; }
        public Dictionary<string, double> QuantSNDict { get; set; }
        public double MedianMS2QuantIonFlux { get; set; }
        public double MedianMS2QuantSN { get; set; }
        public double MedianMS3QuantIonFlux { get; set; }
        public double MedianMS3QuantSN { get; set; }
        public double NumberOfSamples { get; set; }
        public double SNperChannel { get; set; }
        public double QuantSNQuantScans { get; set; }
        public double QuatSNQuantPercent { get; set; }
        public List<double> MS2QuantSuggMaxIT { get; set; }
        public double MaxInjectionTime { get; set; }
        //New Params - End

        public SortedList<double, double> SNtoCarrierRatio { get; set; }
        public Dictionary<string, double> SumSNDict { get; set; }

        public double MedianMS2IT { get; set; }
        public double MedianMS3IT { get; set; }
        public double MedianMS2IonFlux { get; set; }
        public double MedianMS2SN { get; set; }
        public double MedianMS3IonFlux { get; set; }
        public double MedianMS3SN { get; set; }
        public double MedianCarrierRatio { get; set; }

        public double TotalScans { get; set; }
        public double QuantScans { get; set; }
        public double QuantPercent { get; set; }

        public double CalculatedCarrierRatio { get; set; }
        public double UserDefinedCarrierRatio { get; set; }

        public bool UseCalculatedRatio { get; set; }

        public int SuggestedSNCutoff { get; set; }
        public int SuggestedMaxIT { get; set; }

        public int SuggestedQuantSNCutoff { get; set; }
        public int SuggestedQuantMaxIT { get; set; }

        public int SuggQuantMaxITFromHist { get; set; }

        private List<MSnScan> MSnScans { get; set; }

        public AnalyzedRun(int quantOrder, string rawfileName, int MS1scanNum, int MS2ScanNum, int MS3ScanNum, List<MSnScan> msnScans, List<MultiplexingLine> multiplexinglines, string carrierRatioCutoffType, string carrierRatioCutoffValue, string numberOfBlanks, string carrierRatioHistCutoff, string userSNperChannel)
        {
            QuantOrder = quantOrder;

            //RawFile Name
            RawFileName = Path.GetFileNameWithoutExtension(rawfileName);

            //Add Values
            NumMS1Scans = MS1scanNum;
            NumMS2Scans = MS2ScanNum;
            NumMS3Scans = MS3ScanNum;
            SNperChannel = double.Parse(userSNperChannel);

            //Initialize the lists
            MS2ITs = new List<double>();
            MS3ITs = new List<double>();
            MS2IonFluxs = new List<double>();
            MS2SumSNs = new List<double>();
            MS3IonFluxs = new List<double>();
            MS3SumSNs = new List<double>();
            SumSNDict = new Dictionary<string, double>();
            SNtoCarrierRatio = new SortedList<double, double>();
            CarrierHistogram = new List<double>();
            MS2QuantIonFluxs = new List<double>();
            MS2QuantSNs = new List<double>();
            MS3QuantIonFluxs = new List<double>();
            MS3QuantSNs = new List<double>();
            QuantSNDict = new Dictionary<string, double>();
            MS2QuantSuggMaxIT = new List<double>();

            MSnScans = new List<MSnScan>();

            //Initialize Variables
            MedianMS2IT = -1;
            MedianMS3IT = -1;
            MedianMS2IonFlux = -1;
            MedianMS2SN = -1;
            MedianMS3IonFlux = -1;
            MedianMS3SN = -1;
            MedianCarrierRatio = -1;
            UserDefinedCarrierRatio = -1;
            CalculatedCarrierRatio = -1;
            MedianMS2QuantIonFlux = -1;
            MedianMS2QuantSN = -1;
            MedianMS3QuantIonFlux = -1;
            MedianMS3QuantSN = -1;
            NumberOfSamples = -1;
            SuggestedQuantSNCutoff = -1;
            SuggestedQuantMaxIT = -1;
            SuggQuantMaxITFromHist = -1;
            MaxInjectionTime = - 1;

            SuggestedSNCutoff = -1;
            SuggestedMaxIT = -1;

            TotalScans = -1;
            QuantScans = -1;
            QuantPercent = -1;
            QuantSNQuantScans = -1;
            QuatSNQuantPercent = -1;

            UseCalculatedRatio = true;


            SortedDictionary<double, int> maxITFinder = new SortedDictionary<double, int>();

            //Cycle through each of the scans in order to extract the data
            foreach (MSnScan scan in msnScans)
            {
                int msOrder = scan.Order;
                foreach (KeyValuePair<string, double> kvp in scan.SNDict)
                {
                    double l_outDoub = 0;
                    if (SumSNDict.TryGetValue(kvp.Key, out l_outDoub))
                    {
                        SumSNDict[kvp.Key] += kvp.Value;
                    }
                    else
                    {
                        SumSNDict.Add(kvp.Key, kvp.Value);
                    }
                }

                double numOfSamples = -1;
                double quantSN = -1;
                double carrierRatio = CalculateCarrierRatio(scan.SNDict, multiplexinglines, numberOfBlanks, out numOfSamples, out quantSN);

                NumberOfSamples = numOfSamples;

                double injectionTime = scan.InjectionTime;
                int itCount = 0;
                if(!maxITFinder.TryGetValue(injectionTime, out itCount))
                {
                    maxITFinder.Add(injectionTime, 1);
                }
                else
                {
                    maxITFinder[injectionTime]++;
                }


                double ionFlux = scan.SumSN / injectionTime;
                double quantIonFlux = quantSN / injectionTime;

                scan.QuantSN = quantSN;
                scan.QuantIonFlux = quantIonFlux;
                scan.CarrierRatio = carrierRatio;

                if (msOrder == 2)
                {
                    MS2ITs.Add(injectionTime);
                    MS2SumSNs.Add(scan.SumSN);
                }
                else if (msOrder == 3)
                {
                    MS3ITs.Add(injectionTime);
                    MS3SumSNs.Add(scan.SumSN);
                }

                double l_sumsN = scan.SumSN;
                double outDoub = 0;
                while(SNtoCarrierRatio.TryGetValue(l_sumsN, out outDoub))
                {
                    l_sumsN += 0.00001;
                }

                SNtoCarrierRatio.Add(l_sumsN, carrierRatio);

                //Add the scan to the list of all the scans
                MSnScans.Add(scan);
            }

            MaxInjectionTime = maxITFinder.ElementAt(maxITFinder.Count - 1).Key;
            //Cycle through each of the scans in order to extract the data
            foreach (MSnScan scan in msnScans)
            {
                double maxITPercent = scan.InjectionTime / MaxInjectionTime;
                scan.MaxITPercent = maxITPercent;

                if (scan.Order == 2)
                {
                    if (scan.IonFlux > 0 && maxITPercent > 0.5 && maxITPercent <= 1)
                    {
                        MS2IonFluxs.Add(scan.IonFlux);
                    }


                    if (scan.QuantIonFlux > 0 && maxITPercent > 0.5 && maxITPercent <= 1)
                    {
                        MS2QuantIonFluxs.Add(scan.QuantIonFlux);
                    }                
                    
                    if(scan.QuantSN > 0)
                    {
                        MS2QuantSNs.Add(scan.QuantSN);
                    }
                }
                else if (scan.Order == 3)
                {
                    if (scan.IonFlux > 0 && maxITPercent > 0.5 && maxITPercent <= 1)
                    {
                        MS3IonFluxs.Add(scan.IonFlux);
                    }


                    if (scan.QuantIonFlux > 0 && maxITPercent > 0.5 && maxITPercent <= 1)
                    {
                        MS3QuantIonFluxs.Add(scan.QuantIonFlux);
                    }
                        
                    if (scan.QuantSN > 0)
                    {
                        MS3QuantSNs.Add(scan.QuantSN);
                    }
                }
            }

            //Calculate the medians
            MedianMS2IT = Math.Round(Median(MS2ITs), 2);
            MedianMS3IT = Math.Round(Median(MS3ITs), 2);
            MedianMS2IonFlux = Math.Round(Median(MS2IonFluxs), 2);
            MedianMS2SN = Math.Round(Median(MS2SumSNs), 2);
            MedianMS3IonFlux = Math.Round(Median(MS3IonFluxs), 2);
            MedianMS3SN = Math.Round(Median(MS3SumSNs), 2);

            MedianMS2QuantIonFlux = Math.Round(Median(MS2QuantIonFluxs), 3);
            MedianMS2QuantSN = Math.Round(Median(MS2QuantSNs), 2);
            MedianMS3QuantIonFlux = Math.Round(Median(MS3QuantIonFluxs), 3);
            MedianMS3QuantSN = Math.Round(Median(MS3QuantSNs), 2);

            if (carrierRatioCutoffType=="Percent")
            {
                MedianCarrierRatio = Math.Round(MedianWithoutNAPercent(SNtoCarrierRatio, carrierRatioHistCutoff, percent: double.Parse(carrierRatioCutoffValue)), 2);
            }
            else if(carrierRatioCutoffType=="Signal to Noise")
            {
                MedianCarrierRatio = Math.Round(MedianWithoutNA(SNtoCarrierRatio, carrierRatioHistCutoff, minSN: double.Parse(carrierRatioCutoffValue)), 2);
            }

            CalculatedCarrierRatio = MedianCarrierRatio;
        }

        private double CalculateCarrierRatio(Dictionary<string, double> snDict, List<MultiplexingLine> multiplexinglines, string numberOfBlanks, out double numOfSamples, out double quantSN)
        {
            double carrierRatio = -1;

            Dictionary<string, List<string>> channelTypeToName = new Dictionary<string, List<string>>();
            foreach(MultiplexingLine line in multiplexinglines)
            {
                List<string> outList = null;
                if(channelTypeToName.TryGetValue(line.Type, out outList))
                {
                    channelTypeToName[line.Type].Add(line.Name);
                }
                else
                {
                    channelTypeToName.Add(line.Type, new List<string>());
                    channelTypeToName[line.Type].Add(line.Name);
                }
            }

            Dictionary<string, double> sumSNByGroup = new Dictionary<string, double>();
            Dictionary<string, List<double>> sumSNCarrierVsSample = new Dictionary<string, List<double>>();
            sumSNCarrierVsSample.Add("Carrier", new List<double>());
            sumSNCarrierVsSample.Add("Sample", new List<double>());

            foreach (KeyValuePair<string, double> kvp in snDict)
            {
                double outDoub = 0;
                if (sumSNByGroup.TryGetValue(kvp.Key, out outDoub))
                {
                    sumSNByGroup[kvp.Key] += kvp.Value;
                }
                else
                {
                    sumSNByGroup.Add(kvp.Key, kvp.Value);
                }
            }

            foreach(KeyValuePair<string, List<string>> kvp2 in channelTypeToName)
            {
                if (kvp2.Key.Contains("Carrier"))
                {
                    foreach (string channelName in kvp2.Value)
                    {
                        double outDoub = 0;
                        if (snDict.TryGetValue(channelName, out outDoub))
                        {
                            sumSNCarrierVsSample["Carrier"].Add(outDoub);
                        }
                    }
                }
                else if (kvp2.Key.Contains("Sample"))
                {
                    foreach (string channelName in kvp2.Value)
                    {
                        double outDoub = 0;
                        if (snDict.TryGetValue(channelName, out outDoub))
                        {
                            sumSNCarrierVsSample["Sample"].Add(outDoub);
                        }
                    }
                }
            }

            double numOfBlanks = double.Parse(numberOfBlanks);
            numOfSamples = ((double) sumSNCarrierVsSample["Sample"].Count) - numOfBlanks;
            quantSN = sumSNCarrierVsSample["Sample"].Sum();
            carrierRatio = sumSNCarrierVsSample["Carrier"].Sum() / (sumSNCarrierVsSample["Sample"].Sum() / numOfSamples);

            return carrierRatio;
        }

        public void CalculateQuantSNSuggestedValues(double maxITAdjustment, double snAdjTB)
        {
            double snPerChannel = SNperChannel;
            double numOfSamples = NumberOfSamples;

            double quantIonFlux = -1;
            if (QuantOrder == 2)
            {
                quantIonFlux = MedianMS2QuantIonFlux;
            }
            else if (QuantOrder == 3)
            {
                quantIonFlux = MedianMS3QuantIonFlux;
            }

            double suggestedSNCutoff = SNperChannel * NumberOfSamples * snAdjTB;
            double suggestedMaxIT = ((suggestedSNCutoff * maxITAdjustment) / quantIonFlux);

            SuggestedQuantSNCutoff = (int) Math.Round(suggestedSNCutoff, 0);
            SuggestedQuantMaxIT = (int)Math.Round(suggestedMaxIT, 0);

            TotalScans = (double)MSnScans.Count;
            QuantSNQuantScans = 0;
            foreach (MSnScan scan in MSnScans)
            {
                if (scan.QuantSN >= SuggestedQuantSNCutoff)
                {
                    QuantSNQuantScans++;
                }
            }

            QuatSNQuantPercent = Math.Round(100 * QuantSNQuantScans / TotalScans, 1);
        }

        private double CalculateStandardDeviation(IEnumerable<double> values)
        {
            double standardDeviation = 0;

            if (values.Any())
            {
                // Compute the average.     
                double avg = values.Average();

                // Perform the Sum of (value-avg)_2_2.      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));

                // Put it all together.      
                standardDeviation = Math.Sqrt((sum) / (values.Count() - 1));
            }

            return standardDeviation;
        }

        public double ThirdQuartile(double[] afVal)
        {
            int iSize = afVal.Length;
            int iMid = iSize / 2; //this is the mid from a zero based index, eg mid of 7 = 3;

            double fQ1 = 0;
            double fQ2 = 0;
            double fQ3 = 0;

            if (iSize % 2 == 0)
            {
                //================ EVEN NUMBER OF POINTS: =====================
                //even between low and high point
                fQ2 = (afVal[iMid - 1] + afVal[iMid]) / 2;

                int iMidMid = iMid / 2;

                //easy split 
                if (iMid % 2 == 0)
                {
                    fQ1 = (afVal[iMidMid - 1] + afVal[iMidMid]) / 2;
                    fQ3 = (afVal[iMid + iMidMid - 1] + afVal[iMid + iMidMid]) / 2;
                }
                else
                {
                    fQ1 = afVal[iMidMid];
                    fQ3 = afVal[iMidMid + iMid];
                }
            }
            else if (iSize == 1)
            {
                //================= special case, sorry ================
                fQ1 = afVal[0];
                fQ2 = afVal[0];
                fQ3 = afVal[0];
            }
            else
            {
                //odd number so the median is just the midpoint in the array.
                fQ2 = afVal[iMid];

                if ((iSize - 1) % 4 == 0)
                {
                    //======================(4n-1) POINTS =========================
                    int n = (iSize - 1) / 4;
                    fQ1 = (afVal[n - 1] * .25) + (afVal[n] * .75);
                    fQ3 = (afVal[3 * n] * .75) + (afVal[3 * n + 1] * .25);
                }
                else if ((iSize - 3) % 4 == 0)
                {
                    //======================(4n-3) POINTS =========================
                    int n = (iSize - 3) / 4;

                    fQ1 = (afVal[n] * .75) + (afVal[n + 1] * .25);
                    fQ3 = (afVal[3 * n + 1] * .25) + (afVal[3 * n + 2] * .75);
                }
            }

            return fQ3;
        }

        public void CalculateSuggestedValues(double carrierProteomeLevel, double slope, double yIntercept, double maxITAdjustment, bool calculateCarrierRatio = true)
        {
            UserDefinedCarrierRatio = carrierProteomeLevel;
            UseCalculatedRatio = calculateCarrierRatio;

            if (calculateCarrierRatio)
            {
                carrierProteomeLevel = CalculatedCarrierRatio;
            }

            double ionFlux = -1;
            if(QuantOrder == 2)
            {
                ionFlux = MedianMS2IonFlux;
            }
            else if(QuantOrder == 3)
            {
                ionFlux = MedianMS3IonFlux;
            }

            double suggestedSNCutoff = (carrierProteomeLevel * slope) + yIntercept;
            double suggestedMaxIT = (suggestedSNCutoff / ionFlux) * maxITAdjustment;

            int divide = 10;
            while(suggestedSNCutoff/divide > 10)
            {
                SuggestedSNCutoff = (int) Math.Ceiling(suggestedSNCutoff / divide) * divide;

                divide *= 10;
            }

            if(suggestedMaxIT > 1000)
            {
                divide = 10;
                while (suggestedMaxIT / divide > 10)
                {
                    SuggestedMaxIT = (int)Math.Ceiling(suggestedMaxIT / divide) * divide;

                    divide *= 10;
                }
            }
            else
            {
                divide = 10;
                while (suggestedMaxIT / divide > 1)
                {
                    SuggestedMaxIT = (int)Math.Ceiling(suggestedMaxIT / divide) * divide;

                    divide *= 10;
                }
            }

            TotalScans = (double)MSnScans.Count;
            QuantScans = 0;
            foreach(MSnScan scan in MSnScans)
            {
                if(scan.SumSN >= SuggestedSNCutoff)
                {
                    QuantScans++;
                }
            }

            QuantPercent = Math.Round(100 * QuantScans / TotalScans, 1);
        }

        private double Median(List<double> numberList)
        {
            if(numberList.Count == 0)
            {
                return -1;
            }

            double[] numbers = numberList.ToArray();

            int numberCount = numbers.Count();
            int halfIndex = numbers.Count() / 2;
            var sortedNumbers = numbers.OrderBy(n => n);
            double median;
            if ((numberCount % 2) == 0)
            {
                median = ((sortedNumbers.ElementAt(halfIndex) + sortedNumbers.ElementAt(halfIndex - 1)) / 2);
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }

            return median;
        }

        private double MedianWithoutNA(List<double> list)
        {
            List<double> numberList = new List<double>();
            foreach (double number in list)
            {
                if (HasValue(number))
                {
                    numberList.Add(number);
                }
            }

            if (numberList.Count == 0)
            {
                return -1;
            }

            double[] numbers = numberList.ToArray();

            int numberCount = numbers.Count();
            int halfIndex = numbers.Count() / 2;
            var sortedNumbers = numbers.OrderBy(n => n);
            double median;
            if ((numberCount % 2) == 0)
            {
                median = ((sortedNumbers.ElementAt(halfIndex) + sortedNumbers.ElementAt(halfIndex - 1)) / 2);
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }

            return median;
        }

        private double MedianWithoutNA(SortedList<double, double> snToCarrierRatio, string carrierRatioHistCutoff, double minSN = 0)
        {
            double carrierRatioHistCutoffDoub = double.Parse(carrierRatioHistCutoff); //histCarrierRatioCutoffTB
            List<double> numberList = new List<double>();
            foreach(KeyValuePair<double, double> kvp in snToCarrierRatio)
            {
                if(HasValue(kvp.Value) && kvp.Key != 0 && kvp.Key >= minSN && kvp.Value >= carrierRatioHistCutoffDoub)
                {
                    numberList.Add(kvp.Value);
                }
            }

            CarrierHistogram = numberList;

            if (numberList.Count == 0)
            {
                return -1;
            }

            double[] numbers = numberList.ToArray();

            int numberCount = numbers.Count();
            int halfIndex = numbers.Count() / 2;
            var sortedNumbers = numbers.OrderBy(n => n);
            double median;
            if ((numberCount % 2) == 0)
            {
                median = ((sortedNumbers.ElementAt(halfIndex) + sortedNumbers.ElementAt(halfIndex - 1)) / 2);
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }

            return median;
        }

        private double MedianWithoutNAPercent(SortedList<double, double> snToCarrierRatio, string carrierRatioHistCutoff, double percent = 2)
        {
            double carrierRatioHistCutoffDoub = double.Parse(carrierRatioHistCutoff); //histCarrierRatioCutoffTB

            int dictCount = SNtoCarrierRatio.Count;
            int percentCount = (int) Math.Floor(dictCount * percent / 100);
            int percentIndex = dictCount - percentCount;

            List<double> numberList = new List<double>();
            for (int i = percentIndex; i < dictCount; i++)
            {
                double value = SNtoCarrierRatio.ElementAt(i).Value;
                if (HasValue(value) && value != 0 && value >= carrierRatioHistCutoffDoub)
                {
                    numberList.Add(value);
                }
            }

            CarrierHistogram = numberList;

            if (numberList.Count == 0)
            {
                return -1;
            }

            double[] numbers = numberList.ToArray();

            int numberCount = numbers.Count();
            int halfIndex = numbers.Count() / 2;
            var sortedNumbers = numbers.OrderBy(n => n);
            double median;
            if ((numberCount % 2) == 0)
            {
                median = ((sortedNumbers.ElementAt(halfIndex) + sortedNumbers.ElementAt(halfIndex - 1)) / 2);
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }

            return median;
        }

        public bool HasValue(double value)
        {
            return !Double.IsNaN(value) && !Double.IsInfinity(value);
        }

        public string PrintSummary()
        {
            string retString = "";

            if (QuantOrder == 2)
            {
                retString = PrintMS2Data();
            }
            else if (QuantOrder == 3)
            {
                retString = PrintMS3Data();
            }

            return retString;
        }

        public string PrintMS2Data()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(RawFileName);
            sb.Append(',');
            sb.Append(NumMS1Scans);
            sb.Append(',');
            sb.Append(NumMS2Scans);
            sb.Append(',');
            sb.Append(MedianMS2IT);
            sb.Append(',');
            sb.Append(MedianMS2SN);
            sb.Append(',');
            sb.Append(MedianMS2IonFlux);
            sb.Append(',');
            sb.Append(MedianMS2QuantSN);
            sb.Append(',');
            sb.Append(MedianMS2QuantIonFlux);
            sb.Append(',');
            sb.Append(CalculatedCarrierRatio);
            sb.Append(',');
            sb.Append(SuggestedQuantSNCutoff);
            sb.Append(',');
            sb.Append(SuggestedQuantMaxIT);

            return sb.ToString();
        }

        public string PrintMS3Data()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(RawFileName);
            sb.Append(',');
            sb.Append(NumMS1Scans);
            sb.Append(',');
            sb.Append(NumMS2Scans);
            sb.Append(',');
            sb.Append(NumMS3Scans);
            sb.Append(',');
            sb.Append(MedianMS2IT);
            sb.Append(',');
            sb.Append(MedianMS3IT);
            sb.Append(',');
            sb.Append(MedianMS3SN);
            sb.Append(',');
            sb.Append(MedianMS3IonFlux);
            sb.Append(',');
            sb.Append(MedianMS3QuantSN);
            sb.Append(',');
            sb.Append(MedianMS3QuantIonFlux);
            sb.Append(',');
            sb.Append(CalculatedCarrierRatio);
            sb.Append(',');
            sb.Append(SuggestedQuantSNCutoff);
            sb.Append(',');
            sb.Append(SuggestedQuantMaxIT);

            return sb.ToString();
        }

        public void PrintQuantScans(string Path, List<Modification> tmtIons)
        {
            using (StreamWriter writer = new StreamWriter(Path + "\\SCPCompanionAnalysis_MS" + QuantOrder + "_" + RawFileName + ".csv"))
            {
                StringBuilder headerLine = new StringBuilder();

                if(QuantOrder == 2)
                {
                    headerLine.Append("RawFileName,RetentionTime,MS2 Scan Number,MS2 Injection Time,Sum SN,Ion Flux,Quant SN,Quant Ion Flux,Max IT Percent,Estimted Carrier Ratio");
                }
                else if (QuantOrder == 3)
                {
                    headerLine.Append("RawFileName,RetentionTime,MS2 Scan Number,MS3 Scan Number,MS2 Injection Time,MS3 Inj Time,Sum SN,Ion Flux,Quant SN,Quant Ion Flux,Max IT Percent,Estimted Carrier Ratio");
                }

                foreach (Modification tmt in tmtIons)
                {
                    headerLine.Append(',');
                    headerLine.Append(tmt.Name);              
                }

                foreach (Modification tmt in tmtIons)
                {
                    headerLine.Append(',');
                    headerLine.Append(tmt.Name + "_ppmerror");
                }

                writer.WriteLine(headerLine.ToString());

                List<string> printList = new List<string>();
                if(QuantOrder == 2) { printList = PrintMS2Scans(); } else if (QuantOrder == 3) { printList = PrintMS3Scans(); }

                foreach(string printLine in printList)
                {
                    writer.WriteLine(printLine);
                }
            }
        }

        public List<string> PrintMS2Scans()
        {
            List<string> scanStrings = new List<string>();
            foreach (MSnScan scan in MSnScans)
            {
                StringBuilder sb2 = new StringBuilder();
                sb2.Append(RawFileName);
                sb2.Append(',');
                sb2.Append(scan.RetentionTime);
                sb2.Append(',');
                sb2.Append(scan.ScanNumber);
                sb2.Append(',');
                sb2.Append(scan.InjectionTime);
                sb2.Append(',');
                sb2.Append(scan.SumSN);
                sb2.Append(',');
                sb2.Append(scan.IonFlux);
                sb2.Append(',');
                sb2.Append(scan.QuantSN);
                sb2.Append(',');
                sb2.Append(scan.QuantIonFlux);
                sb2.Append(',');
                sb2.Append(scan.MaxITPercent);
                sb2.Append(',');
                sb2.Append(scan.CarrierRatio);

                foreach (KeyValuePair<string, double> kvp in scan.SNDict)
                {
                    sb2.Append(',');
                    sb2.Append(kvp.Value);
                }

                foreach (KeyValuePair<string, double> kvp in scan.MassErrorDict)
                {
                    sb2.Append(',');
                    sb2.Append(kvp.Value);
                }

                scanStrings.Add(sb2.ToString());
            }

            return (scanStrings);
        }

        public List<string> PrintMS3Scans()
        {
            List<string> scanStrings = new List<string>();
            foreach (MSnScan scan in MSnScans)
            {
                StringBuilder sb2 = new StringBuilder();
                sb2.Append(RawFileName);
                sb2.Append(',');
                sb2.Append(scan.RetentionTime);
                sb2.Append(',');
                sb2.Append(scan.ParentScanNumber);
                sb2.Append(',');
                sb2.Append(scan.ScanNumber);
                sb2.Append(',');
                sb2.Append(scan.ParentInjectionTime);
                sb2.Append(',');
                sb2.Append(scan.InjectionTime);
                sb2.Append(',');
                sb2.Append(scan.SumSN);
                sb2.Append(',');
                sb2.Append(scan.IonFlux);
                sb2.Append(',');
                sb2.Append(scan.QuantSN);
                sb2.Append(',');
                sb2.Append(scan.QuantIonFlux);
                sb2.Append(',');
                sb2.Append(scan.MaxITPercent);
                sb2.Append(',');
                sb2.Append(scan.CarrierRatio);

                foreach (KeyValuePair<string, double> kvp in scan.SNDict)
                {
                    sb2.Append(',');
                    sb2.Append(kvp.Value);
                }

                foreach (KeyValuePair<string, double> kvp in scan.MassErrorDict)
                {
                    sb2.Append(',');
                    sb2.Append(kvp.Value);
                }

                scanStrings.Add(sb2.ToString());
            }

            return (scanStrings);
        }
    }
}
