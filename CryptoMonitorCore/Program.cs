﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Websocket.Client;

namespace CryptoMonitorCore
{
    public class SettingsUrls
    {
        public string gateio { get; set; }
        public string okex { get; set; }
        public string huobi { get; set; }
    }
    public class General
    {
        public string minDiff { get; set; }
        public string minStep { get; set; }
        public string lotMinUsd { get; set; }
    }
    public class Settings
    {
        public List<List<string>> Symbols { get; set; }
        public SettingsUrls Urls { get; set; }
        public General General { get; set; }
    }    

    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            var exitEvent = new ManualResetEvent(false);
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Config.json")
            .Build();
            var setting = new Settings();
            configuration.Bind(setting);

            Currencies.checkSymbolsForAvailabilityInExchanges(setting);
            Currencies.formedPairs();

            List<Symbol> symbols = Utils.InitSymbols(setting);

            List<SymbolMarket> symbolMarkets = Utils.InitSymbolMarkets(setting, symbols, client);            

            string gateioJson = Utils.gateioStringGenerator(symbols);
            string okexJson = Utils.okexStringGenerator(symbols);
            List<string> huobuJsons = Utils.huobiStringGenerator(symbols);

            WebsocketClient gateioClient = WebSocketClients.StartGateIO(setting.Urls.gateio, symbols, symbolMarkets);
            Task.Run(() => gateioClient.Start());
            Task.Run(() => gateioClient.Send($"{gateioJson}"));

            WebsocketClient okexClient = WebSocketClients.StartOkex(setting.Urls.okex, symbols, symbolMarkets);
            Task.Run(() => okexClient.Start());
            Task.Run(() => okexClient.Send($"{okexJson}"));

            WebsocketClient huobiClient = WebSocketClients.StartHuobi(setting.Urls.huobi, symbols, symbolMarkets);
            Task.Run(() => huobiClient.Start());
            foreach (string hubiJson in huobuJsons)
            {
                Task.Run(() => huobiClient.Send($"{hubiJson}"));
            }

            exitEvent.WaitOne();
        }
    }
}
