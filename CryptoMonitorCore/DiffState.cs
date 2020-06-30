using System;
using System.Globalization;
using System.IO;

namespace CryptoMonitorCore
{
    public class DiffState
    {
        private Symbol objA;
        private Symbol objB;
        private decimal minDiffValue;
        private decimal minStepValue;

        private decimal lastDiffAB;
        private bool wasProfitableAB;
        private DateTime lastABtime;

        private decimal lastDiffBA;
        private bool wasProfitableBA;
        private DateTime lastBAtime;

        public Symbol ObjA { get { return objA; } }
        public Symbol ObjB { get { return objB; } }

        public DiffState(Symbol objA, Symbol objB, decimal minDiffValue, decimal minStepValue)
        {
            this.objA = objA;
            this.objB = objB;
            this.minDiffValue = minDiffValue;
            this.minStepValue = minStepValue;
        }


        public void WriteDiffState()
        {
            if (objA.Ask != 0 && objA.Bid != 0 && objB.Ask != 0 && objB.Bid != 0)
            {
                string message = $"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff")}   ";
                if (objA.Ask < objB.Bid)
                {
                    noLongerProfitableBA();
                    decimal AB = ((objB.Bid - objA.Ask) / objA.Ask) * 100;
                    if (AB >= minDiffValue && (Math.Abs(AB - lastDiffAB) >= minStepValue))
                    {
                        message += $"{objA.ExchangeName}_{objB.ExchangeName}   ";
                        message += $"{TruncateDecimal(AB, 3)} /{objA.Ask.ToString(CultureInfo.InvariantCulture)} {objA.ExchangeName} \\{objB.Bid.ToString(CultureInfo.InvariantCulture)} {objB.ExchangeName}\n";
                        File.AppendAllText($"{objA.SymbolName}.log", message);
                        wasProfitableAB = true;
                        lastDiffAB = AB;
                        lastABtime = DateTime.Now;
                    }
                    else if (AB < minDiffValue)
                    {
                        noLongerProfitableAB();
                    }
                }
                else if (objB.Ask < objA.Bid)
                {
                    noLongerProfitableAB();
                    decimal BA = ((objA.Bid - objB.Ask) / objB.Ask) * 100;
                    if (BA >= minDiffValue && (Math.Abs(BA - lastDiffBA) >= minStepValue))
                    {
                        message += $"{objA.ExchangeName}_{objB.ExchangeName}   ";
                        message += $"{TruncateDecimal(BA, 3)} /{objB.Ask.ToString(CultureInfo.InvariantCulture)} {objB.ExchangeName} \\{objA.Bid.ToString(CultureInfo.InvariantCulture)} {objA.ExchangeName}\n";
                        File.AppendAllText($"{objA.SymbolName}.log", message);
                        wasProfitableBA = true;
                        lastDiffBA = BA;
                        lastBAtime = DateTime.Now;
                    }
                    else if (BA < minDiffValue)
                    {
                        noLongerProfitableBA();
                    }
                }
                else
                {
                    noLongerProfitableAB();
                    noLongerProfitableBA();
                }
            }
        }

        private void noLongerProfitableAB()
        {
            if (wasProfitableAB == true)
            {
                DateTime currentDateTime = DateTime.Now;
                TimeSpan timeDiff = currentDateTime - lastABtime;
                string message = $"{currentDateTime.ToString("dd.MM.yyyy HH:mm:ss.fff")}   ";
                message += $"{objA.ExchangeName}_{objB.ExchangeName}   ";
                message += $"{timeDiff:hh\\:mm\\:ss\\.fff}\n";
                File.AppendAllText($"{objA.SymbolName}.log", message);
            }
            wasProfitableAB = false;
            lastDiffAB = 0m;
        }

        private void noLongerProfitableBA()
        {
            if (wasProfitableBA == true)
            {
                DateTime currentDateTime = DateTime.Now;
                TimeSpan timeDiff = currentDateTime - lastBAtime;
                string message = $"{currentDateTime.ToString("dd.MM.yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture)}   ";
                message += $"{objA.ExchangeName}_{objB.ExchangeName}   ";
                message += $"{timeDiff:hh\\:mm\\:ss\\.fff}\n";
                File.AppendAllText($"{objA.SymbolName}.log", message);
            }
            wasProfitableBA = false;
            lastDiffBA = 0m;
        }

        string TruncateDecimal(decimal value, int precision)
        {
            decimal step = (decimal)Math.Pow(10, precision);
            decimal tmp = Math.Truncate(step * value);
            decimal result = tmp / step;
            return result.ToString(CultureInfo.InvariantCulture);
        }

        public void DiffStateStatus()
        {
            if (objA.SymbolName == objB.SymbolName)
            {
                Console.WriteLine($"{objA.SymbolName}: {objA.ExchangeName} – {objB.ExchangeName}");
            }
            else
            {
                Console.WriteLine("Error in DiffStates");
            }
        }
    }
}
