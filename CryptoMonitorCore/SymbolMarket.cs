using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CryptoMonitorCore
{
    public class SymbolMarket
    {
        decimal minDiffValue;
        decimal minStepValue;
        decimal minVolume;
        List<Symbol> specificSymbols = new List<Symbol>();
        List<DiffState> diffStates = new List<DiffState>();
        public SymbolMarket()
        {
        }
        public SymbolMarket(string symbolName, decimal minDiffValue, decimal minStepValue, decimal minVolume, List<Symbol> symbols)
        {            
            SymbolName = symbolName;
            this.minDiffValue = minDiffValue;
            this.minStepValue = minStepValue;
            this.minVolume = minVolume;
            foreach (Symbol symObj in symbols)
            {
                if (symObj.SymbolName == SymbolName)
                {
                    specificSymbols.Add(symObj);
                }
            }
            diffStates = Utils.InitDiffStates(specificSymbols, minDiffValue, minStepValue);

            foreach (DiffState diffStateObj in diffStates)
            {
                diffStateObj.DiffStateStatus();
            }
        }
        public string SymbolName { get; }
        public decimal MinVolume
        {
            get { return minVolume;  }
        }

        public void WriteMarket()
        {            
            foreach (Symbol symObj in specificSymbols)
            {
                if (symObj.Ask != symObj.lastAsk && symObj.Bid != symObj.lastBid)
                {
                    string message = $"{DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss.fff", CultureInfo.InvariantCulture)} {symObj.ExchangeName} {SymbolName}:";
                    message += $"\nAsk: {symObj.Ask}; Bid: {symObj.Bid}";
                    message += "\n---------------------------\n";
                    File.AppendAllText("Market.log", message);
                    symObj.lastAsk = symObj.Ask;
                    symObj.lastBid = symObj.Bid;
                }
                foreach (DiffState diffStateObj in diffStates)
                {
                    if (diffStateObj.ObjA == symObj || diffStateObj.ObjB == symObj)
                    {
                        //diffStateObj.WriteMarketDiffState();
                        diffStateObj.WriteDiffState();
                    }
                }
            }

        }
        public void SymMarketStatus()
        {
            Console.WriteLine($"SymMarket {SymbolName} {minDiffValue} {minStepValue} {minVolume}");
        }
    }
}
