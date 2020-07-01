using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoMonitorCore
{
    public class Symbol
    {
        private decimal bestAsk;
        private decimal bestBid;
        public decimal lastAsk { get; set; }
        public decimal lastBid { get; set; }
        private string coinA;
        private string coinB;
        private SortedDictionary<decimal, decimal> asks = new SortedDictionary<decimal, decimal>();
        private SortedDictionary<decimal, decimal> bids = new SortedDictionary<decimal, decimal>();

        public Symbol()
        {
        }

        public Symbol(string exchangeName, string coinA, string coinB)
        {
            ExchangeName = exchangeName;
            this.coinA = coinA;
            this.coinB = coinB;
        }

        public string ExchangeName { get; }
        public string SymbolName
        {
            get
            {
                return $"{coinA}-{coinB}";
            }
        }

        public string CoinA
        {
            get { return coinA; }
        }

        public string CoinB
        {
            get { return coinB; }
        }

        public decimal Ask
        {
            get
            {
                return bestAsk;
            }
        }
        public decimal Bid
        {
            get
            {
                return Math.Abs(bestBid);
            }
        }

        public void ClearGlass()
        {
            asks.Clear();
            bids.Clear();
        }

        public void AddAskEl(decimal value, decimal volume)
        {
            asks.Add(value, volume);
        }
        public void AddBidEl(decimal value, decimal volume)
        {
            bids.Add(value, volume);
        }

        public void AddAskElPartial(decimal value, decimal volume)
        {
            if (asks.ContainsKey(value))
            {
                asks[value] = volume;
            }
            else
            {
                asks.Add(value, volume);
            }
        }
        public void AddBidElPartial(decimal value, decimal volume)
        {
            if (bids.ContainsKey(value))
            {
                bids[value] = volume;
            }
            else
            {
                bids.Add(value, volume);
            }
        }

        public void RegisterGlass(decimal minVolume)
        {
            foreach (var s in asks.Where(kv => kv.Value < minVolume).ToList())
            {
                asks.Remove(s.Key);
            }

            foreach (var s in bids.Where(kv => kv.Value < minVolume).ToList())
            {
                bids.Remove(s.Key);
            }

            foreach (decimal key in asks.Keys)
            {
                bestAsk = key;
                break;
            }
            foreach (decimal key in bids.Keys)
            {
                bestBid = key;
                break;
            }

            //Console.WriteLine($"{ExchangeName} {SymbolName} Asks");
            //foreach (decimal key in asks.Keys)
            //{
            //    Console.WriteLine($"{key} {asks[key]}");
            //}

            //Console.WriteLine($"------------Bids");
            //foreach (decimal key in bids.Keys)
            //{
            //    Console.WriteLine($"{Math.Abs(key)} {bids[key]}");
            //}
            //Console.WriteLine($"----------------");
        }

    }
}
