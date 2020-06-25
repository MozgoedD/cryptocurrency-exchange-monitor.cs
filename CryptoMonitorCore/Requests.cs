using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CryptoMonitorCore
{
    public class Requests
    {
        public class GateIO
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("method")]
            public string Method { get; set; }
            [JsonProperty("params")]
            public ArrayList Params { get; set; }
        }
        public class Okex
        {
            [JsonProperty("op")]
            public string Op { get; set; }
            [JsonProperty("args")]
            public List<string> Args { get; set; }
        }
        public class Huobi
        {
            [JsonProperty("sub")]
            public string Sub { get; set; }
            [JsonProperty("id")]
            public int Id { get; set; }
        }
        public class HuobiPong
        {
            [JsonProperty("pong")]
            public long PongId { get; set; }
        }
    }
}
