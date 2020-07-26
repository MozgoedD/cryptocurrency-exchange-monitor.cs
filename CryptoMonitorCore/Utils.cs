using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoMonitorCore
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

    public static class Utils
    {
        public static List<Symbol> InitSymbols(Settings setting)
        {
            List<string> exhanges = new List<string>() { "gate", "okex", "huobi" };
            ISymbolFactory symbolFactory = new SymbolFactory();
            List<Symbol> symbols = new List<Symbol>();
            foreach (var SymSettingList in setting.Symbols)
            {
                if (Currencies.gate_okex_Intersect.Contains($"{SymSettingList[0]}-{SymSettingList[1]}") || Currencies.okex_huobiIntersect.Contains($"{SymSettingList[0]}-{SymSettingList[1]}") || Currencies.gate_huobiIntersect.Contains($"{SymSettingList[0]}-{SymSettingList[1]}"))
                {
                    foreach (string exchange in exhanges)
                    {
                        Symbol symbolObj = symbolFactory.CreateSymbol(exchange, SymSettingList[0], SymSettingList[1]);
                        symbols.Add(symbolObj);
                        //Console.WriteLine($"Symbol: {symbolObj.ExchangeName} {symbolObj.SymbolName} has been initialized");
                    }
                }
                else if (Currencies.gateReadyToGo.Contains($"{SymSettingList[0]}-{SymSettingList[1]}") || Currencies.okexReadyToGo.Contains($"{SymSettingList[0]}-{SymSettingList[1]}") || Currencies.huobiReadyToGo.Contains($"{SymSettingList[0]}-{SymSettingList[1]}"))
                {
                    Console.WriteLine($"{SymSettingList[0]}-{SymSettingList[1]} available only for 1 exchange. This symbol will not not be included in the program");
                    continue;
                }
                else
                {
                    Console.WriteLine($"{SymSettingList[0]}-{SymSettingList[1]} does not support by any exchange!!!");
                    continue;
                }
            }
            return symbols;
        }

        public static List<SymbolMarket> InitSymbolMarkets(Settings setting, List<Symbol> symbols, HttpClient client)
        {
            ISymbolFactory symbolFactory = new SymbolFactory();
            List<SymbolMarket> symbolMarkets = new List<SymbolMarket>();
            foreach (var SymSettingList in setting.Symbols)
            {
                //var responseString = client.GetStringAsync($"https://data.gateio.la/api2/1/ticker/{SymSettingList[0].ToLower()}_usdt");
                //JObject jsonObj = JObject.Parse(responseString.Result);
                //decimal coinUSDT = Convert.ToDecimal(jsonObj["last"], CultureInfo.InvariantCulture);
                //decimal minVolume = lotMinUsd / coinUSDT;

                if (Currencies.gate_okex_Intersect.Contains($"{SymSettingList[0]}-{SymSettingList[1]}") || Currencies.okex_huobiIntersect.Contains($"{SymSettingList[0]}-{SymSettingList[1]}") || Currencies.gate_huobiIntersect.Contains($"{SymSettingList[0]}-{SymSettingList[1]}"))
                {
                    decimal minDiffValue = Convert.ToDecimal(setting.General.minDiff, CultureInfo.InvariantCulture);
                    decimal minStepValue = Convert.ToDecimal(setting.General.minStep, CultureInfo.InvariantCulture);
                    decimal lotMinUsd = Convert.ToDecimal(setting.General.lotMinUsd, CultureInfo.InvariantCulture);
                    decimal minVolume = Currencies.getMinVolume(SymSettingList[0], lotMinUsd, client);

                    SymbolMarket symMarketObj = symbolFactory.CreateSymbolMarket($"{SymSettingList[0]}-{SymSettingList[1]}", minDiffValue, minStepValue, minVolume, symbols);
                    symbolMarkets.Add(symMarketObj);
                    symMarketObj.SymMarketStatus();
                }
                else
                {
                    //Console.WriteLine($"{SymSettingList[0]}-{SymSettingList[1]} does not support by any exchange!!!");
                    continue;
                }
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
