using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace CryptoMonitorCore
{
    public class WebSocketClients
    {

        public static string DecompressOkex(byte[] baseBytes)
        {
            using (var decompressedStream = new MemoryStream())
            using (var compressedStream = new MemoryStream(baseBytes))
            using (var deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                deflateStream.CopyTo(decompressedStream);
                decompressedStream.Position = 0;
                using (var streamReader = new StreamReader(decompressedStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public static byte[] DecompressHuobi(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }

        public static WebsocketClient StartGateIO(string url, List<Symbol> symbols, List<SymbolMarket> symbolMarkets)
        {
            Uri gateio_url = new Uri(url);
            var gateioClient = new WebsocketClient(gateio_url);
            Symbol processingSymObj = new Symbol();
            SymbolMarket processingSymMarketObj = new SymbolMarket();
            List<List<string>> asks = new List<List<string>>();
            List<List<string>> bids = new List<List<string>>();
            gateioClient.ReconnectTimeout = TimeSpan.FromSeconds(30);
            gateioClient.ReconnectionHappened.Subscribe(info =>
                Console.WriteLine($"Gate.io Reconnection happened, type: {info.Type}"));
            gateioClient
                .MessageReceived
                .ObserveOn(TaskPoolScheduler.Default)
                .Subscribe(msg =>
                {
                    asks.Clear();
                    bids.Clear();
                    string jsonString = msg.ToString();
                    //LogWriters.WriteResponce("gate.io", jsonString);

                    JObject jsonObj = JObject.Parse(jsonString);
                    if (jsonObj.ContainsKey("result"))
                    {
                        if (jsonObj["result"]["status"].ToString() == "success")
                        {
                            Console.WriteLine("gate.io: OK");
                        }
                    }
                    else if (jsonObj.ContainsKey("params"))
                    {                      
                        string processingSym = jsonObj["params"][2].ToString();
                        processingSym = processingSym.Replace("_", "-");
                        foreach (Symbol symObj in symbols)
                        {
                            if (symObj.ExchangeName == "gate" && symObj.SymbolName == processingSym)
                            {
                                processingSymObj = symObj;
                                break;
                            }
                        }
                        foreach (SymbolMarket symMarketObj in symbolMarkets)
                        {
                            if (symMarketObj.SymbolName == processingSym)
                            {
                                processingSymMarketObj = symMarketObj;
                                break;
                            }
                        }

                        if (jsonObj["params"][1]["asks"] != null)
                        {
                            asks = jsonObj["params"][1]["asks"].ToObject<List<List<string>>>();
                        }
                        if (jsonObj["params"][1]["bids"] != null)
                        {
                            bids = jsonObj["params"][1]["bids"].ToObject<List<List<string>>>();
                        }

                        if (Convert.ToBoolean(jsonObj["params"][0].ToString()) == true)
                        {
                            processingSymObj.ClearGlass();
                            foreach (var valVol in asks)
                            {
                                decimal value = Convert.ToDecimal(valVol[0], CultureInfo.InvariantCulture);
                                decimal volume = Convert.ToDecimal(valVol[1], CultureInfo.InvariantCulture);
                                processingSymObj.AddAskEl(value, volume);
                            }
                            foreach (var valVol in bids)
                            {
                                decimal value = Convert.ToDecimal(valVol[0], CultureInfo.InvariantCulture);
                                decimal volume = Convert.ToDecimal(valVol[1], CultureInfo.InvariantCulture);
                                processingSymObj.AddBidEl(value * -1, volume);
                            }
                        }
                        else if (Convert.ToBoolean(jsonObj["params"][0].ToString()) == false)
                        {
                            foreach (var valVol in asks)
                            {
                                decimal value = Convert.ToDecimal(valVol[0], CultureInfo.InvariantCulture);
                                decimal volume = Convert.ToDecimal(valVol[1], CultureInfo.InvariantCulture);
                                processingSymObj.AddAskElPartial(value, volume);
                            }
                            foreach (var valVol in bids)
                            {
                                decimal value = Convert.ToDecimal(valVol[0], CultureInfo.InvariantCulture);
                                decimal volume = Convert.ToDecimal(valVol[1], CultureInfo.InvariantCulture);
                                processingSymObj.AddBidElPartial(value * -1, volume);
                            }
                        }
                        processingSymObj.RegisterGlass(processingSymMarketObj.MinVolume);
                        processingSymMarketObj.WriteMarket();
                    }
                });
            return gateioClient;
        }

        public static WebsocketClient StartOkex(string url, List<Symbol> symbols, List<SymbolMarket> symbolMarkets)
        {
            Uri okex_url = new Uri(url);
            var okexClient = new WebsocketClient(okex_url);
            Symbol processingSymObj = new Symbol();
            SymbolMarket processingSymMarketObj = new SymbolMarket();
            List<List<string>> asks = new List<List<string>>();
            List<List<string>> bids = new List<List<string>>();
            okexClient.ReconnectTimeout = TimeSpan.FromSeconds(30);
            okexClient.ReconnectionHappened.Subscribe(info =>
                Console.WriteLine($"Okex Reconnection happened, type: {info.Type}"));
            okexClient
                .MessageReceived
                .ObserveOn(TaskPoolScheduler.Default)
                .Subscribe(msg =>
                {
                    asks.Clear();
                    bids.Clear();
                    byte[] bytes = msg.Binary;
                    string jsonString = DecompressOkex(bytes);
                    //LogWriters.WriteResponce("Okex", jsonString);

                    JObject jsonObj = JObject.Parse(jsonString);

                    if (jsonObj.ContainsKey("event"))
                    {
                        if (jsonObj["event"].ToString() == $"subscribe")
                        {
                            string okSym = jsonObj["channel"].ToString();
                            okSym = okSym.Substring(11);
                            Console.WriteLine($"Okex: {okSym} OK");
                        }
                    }
                    else if (jsonObj.ContainsKey("data"))
                    {
                        string processingSym = jsonObj["data"][0]["instrument_id"].ToString();

                        foreach (Symbol symObj in symbols)
                        {
                            if (symObj.ExchangeName == "okex" && symObj.SymbolName == processingSym)
                            {
                                processingSymObj = symObj;
                                break;
                            }
                        }
                        foreach (SymbolMarket symMarketObj in symbolMarkets)
                        {
                            if (symMarketObj.SymbolName == processingSym)
                            {
                                processingSymMarketObj = symMarketObj;
                                break;
                            }
                        }
                        asks = jsonObj["data"][0]["asks"].ToObject<List<List<string>>>();
                        bids = jsonObj["data"][0]["bids"].ToObject<List<List<string>>>();

                        if (jsonObj["action"].ToString() == "partial")
                        {
                            processingSymObj.ClearGlass();
                            foreach (var valVol in asks)
                            {
                                decimal value = Convert.ToDecimal(valVol[0], CultureInfo.InvariantCulture);
                                decimal volume = Convert.ToDecimal(valVol[1], CultureInfo.InvariantCulture);
                                processingSymObj.AddAskEl(value, volume);
                            }
                            foreach (var valVol in bids)
                            {
                                decimal value = Convert.ToDecimal(valVol[0], CultureInfo.InvariantCulture);
                                decimal volume = Convert.ToDecimal(valVol[1], CultureInfo.InvariantCulture);
                                processingSymObj.AddBidEl(value * -1, volume);
                            }
                            processingSymObj.RegisterGlass(processingSymMarketObj.MinVolume);
                            processingSymMarketObj.WriteMarket();
                        }
                        if (jsonObj["action"].ToString() == "update")
                        {

                            foreach (var valVol in asks)
                            {
                                decimal value = Convert.ToDecimal(valVol[0], CultureInfo.InvariantCulture);
                                decimal volume = Convert.ToDecimal(valVol[1], CultureInfo.InvariantCulture);
                                processingSymObj.AddAskElPartial(value, volume);
                            }
                            foreach (var valVol in bids)
                            {
                                decimal value = Convert.ToDecimal(valVol[0], CultureInfo.InvariantCulture);
                                decimal volume = Convert.ToDecimal(valVol[1], CultureInfo.InvariantCulture);
                                processingSymObj.AddBidElPartial(value * -1, volume);
                            }
                            processingSymObj.RegisterGlass(processingSymMarketObj.MinVolume);
                            processingSymMarketObj.WriteMarket();
                        }
                    }
                });
            return okexClient;
        }

        public static WebsocketClient StartHuobi(string url, List<Symbol> symbols, List<SymbolMarket> symbolMarkets)
        {
            Uri okex_url = new Uri(url);
            var huobiClient = new WebsocketClient(okex_url);
            Symbol processingSymObj = new Symbol();
            SymbolMarket processingSymMarketObj = new SymbolMarket();
            List<List<string>> asks = new List<List<string>>();
            List<List<string>> bids = new List<List<string>>();
            huobiClient.ReconnectTimeout = TimeSpan.FromSeconds(30);
            huobiClient.ReconnectionHappened.Subscribe(info =>
                Console.WriteLine($"Huobi Reconnection happened, type: {info.Type}"));
            huobiClient
                .MessageReceived
                .ObserveOn(TaskPoolScheduler.Default)
                .Subscribe(msg =>
                {
                    asks.Clear();
                    bids.Clear();
                    byte[] bytes = msg.Binary;
                    string jsonString = Encoding.UTF8.GetString(DecompressHuobi(bytes));
                    //LogWriters.WriteResponce("Huobi", jsonString);

                    JObject jsonObj = JObject.Parse(jsonString);

                    if (jsonObj.ContainsKey("status"))
                    {
                        if (jsonObj["status"].ToString() == "ok")
                        {
                            string okSym = jsonObj["subbed"].ToString();
                            okSym = okSym.Substring(7);
                            okSym = okSym.Remove(okSym.Length - 15).ToUpper();
                            Console.WriteLine($"Huobi: {okSym} OK");
                        } 
                    }

                    if (jsonObj.ContainsKey("ping"))
                    {
                        long pingId = jsonObj["ping"].ToObject<long>();
                        Requests.HuobiPong huobiPong = new Requests.HuobiPong()
                        {
                            PongId = pingId
                        };
                        string json = JsonConvert.SerializeObject(huobiPong, Formatting.Indented);
                        Task.Run(() => huobiClient.Send($"{json}"));
                    }

                    if (jsonObj.ContainsKey("ch"))
                    {
                        string processingSym = jsonObj["ch"].ToString();

                        processingSym = processingSym.Substring(7);
                        processingSym = processingSym.Remove(processingSym.Length - 15).ToUpper();

                        foreach (Symbol symObj in symbols)
                        {
                            if (symObj.ExchangeName == "huobi" && symObj.SymbolName.Replace("-", "") == processingSym)
                            {
                                processingSymObj = symObj;
                                break;
                            }
                        }

                        foreach (SymbolMarket symMarketObj in symbolMarkets)
                        {
                            if (symMarketObj.SymbolName.Replace("-", "") == processingSym)
                            {
                                processingSymMarketObj = symMarketObj;
                                break;
                            }
                        }

                        asks = jsonObj["tick"]["asks"].ToObject<List<List<string>>>();
                        bids = jsonObj["tick"]["bids"].ToObject<List<List<string>>>();

                        processingSymObj.ClearGlass();

                        foreach (var valVol in asks)
                        {
                            decimal value = Convert.ToDecimal(valVol[0], CultureInfo.InvariantCulture);
                            decimal volume;
                            try
                            {
                                volume = Convert.ToDecimal(valVol[1], CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                //Console.WriteLine($"Exception with {valVol[1]}");
                                volume = decimal.Parse(valVol[1], NumberStyles.Float, CultureInfo.InvariantCulture);
                                //Console.WriteLine($"{valVol[1]} {volume}");
                            }
                            processingSymObj.AddAskEl(value, volume);
                        }
                        foreach (var valVol in bids)
                        {
                            decimal value = Convert.ToDecimal(valVol[0], CultureInfo.InvariantCulture);
                            decimal volume;
                            try
                            {
                                volume = Convert.ToDecimal(valVol[1], CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                //Console.WriteLine($"Exception with {valVol[1]}");
                                volume = decimal.Parse(valVol[1], NumberStyles.Float, CultureInfo.InvariantCulture);
                                //Console.WriteLine($"{valVol[1]} {volume}");
                            }
                            processingSymObj.AddBidEl(value * -1, volume);
                        }

                        processingSymObj.RegisterGlass(processingSymMarketObj.MinVolume);
                        processingSymMarketObj.WriteMarket();
                    }
                });
            return huobiClient;
        }
    }
}
