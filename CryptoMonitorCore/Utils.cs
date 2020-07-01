using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace CryptoMonitorCore
{
    public class Utils
    {
        interface ISymbolFactory
        {
            Symbol CreateSymbol(string exchangeName, string coinA, string coinB);
            SymbolMarket CreateSymbolMarket(string symbolName, decimal minDiffValue, decimal minStepValue, decimal minVolume, List<Symbol> symbols);
            DiffState CreateDiffState(Symbol objA, Symbol objB, decimal minDiffValue, decimal minStepValue);
        }

        public class SymbolFactory : ISymbolFactory
        {
            public Symbol CreateSymbol(string exchangeName, string coinA, string coinB)
            {
                return new Symbol(exchangeName, coinA, coinB);
            }
            public SymbolMarket CreateSymbolMarket(string symbolName, decimal minDiffValue, decimal minStepValue, decimal minVolume, List<Symbol> symbols)
            {
                return new SymbolMarket(symbolName, minDiffValue, minStepValue, minVolume, symbols);
            }
            public DiffState CreateDiffState(Symbol objA, Symbol objB, decimal minDiffValue, decimal minStepValue)
            {
                return new DiffState(objA, objB, minDiffValue, minStepValue);
            }
        }

        public static List<Symbol> InitSymbols(Settings setting)
        {
            List<string> exhanges = new List<string>() { "gate", "okex", "huobi" };
            ISymbolFactory symbolFactory = new SymbolFactory();
            List<Symbol> symbols = new List<Symbol>();
            foreach (var SymSettingList in setting.Symbols)
            {
                foreach (string exchange in exhanges)
                {
                    Symbol symbolObj = symbolFactory.CreateSymbol(exchange, SymSettingList[0], SymSettingList[1]);
                    symbols.Add(symbolObj);
                    Console.WriteLine($"Symbol: {symbolObj.ExchangeName} {symbolObj.SymbolName} has been initialized");
                }
            }
            return symbols;
        }

        public static List<SymbolMarket> InitSymbolMarkets(Settings setting, List<Symbol> symbols)
        {
            ISymbolFactory symbolFactory = new SymbolFactory();
            List<SymbolMarket> symbolMarkets = new List<SymbolMarket>();
            foreach (var SymSettingList in setting.Symbols)
            {
                decimal minDiffValue = Convert.ToDecimal(SymSettingList[2], CultureInfo.InvariantCulture);
                decimal minStepValue = Convert.ToDecimal(SymSettingList[3], CultureInfo.InvariantCulture);
                decimal minVolume = Convert.ToDecimal(SymSettingList[4], CultureInfo.InvariantCulture);
                SymbolMarket symMarketObj = symbolFactory.CreateSymbolMarket($"{SymSettingList[0]}-{SymSettingList[1]}", minDiffValue, minStepValue, minVolume, symbols);
                symbolMarkets.Add(symMarketObj);
                //symMarketObj.SymMarketStatus();
            }
            return symbolMarkets;
        }

        public static List<DiffState> InitDiffStates(List<Symbol> specificSymbols, decimal minDiffValue, decimal minStepValue)
        {
            ISymbolFactory symbolFactory = new SymbolFactory();
            List<DiffState> diffStates = new List<DiffState>();

            foreach (Symbol symbolObj in specificSymbols)
            {
                foreach (Symbol secondSymbolObj in specificSymbols)
                {
                    if (symbolObj != secondSymbolObj)
                    {
                        bool isFoudPair = false;
                        DiffState diffStateObj = symbolFactory.CreateDiffState(symbolObj, secondSymbolObj, minDiffValue, minStepValue);

                        foreach (DiffState diffStateListObj in diffStates)
                        {
                            if (diffStateListObj.ObjB == symbolObj && diffStateListObj.ObjA == secondSymbolObj)
                            {
                                isFoudPair = true;
                            }
                        }
                        if (!isFoudPair)
                        {
                            diffStates.Add(diffStateObj);
                        }                        
                    }
                }
            }
            return diffStates;
        }

        public static string gateioStringGenerator(List<Symbol> symbols)
        {
            ArrayList Params = new ArrayList();
            ArrayList symParams;
            foreach (Symbol symObj in symbols)
            {
                if(symObj.ExchangeName == "gate")
                {
                    symParams = new ArrayList()
                    {
                        $"{symObj.CoinA}_{symObj.CoinB}",
                        5,
                        "0.00000001"
                    };
                    Params.Add(symParams);
                }
            }
            Requests.GateIO reqObj = new Requests.GateIO()
            {
                Id = 1,
                Method= "depth.subscribe",
                Params = Params
            };
            string json = JsonConvert.SerializeObject(reqObj, Formatting.Indented);
            return json;
        }

        public static string okexStringGenerator(List<Symbol> symbols)
        {
            List<string> Args = new List<string>();
            foreach (Symbol symObj in symbols)
            {
                if (symObj.ExchangeName == "okex")
                {
                    Args.Add($"spot/depth:{symObj.CoinA}-{symObj.CoinB}");
                }                    
            }
            Requests.Okex reqObj = new Requests.Okex()
            {
                Op = "subscribe",
                Args = Args
            };
            string json = JsonConvert.SerializeObject(reqObj, Formatting.Indented);
            return json;
        }

        public static List<string> huobiStringGenerator(List<Symbol> symbols)
        {
            List<string> jsonStrings = new List<string>();
            int id = 1;
            string json;

            foreach (Symbol symObj in symbols)
            {
                if (symObj.ExchangeName == "huobi")
                {
                    Requests.Huobi reqObj = new Requests.Huobi()
                    {
                        Sub = $"market.{symObj.CoinA.ToLower()}{symObj.CoinB.ToLower()}.mbp.refresh.10",
                        Id = id
                    };
                    id++;
                    json = JsonConvert.SerializeObject(reqObj, Formatting.Indented);
                    jsonStrings.Add(json);
                }
            }
            return jsonStrings;
        }
    }
}
