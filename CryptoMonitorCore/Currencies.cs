using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoMonitorCore
{
    public static class Currencies
    {
        static List<string> gateSymbols = new List<string>() { "USDT-CNYX", "BTC-USDC", "BTC-PAX", "BTC-USDT", "BCH-USDT", "ETH-USDT", "ETC-USDT", "QTUM-USDT", "LTC-USDT", "DASH-USDT", "ZEC-USDT", "BTM-USDT", "EOS-USDT", "REQ-USDT", "SNT-USDT", "OMG-USDT", "PAY-USDT", "CVC-USDT", "ZRX-USDT", "TNT-USDT", "XMR-USDT", "XRP-USDT", "DOGE-USDT", "BAT-USDT", "PST-USDT", "BTG-USDT", "DPY-USDT", "LRC-USDT", "STORJ-USDT", "RDN-USDT", "STX-USDT", "KNC-USDT", "LINK-USDT", "CDT-USDT", "AE-USDT", "AE-ETH", "AE-BTC", "CDT-ETH", "RDN-ETH", "STX-ETH", "KNC-ETH", "LINK-ETH", "REQ-ETH", "RCN-ETH", "TRX-ETH", "ARN-ETH", "BNT-ETH", "VET-ETH", "MCO-ETH", "FUN-ETH", "DATA-ETH", "RLC-ETH", "RLC-USDT", "ZSC-ETH", "WINGS-ETH", "MDA-ETH", "RCN-USDT", "TRX-USDT", "VET-USDT", "MCO-USDT", "FUN-USDT", "DATA-USDT", "ZSC-USDT", "MDA-USDT", "XTZ-USDT", "XTZ-BTC", "XTZ-ETH", "GNT-USDT", "GNT-ETH", "GEM-USDT", "GEM-ETH", "RFR-USDT", "RFR-ETH", "DADI-USDT", "DADI-ETH", "ABT-USDT", "ABT-ETH", "LEDU-BTC", "LEDU-ETH", "OST-USDT", "OST-ETH", "XLM-USDT", "XLM-ETH", "XLM-BTC", "MOBI-USDT", "MOBI-ETH", "MOBI-BTC", "OCN-USDT", "OCN-ETH", "OCN-BTC", "ZPT-USDT", "ZPT-ETH", "ZPT-BTC", "COFI-USDT", "COFI-ETH", "JNT-USDT", "JNT-ETH", "JNT-BTC", "BLZ-USDT", "BLZ-ETH", "GXS-USDT", "GXS-BTC", "MTN-USDT", "MTN-ETH", "RUFF-USDT", "RUFF-ETH", "RUFF-BTC", "TNC-USDT", "TNC-ETH", "TNC-BTC", "ZIL-USDT", "ZIL-ETH", "BTO-USDT", "BTO-ETH", "THETA-USDT", "THETA-ETH", "DDD-USDT", "DDD-ETH", "DDD-BTC", "MKR-USDT", "MKR-ETH", "DAI-USDT", "SMT-USDT", "SMT-ETH", "MDT-USDT", "MDT-ETH", "MDT-BTC", "MANA-USDT", "MANA-ETH", "LUN-USDT", "LUN-ETH", "SALT-USDT", "SALT-ETH", "FUEL-USDT", "FUEL-ETH", "ELF-USDT", "ELF-ETH", "DRGN-USDT", "DRGN-ETH", "GTC-USDT", "GTC-ETH", "GTC-BTC", "QLC-USDT", "QLC-BTC", "QLC-ETH", "DBC-USDT", "DBC-BTC", "DBC-ETH", "BNTY-USDT", "BNTY-ETH", "LEND-USDT", "LEND-ETH", "ICX-USDT", "ICX-ETH", "BTF-USDT", "BTF-BTC", "ADA-USDT", "ADA-BTC", "LSK-USDT", "LSK-BTC", "WAVES-USDT", "WAVES-BTC", "BIFI-USDT", "BIFI-BTC", "MDS-ETH", "MDS-USDT", "DGD-USDT", "DGD-ETH", "QASH-USDT", "QASH-ETH", "QASH-BTC", "POWR-USDT", "POWR-ETH", "POWR-BTC", "FIL-USDT", "BCD-USDT", "BCD-BTC", "SBTC-USDT", "SBTC-BTC", "GOD-USDT", "GOD-BTC", "BCX-USDT", "BCX-BTC", "QSP-USDT", "QSP-ETH", "INK-BTC", "INK-USDT", "INK-ETH", "INK-QTUM", "QBT-QTUM", "QBT-ETH", "QBT-USDT", "TSL-QTUM", "TSL-USDT", "GNX-USDT", "GNX-ETH", "NEO-USDT", "GAS-USDT", "NEO-BTC", "GAS-BTC", "IOTA-USDT", "IOTA-BTC", "NAS-USDT", "NAS-ETH", "NAS-BTC", "ETH-BTC", "ETC-BTC", "ETC-ETH", "ZEC-BTC", "DASH-BTC", "LTC-BTC", "BCH-BTC", "BTG-BTC", "QTUM-BTC", "QTUM-ETH", "XRP-BTC", "DOGE-BTC", "XMR-BTC", "ZRX-BTC", "ZRX-ETH", "DNT-ETH", "DPY-ETH", "OAX-BTC", "OAX-USDT", "OAX-ETH", "REP-ETH", "LRC-ETH", "LRC-BTC", "PST-ETH", "BCDN-ETH", "BCDN-USDT", "TNT-ETH", "SNT-ETH", "SNT-BTC", "BTM-ETH", "BTM-BTC", "SNET-ETH", "SNET-USDT", "LLT-SNET", "OMG-ETH", "OMG-BTC", "PAY-ETH", "PAY-BTC", "BAT-ETH", "BAT-BTC", "CVC-ETH", "STORJ-ETH", "STORJ-BTC", "EOS-ETH", "EOS-BTC", "BTS-USDT", "BTS-BTC", "TIPS-ETH", "GT-BTC", "GT-USDT", "ATOM-BTC", "ATOM-USDT", "KAVA-USDT", "ANKR-USDT", "RSR-USDT", "RSV-USDT", "KAI-USDT", "COMP-USDT", "OCEAN-USDT", "DOT-USDT", "MTRG-USDT", "SOL-USDT", "COTI-USDT", "LBK-USDT", "BTMX-USDT", "XEM-ETH", "XEM-USDT", "XEM-BTC", "BU-USDT", "BU-ETH", "BU-BTC", "HNS-BTC", "HNS-USDT", "BTC3L-USDT", "BTC3S-USDT", "ETH3L-USDT", "ETH3S-USDT", "EOS3L-USDT", "EOS3S-USDT", "BSV3L-USDT", "BSV3S-USDT", "BCH3L-USDT", "BCH3S-USDT", "LTC3L-USDT", "LTC3S-USDT", "XTZ3L-USDT", "XTZ3S-USDT", "BCHSV-USDT", "BCHSV-BTC", "RVN-USDT", "RVC-USDT", "AR-USDT", "DCR-USDT", "DCR-BTC", "BCN-USDT", "BCN-BTC", "XMC-USDT", "XMC-BTC", "STEEM-USDT", "HIVE-USDT", "ATP-USDT", "ATP-ETH", "NAX-USDT", "NAX-ETH", "KLAY-USDT", "NBOT-ETH", "NBOT-USDT", "MED-USDT", "MED-ETH", "GRIN-USDT", "GRIN-ETH", "GRIN-BTC", "BEAM-USDT", "BEAM-ETH", "BEAM-BTC", "HBAR-USDT", "OKB-USDT", "VTHO-ETH", "VTHO-USDT", "BTT-USDT", "BTT-ETH", "BTT-TRX", "TFUEL-ETH", "TFUEL-USDT", "CELR-ETH", "CELR-USDT", "CS-ETH", "CS-USDT", "MAN-ETH", "MAN-USDT", "REM-ETH", "REM-USDT", "LYM-ETH", "LYM-BTC", "LYM-USDT", "ONG-ETH", "ONG-USDT", "ONT-ETH", "ONT-USDT", "BFT-ETH", "BFT-USDT", "IHT-ETH", "IHT-USDT", "SENC-ETH", "SENC-USDT", "TOMO-ETH", "TOMO-USDT", "ELEC-ETH", "ELEC-USDT", "HAV-ETH", "HAV-USDT", "SWTH-ETH", "SWTH-USDT", "NKN-ETH", "NKN-USDT", "SOUL-ETH", "SOUL-USDT", "LRN-ETH", "LRN-USDT", "EOSDAC-ETH", "EOSDAC-USDT", "DOCK-USDT", "DOCK-ETH", "GSE-USDT", "GSE-ETH", "RATING-USDT", "RATING-ETH", "HSC-USDT", "HSC-ETH", "HIT-USDT", "HIT-ETH", "DX-USDT", "DX-ETH", "CNNS-ETH", "CNNS-USDT", "DREP-ETH", "DREP-USDT", "MBL-USDT", "MBL-ETH", "GMAT-USDT", "GMAT-ETH", "MIX-USDT", "MIX-ETH", "LAMB-USDT", "LAMB-ETH", "LEO-USDT", "LEO-BTC", "BTCBULL-USDT", "BTCBEAR-USDT", "ETHBEAR-USDT", "ETHBULL-USDT", "EOSBULL-USDT", "EOSBEAR-USDT", "XRPBEAR-USDT", "XRPBULL-USDT", "WICC-USDT", "WICC-ETH", "SERO-USDT", "SERO-ETH", "VIDY-USDT", "VIDY-ETH", "KGC-USDT", "FTM-USDT", "FTM-ETH", "COS-USDT", "CRO-USDT", "ALY-USDT", "WIN-USDT", "MTV-USDT", "ONE-USDT", "ARPA-USDT", "ARPA-ETH", "DILI-USDT", "ALGO-USDT", "PI-USDT", "CKB-USDT", "CKB-BTC", "CKB-ETH", "BKC-USDT", "BXC-USDT", "BXC-ETH", "PAX-USDT", "USDC-USDT", "TUSD-USDT", "HC-USDT", "HC-BTC", "HC-ETH", "GARD-USDT", "GARD-ETH", "FTI-USDT", "FTI-ETH", "SOP-ETH", "SOP-USDT", "LEMO-USDT", "LEMO-ETH", "QKC-USDT", "QKC-ETH", "QKC-BTC", "IOTX-USDT", "IOTX-ETH", "RED-USDT", "RED-ETH", "LBA-USDT", "LBA-ETH", "OPEN-USDT", "OPEN-ETH", "MITH-USDT", "MITH-ETH", "SKM-USDT", "SKM-ETH", "XVG-USDT", "XVG-BTC", "NANO-USDT", "NANO-BTC", "HT-USDT", "BNB-USDT", "MET-ETH", "MET-USDT", "TCT-ETH", "TCT-USDT", "MXC-USDT", "MXC-BTC", "MXC-ETH" };
        static List<string> okexSymbols = new List<string>() { "XPO-USDT", "SPND-BTC", "ROAD-USDK", "RVN-USDT", "HDAO-USDK", "BAT-USDT", "OXT-USDT", "OXT-BTC", "BCH-BTC", "BSV-BTC", "USDC-BTC", "DASH-BTC", "ADA-BTC", "AE-BTC", "ALGO-BTC", "APIX-BTC", "APM-BTC", "ARDR-BTC", "ATOM-BTC", "BAT-BTC", "BHP-BTC", "BTT-BTC", "CELO-BTC", "COMP-BTC", "CRO-BTC", "CTC-BTC", "CTXC-BTC", "CVT-BTC", "DCR-BTC", "DNA-BTC", "EGT-BTC", "GUSD-BTC", "HBAR-BTC", "HYC-BTC", "IQ-BTC", "KAN-BTC", "LBA-BTC", "LEO-BTC", "LET-BTC", "LSK-BTC", "ORS-BTC", "PAX-BTC", "PMA-BTC", "RVN-BTC", "SC-BTC", "TMTG-BTC", "TUSD-BTC", "VITE-BTC", "VSYS-BTC", "WAVES-BTC", "WXT-BTC", "XTZ-BTC", "YOU-BTC", "ZIL-BTC", "XRP-BTC", "ELF-BTC", "LRC-BTC", "MCO-BTC", "NULS-BTC", "BCX-BTC", "CMT-BTC", "ITC-BTC", "SBTC-BTC", "ZEC-BTC", "NEO-BTC", "GAS-BTC", "HC-BTC", "QTUM-BTC", "IOTA-BTC", "EOS-BTC", "SNT-BTC", "OMG-BTC", "LTC-BTC", "ETH-BTC", "ETC-BTC", "BCD-BTC", "BTG-BTC", "ACT-BTC", "PAY-BTC", "BTM-BTC", "GNT-BTC", "LINK-BTC", "WTC-BTC", "ZRX-BTC", "BNT-BTC", "CVC-BTC", "MANA-BTC", "KNC-BTC", "GNX-BTC", "ICX-BTC", "XEM-BTC", "ARK-BTC", "YOYO-BTC", "FUN-BTC", "TRX-BTC", "DGB-BTC", "SWFTC-BTC", "XMR-BTC", "XLM-BTC", "KCASH-BTC", "MDT-BTC", "NAS-BTC", "AAC-BTC", "VIB-BTC", "QUN-BTC", "INT-BTC", "IOST-BTC", "MOF-BTC", "TCT-BTC", "THETA-BTC", "PST-BTC", "SNC-BTC", "MKR-BTC", "TRUE-BTC", "SOC-BTC", "ZEN-BTC", "NANO-BTC", "GTO-BTC", "CHAT-BTC", "MITH-BTC", "ABT-BTC", "TRIO-BTC", "ONT-BTC", "OKB-BTC", "ADA-ETH", "AE-ETH", "ALGO-ETH", "ATOM-ETH", "BTT-ETH", "CTXC-ETH", "EGT-ETH", "HYC-ETH", "KAN-ETH", "LEO-ETH", "ORS-ETH", "SC-ETH", "WAVES-ETH", "YOU-ETH", "ZIL-ETH", "ELF-ETH", "LTC-ETH", "CMT-ETH", "LRC-ETH", "MCO-ETH", "NULS-ETH", "STORJ-ETH", "BTM-ETH", "EOS-ETH", "OMG-ETH", "DASH-ETH", "XRP-ETH", "ZEC-ETH", "NEO-ETH", "GAS-ETH", "HC-ETH", "QTUM-ETH", "IOTA-ETH", "ETC-ETH", "LINK-ETH", "WTC-ETH", "ZRX-ETH", "CVC-ETH", "MANA-ETH", "GNX-ETH", "XEM-ETH", "TRX-ETH", "SWFTC-ETH", "XMR-ETH", "XLM-ETH", "KCASH-ETH", "MDT-ETH", "NAS-ETH", "AAC-ETH", "FAIR-ETH", "TOPC-ETH", "INT-ETH", "IOST-ETH", "MOF-ETH", "MKR-ETH", "TRUE-ETH", "ZEN-ETH", "NANO-ETH", "GTO-ETH", "MITH-ETH", "ABT-ETH", "TRIO-ETH", "ONT-ETH", "OKB-ETH", "BTC-USDK", "LTC-USDK", "ETH-USDK", "OKB-USDK", "ETC-USDK", "BCH-USDK", "EOS-USDK", "XRP-USDK", "TRX-USDK", "BSV-USDK", "USDT-USDK", "ALGO-USDK", "CRO-USDK", "DEP-USDK", "DOGE-USDK", "EC-USDK", "EM-USDK", "FSN-USDK", "FTM-USDK", "HBAR-USDK", "LAMB-USDK", "LEO-USDK", "NDN-USDK", "ORBS-USDK", "PLG-USDK", "PMA-USDK", "VSYS-USDK", "WGRT-USDK", "WXT-USDK", "BCH-USDT", "BSV-USDT", "USDC-USDT", "ADA-USDT", "AE-USDT", "ALGO-USDT", "ALV-USDT", "APIX-USDT", "APM-USDT", "ATOM-USDT", "BHP-USDT", "BLOC-USDT", "BTT-USDT", "CELO-USDT", "COMP-USDT", "CRO-USDT", "CTC-USDT", "CTXC-USDT", "CVT-USDT", "DAI-USDT", "DCR-USDT", "DEP-USDT", "DMG-USDT", "DNA-USDT", "DOGE-USDT", "DOT-USDT", "EC-USDT", "EGT-USDT", "EM-USDT", "ETM-USDT", "FSN-USDT", "FTM-USDT", "GUSD-USDT", "HBAR-USDT", "HDAO-USDT", "HYC-USDT", "IQ-USDT", "KAN-USDT", "LAMB-USDT", "LBA-USDT", "LEO-USDT", "LET-USDT", "LSK-USDT", "NDN-USDT", "ORBS-USDT", "ORS-USDT", "PAX-USDT", "PLG-USDT", "ROAD-USDT", "SC-USDT", "TMTG-USDT", "TUSD-USDT", "VSYS-USDT", "WAVES-USDT", "WGRT-USDT", "WXT-USDT", "XTZ-USDT", "YOU-USDT", "ZIL-USDT", "TRX-OKB", "EGT-OKB", "SC-OKB", "WXT-OKB", "BTC-DAI", "ETH-DAI", "ELF-USDT", "DASH-USDT", "BTG-USDT", "LRC-USDT", "MCO-USDT", "NULS-USDT", "DASH-OKB", "XRP-USDT", "ZEC-USDT", "NEO-USDT", "GAS-USDT", "HC-USDT", "QTUM-USDT", "IOTA-USDT", "BTC-USDT", "BCD-USDT", "XUC-USDT", "CMT-USDT", "ITC-USDT", "ETH-USDT", "LTC-USDT", "ETC-USDT", "EOS-USDT", "OMG-USDT", "ACT-USDT", "BTM-USDT", "GNT-USDT", "PAY-USDT", "STORJ-USDT", "SNT-USDT", "LINK-USDT", "WTC-USDT", "ZRX-USDT", "BNT-USDT", "CVC-USDT", "MANA-USDT", "KNC-USDT", "ICX-USDT", "XEM-USDT", "ARK-USDT", "YOYO-USDT", "AST-USDT", "TRX-USDT", "MDA-USDT", "DGB-USDT", "PPT-USDT", "SWFTC-USDT", "XMR-USDT", "XLM-USDT", "KCASH-USDT", "MDT-USDT", "NAS-USDT", "RNT-USDT", "AAC-USDT", "FAIR-USDT", "UBTC-USDT", "VIB-USDT", "UTK-USDT", "TOPC-USDT", "QUN-USDT", "INT-USDT", "IOST-USDT", "YEE-USDT", "MOF-USDT", "TCT-USDT", "THETA-USDT", "PST-USDT", "MKR-USDT", "TRUE-USDT", "SOC-USDT", "ZEN-USDT", "ZIP-USDT", "NANO-USDT", "GTO-USDT", "CHAT-USDT", "BEC-USDT", "MITH-USDT", "ABT-USDT", "TRIO-USDT", "ONT-USDT", "OKB-USDT", "NEO-OKB", "LTC-OKB", "ETC-OKB", "XRP-OKB", "ZEC-OKB", "IOTA-OKB", "EOS-OKB", "BTC-USDC", "LTC-USDC", "ETH-USDC", "OKB-USDC", "ETC-USDC", "BCH-USDC", "EOS-USDC", "XRP-USDC", "TRX-USDC", "BSV-USDC" };
        static List<string> huobiSymbols = new List<string>() { "POWR-ETH", "LBA-USDT", "MT-HT", "BTM-ETH", "LAMB-HT", "CKB-HT", "IOTA-BTC", "HC-BTC", "OMG-ETH", "SBTC-BTC", "OGN-BTC", "NULS-ETH", "STEEM-BTC", "WAVES-USDT", "LOL-USDT", "EKO-ETH", "NODE-USDT", "YEE-BTC", "DCR-USDT", "BAT-USDT", "XMR-USDT", "QUN-BTC", "XMX-ETH", "AKRO-HT", "TNB-BTC", "FAIR-ETH", "BHD-USDT", "WICC-BTC", "CMT-ETH", "XLM-ETH", "LET-USDT", "WTC-ETH", "TRX-USDT", "SALT-BTC", "ATOM-BTC", "GT-USDT", "UIP-USDT", "SC-ETH", "CRE-USDT", "ADA-HUSD", "IOST-USDT", "ARDR-ETH", "AIDOC-BTC", "DASH-HT", "FSN-BTC", "QTUM-BTC", "YCC-BTC", "ICX-BTC", "SHE-BTC", "BCH-USDT", "GRS-BTC", "GSC-ETH", "BKBT-ETH", "BHT-HT", "MX-BTC", "MTX-BTC", "OCN-USDT", "THETA-BTC", "NCASH-BTC", "TNT-BTC", "EKT-ETH", "SWFTC-BTC", "DASH-HUSD", "HIVE-USDT", "LTC-HT", "ARPA-BTC", "ATP-USDT", "KAN-USDT", "KNC-USDT", "SEELE-USDT", "BUT-BTC", "KAN-ETH", "ITC-USDT", "PROPY-BTC", "CHAT-BTC", "AAC-USDT", "EVX-ETH", "EGT-HT", "LSK-ETH", "CVNT-ETH", "HT-ETH", "WAXP-BTC", "RUFF-BTC", "PAY-BTC", "XZC-BTC", "POLY-ETH", "KCASH-BTC", "NEXO-ETH", "WXT-BTC", "MEX-BTC", "FTI-ETH", "XZC-USDT", "NAS-ETH", "ATP-BTC", "SRN-ETH", "EDU-BTC", "BTS-USDT", "CRO-USDT", "GNX-BTC", "KMD-ETH", "SMT-USDT", "BTT-TRX", "AAC-BTC", "XRP-USDT", "KNC-BTC", "VET-ETH", "TOPC-BTC", "CNN-BTC", "BTC-USDT", "CTXC-BTC", "NCC-BTC", "DATX-BTC", "SMT-ETH", "AE-ETH", "MANA-ETH", "LINK-ETH", "WICC-USDT", "WAVES-ETH", "COVA-BTC", "PAI-BTC", "AST-BTC", "SEELE-ETH", "PAI-USDT", "TOS-BTC", "SSP-ETH", "UTK-BTC", "GTC-ETH", "WAN-BTC", "CVC-ETH", "USDT-HUSD", "XTZ-BTC", "CVCOIN-BTC", "RVN-BTC", "CRO-BTC", "BFT-BTC", "TOPC-ETH", "OGN-USDT", "PVT-HT", "LOL-BTC", "GAS-BTC", "VET-USDT", "DOT-USDT", "NANO-BTC", "ELA-USDT", "DCR-ETH", "TOS-ETH", "LXT-ETH", "TOP-HT", "EKT-USDT", "AE-USDT", "TT-USDT", "ZIL-BTC", "TT-HT", "BTG-BTC", "WAVES-BTC", "REN-USDT", "HOT-BTC", "GXC-USDT", "LOOM-BTC", "FTI-BTC", "IDT-ETH", "DTA-USDT", "RVN-USDT", "HC-USDT", "NAS-USDT", "SKM-HT", "TOP-USDT", "RTE-ETH", "DASH-BTC", "ALGO-BTC", "LUN-BTC", "GNT-ETH", "KMD-BTC", "LYM-BTC", "OCN-BTC", "BLZ-BTC", "MEET-BTC", "RUFF-ETH", "SKM-USDT", "ELA-BTC", "TRX-ETH", "UGAS-BTC", "CKB-BTC", "ETC-HUSD", "ZEC-USDT", "VSYS-USDT", "PVT-BTC", "HT-USDT", "XMR-ETH", "EM-HT", "ELA-ETH", "EM-USDT", "ADA-BTC", "IDT-BTC", "FOR-USDT", "KNC-ETH", "ETC-BTC", "HOT-ETH", "AAC-ETH", "DAT-BTC", "ITC-ETH", "VSYS-HT", "TRX-BTC", "XEM-USDT", "DAT-ETH", "LOOM-ETH", "UC-BTC", "STEEM-USDT", "XMR-BTC", "WTC-BTC", "HT-HUSD", "NKN-USDT", "QTUM-USDT", "BTT-USDT", "ZLA-BTC", "CNNS-USDT", "ICX-ETH", "DBC-BTC", "KCASH-USDT", "ATOM-USDT", "UC-ETH", "IRIS-USDT", "BSV-HUSD", "ITC-BTC", "ALGO-ETH", "RUFF-USDT", "BOX-BTC", "BCV-BTC", "ARPA-USDT", "PC-BTC", "DOGE-BTC", "CNNS-HT", "PHX-BTC", "CRO-HT", "VIDY-HT", "BSV-BTC", "DOGE-USDT", "DBC-ETH", "NULS-USDT", "NKN-HT", "CNNS-BTC", "STEEM-ETH", "IRIS-ETH", "WTC-USDT", "BCX-BTC", "GXC-ETH", "LXT-BTC", "BTT-BTC", "XEM-BTC", "BTM-USDT", "ENG-BTC", "ONT-BTC", "XVG-ETH", "OMG-USDT", "BCV-ETH", "STORJ-USDT", "DOGE-ETH", "FOR-HT", "ETC-HT", "UGAS-ETH", "LYM-ETH", "OCN-ETH", "SNT-BTC", "DTA-ETH", "BLZ-ETH", "APPC-BTC", "ONT-USDT", "ZIL-ETH", "DTA-BTC", "PVT-USDT", "PC-ETH", "SKM-BTC", "XVG-BTC", "VSYS-BTC", "OMG-BTC", "BOX-ETH", "RTE-BTC", "TT-BTC", "POWR-BTC", "REN-ETH", "APPC-ETH", "STORJ-BTC", "GXC-BTC", "BTT-ETH", "ONE-BTC", "EDU-ETH", "GRS-ETH", "KCASH-ETH", "EKT-BTC", "XRP-HUSD", "IRIS-BTC", "MTX-ETH", "TOP-BTC", "CKB-USDT", "NAS-BTC", "AE-BTC", "DGD-BTC", "LAMB-USDT", "VET-BTC", "ZIL-USDT", "EOS-HUSD", "REN-BTC", "XLM-BTC", "SNT-USDT", "BUT-ETH", "SOC-USDT", "GVE-BTC", "ONT-ETH", "ATOM-ETH", "EVX-BTC", "ZRX-USDT", "PORTAL-ETH", "QUN-ETH", "CVNT-BTC", "HB10-USDT", "PROPY-ETH", "QTUM-ETH", "SHE-ETH", "ETH-BTC", "EM-BTC", "OGO-USDT", "LOL-HT", "ARDR-BTC", "DOCK-USDT", "BKBT-BTC", "GSC-BTC", "EKO-BTC", "WAXP-ETH", "BIX-BTC", "HT-BTC", "OST-BTC", "BCH-HUSD", "LXT-USDT", "NKN-BTC", "LTC-USDT", "ATP-HT", "ADA-USDT", "MTN-BTC", "NODE-HT", "HIVE-HT", "BTM-BTC", "BSV-USDT", "XRP-BTC", "DASH-USDT", "ALGO-USDT", "THETA-ETH", "NULS-BTC", "BHT-USDT", "XLM-HUSD", "FOR-BTC", "EGT-USDT", "WAN-ETH", "MANA-USDT", "ARPA-HT", "USDC-HUSD", "FAIR-BTC", "ZEC-HUSD", "MEET-ETH", "WICC-ETH", "CTXC-ETH", "DATX-ETH", "NEW-HT", "BIFI-BTC", "ETN-BTC", "LINK-BTC", "ADA-ETH", "GNX-ETH", "HC-ETH", "VIDY-BTC", "UTK-ETH", "REQ-BTC", "ACT-ETH", "HIVE-BTC", "MANA-BTC", "IOST-HT", "DOCK-BTC", "GTC-BTC", "POLY-BTC", "EGT-BTC", "BFT-ETH", "XTZ-ETH", "NEXO-BTC", "AKRO-BTC", "CVC-BTC", "FSN-HT", "ETC-USDT", "BHD-HT", "CTXC-USDT", "GNT-USDT", "AKRO-USDT", "QASH-BTC", "NANO-USDT", "BIX-USDT", "GAS-ETH", "GNT-BTC", "NANO-ETH", "HPT-HT", "BTC-HUSD", "ACT-USDT", "TUSD-HUSD", "DCR-BTC", "XZC-ETH", "SOC-ETH", "CNN-ETH", "ZEC-BTC", "XTZ-USDT", "QSP-BTC", "SSP-BTC", "SMT-BTC", "VIDY-USDT", "LSK-BTC", "COVA-ETH", "WXT-USDT", "SEELE-BTC", "ZJLT-BTC", "SRN-BTC", "BTS-ETH", "DGB-ETH", "IOTA-USDT", "QASH-ETH", "IOST-ETH", "GET-ETH", "TNB-USDT", "IIC-ETH", "MUSK-ETH", "DOCK-ETH", "SOC-BTC", "ETH-USDT", "LAMB-BTC", "NODE-BTC", "ONE-HT", "ETN-ETH", "LBA-BTC", "18C-BTC", "ACT-BTC", "CHAT-ETH", "MCO-ETH", "MXC-BTC", "LTC-BTC", "PAI-ETH", "KAN-BTC", "OGN-HT", "HPT-BTC", "PAY-ETH", "FSN-USDT", "MDS-USDT", "RCCC-BTC", "BHT-BTC", "RVN-HT", "LET-ETH", "MX-HT", "MCO-BTC", "RSR-HT", "THETA-USDT", "UUU-USDT", "BHD-BTC", "FTT-HT", "SNC-BTC", "BCD-BTC", "MUSK-BTC", "LAMB-ETH", "RCCC-ETH", "LET-BTC", "ZEN-BTC", "OGO-BTC", "KCASH-HT", "CVC-USDT", "MX-USDT", "ZRX-ETH", "PAX-HUSD", "REQ-ETH", "ZJLT-ETH", "BTS-BTC", "BIX-ETH", "IOST-BTC", "DAC-USDT", "XLM-USDT", "SC-BTC", "CRE-BTC", "TRIO-BTC", "LINK-USDT", "NEO-BTC", "LBA-ETH", "AIDOC-ETH", "UIP-BTC", "GT-BTC", "ZEN-ETH", "GET-BTC", "ZRX-BTC", "FTT-USDT", "ELF-BTC", "ELF-USDT", "MTL-BTC", "EOS-HT", "YEE-ETH", "EOS-USDT", "WPR-BTC", "ETH-HUSD", "NEW-BTC", "LTC-HUSD", "SWFTC-ETH", "BAT-ETH", "PORTAL-BTC", "MAN-BTC", "NEW-USDT", "TRIO-ETH", "XMX-BTC", "BCH-HT", "UIP-ETH", "XRP-HT", "TNB-ETH", "NEO-USDT", "SNC-ETH", "BCH-BTC", "STK-ETH", "YCC-ETH", "FTT-BTC", "DAC-BTC", "STK-BTC", "RCN-BTC", "18C-ETH", "CRE-HT", "EOS-BTC", "IOTA-ETH", "DGD-ETH", "HIT-BTC", "RSR-USDT", "ABT-ETH", "GT-HT", "MDS-ETH", "CMT-BTC", "ADX-BTC", "IIC-BTC", "UUU-BTC", "RSR-BTC", "HIT-USDT", "WXT-HT", "EOS-ETH", "MT-ETH", "DAC-ETH", "DGB-BTC", "PNT-ETH", "OGO-HT", "MAN-ETH", "EGCC-BTC", "YEE-USDT", "HIT-ETH", "ELF-ETH", "RDN-BTC", "ABT-BTC", "NPXS-BTC", "PNT-BTC", "MT-BTC", "UUU-ETH", "CMT-USDT", "NPXS-ETH", "ONE-USDT", "EGCC-ETH", "MDS-BTC", "HPT-USDT", "BAT-BTC"};

        public static List<string> gateReadyToGo = new List<string>();
        public static List<string> okexReadyToGo = new List<string>();
        public static List<string> huobiReadyToGo = new List<string>();

        public static IEnumerable<string> gate_okex_Intersect;
        public static IEnumerable<string> okex_huobiIntersect;
        public static IEnumerable<string> gate_huobiIntersect;
        public static IEnumerable<string> gate_okex_huobiIntersect;

        public static List<string> symbolsInSettings = new List<string>();

        public static void checkSymbolsForAvailabilityInExchanges(Settings setting)
        {
            foreach (var SymSettingList in setting.Symbols)
            {
                symbolsInSettings.Add($"{SymSettingList[0]}-{SymSettingList[1]}");

                if (gateSymbols.Contains($"{SymSettingList[0]}-{SymSettingList[1]}"))
                {
                    gateReadyToGo.Add($"{SymSettingList[0]}-{SymSettingList[1]}");
                }
                if (okexSymbols.Contains($"{SymSettingList[0]}-{SymSettingList[1]}"))
                {
                    okexReadyToGo.Add($"{SymSettingList[0]}-{SymSettingList[1]}");
                }
                if (huobiSymbols.Contains($"{SymSettingList[0]}-{SymSettingList[1]}"))
                {
                    huobiReadyToGo.Add($"{SymSettingList[0]}-{SymSettingList[1]}");
                }
            }
            //foreach (var SymSettingList in setting.Symbols)
            //{
            //    if (!gateReadyToGo.Contains($"{SymSettingList[0]}-{SymSettingList[1]}"))
            //    {
            //        Console.WriteLine($"GateIO exchange does not support this symbol: {SymSettingList[0]}-{SymSettingList[1]}");
            //    }
            //    if (!okexReadyToGo.Contains($"{SymSettingList[0]}-{SymSettingList[1]}"))
            //    {
            //        Console.WriteLine($"Okex exchange does not support this symbol: {SymSettingList[0]}-{SymSettingList[1]}");
            //    }
            //    if (!huobiReadyToGo.Contains($"{SymSettingList[0]}-{SymSettingList[1]}"))
            //    {
            //        Console.WriteLine($"Huobi exchange does not support this symbol: {SymSettingList[0]}-{SymSettingList[1]}");
            //    }
            //}          
        }

        public static void formedPairs()
        {
            gate_okex_Intersect = gateReadyToGo.Intersect(okexReadyToGo);
            okex_huobiIntersect = okexReadyToGo.Intersect(huobiReadyToGo);
            gate_huobiIntersect = gateReadyToGo.Intersect(huobiReadyToGo);
            gate_okex_huobiIntersect = gate_okex_Intersect.Intersect(huobiReadyToGo);

            foreach (var symbol in symbolsInSettings)
            {
                if (gate_okex_huobiIntersect.Contains(symbol))
                {
                    Console.WriteLine($"{symbol} available for gate.io – okex, okex – huobi, gate.io – huobi");
                }
                else if (gate_okex_Intersect.Contains(symbol))
                {
                    Console.WriteLine($"{symbol} available for gate.io – okex");
                }
                else if (okex_huobiIntersect.Contains(symbol))
                {
                    Console.WriteLine($"{symbol} available for okex – huobi");
                }
                else if (gate_huobiIntersect.Contains(symbol))
                {
                    Console.WriteLine($"{symbol} available for gate.io – huobi");
                }
            }

            //foreach (var symbol in gateSymbols)
            //{
            //    if (huobiSymbols.Contains(symbol))
            //    {
            //        if (!okexSymbols.Contains(symbol))
            //        {
            //            Console.WriteLine($"{symbol} gate-huobi");

            //        }
            //    }                     
            //}
            //foreach (var symbol in gate_okex_Intersect)
            //{
            //    Console.WriteLine($"{symbol} available for gate.io – okex");
            //}

            //foreach (var symbol in okex_huobiIntersect)
            //{
            //    Console.WriteLine($"{symbol} available for okex – huobi");
            //}

            //foreach (var symbol in gate_huobiIntersect)
            //{
            //    Console.WriteLine($"{symbol} available for gate – huobi");
            //}
        }

        public static decimal getMinVolume(string coin, decimal lotMinUsd, HttpClient client)
        {
            if (gateSymbols.Contains($"{coin.ToUpper()}-USDT"))
            {
                var responseString = client.GetStringAsync($"https://data.gateio.la/api2/1/ticker/{coin.ToLower()}_usdt");
                JObject jsonObj = JObject.Parse(responseString.Result);
                decimal coinUSDT = Convert.ToDecimal(jsonObj["last"], CultureInfo.InvariantCulture);
                decimal minVolume = lotMinUsd / coinUSDT;
                return minVolume;
            }
            else if (okexSymbols.Contains($"{coin.ToUpper()}-USDT"))
            {
                var responseString = client.GetStringAsync($"https://www.okex.com/api/spot/v3/instruments/{coin.ToUpper()}-USDT/ticker");
                JObject jsonObj = JObject.Parse(responseString.Result);
                decimal coinUSDT = Convert.ToDecimal(jsonObj["last"], CultureInfo.InvariantCulture);
                decimal minVolume = lotMinUsd / coinUSDT;
                return minVolume;
            }
            else if (huobiSymbols.Contains($"{coin.ToUpper()}-USDT"))
            {
                var responseString = client.GetStringAsync($"https://api.huobi.pro/market/detail?symbol={coin.ToLower()}usdt");
                JObject jsonObj = JObject.Parse(responseString.Result);
                decimal coinUSDT = Convert.ToDecimal(jsonObj["tick"]["close"], CultureInfo.InvariantCulture);
                decimal minVolume = lotMinUsd / coinUSDT;
                return minVolume;
            }
            else
            {
                Console.WriteLine($"Coin {coin}-USDT pair was not found in any exchange");
                return 0m;
            }
        }

        public static void getHuobiSymbols(HttpClient client)
        {
            var responseString = client.GetStringAsync($"https://api.huobi.pro/v1/common/symbols");
            JObject jsonObj = JObject.Parse(responseString.Result);
            List<JObject> data = jsonObj["data"].ToObject<List<JObject>>();
            foreach(JObject partialData in data)
            {
                if (partialData["state"].ToString() == "online")
                {
                    Console.Write($"\"{partialData["base-currency"].ToString().ToUpper()}-{partialData["quote-currency"].ToString().ToUpper()}\", ");
                }
                else { continue;  }
            }
        }

        public static void getOkexSymbolss(HttpClient client)
        {
            var responseString = client.GetStringAsync($"https://www.okex.com/api/spot/v3/instruments");

            List<JObject> jsonObjList = JsonConvert.DeserializeObject<List<JObject>>(responseString.Result);
            foreach (var jsonObj in jsonObjList)
            {
                Console.Write($"\"{jsonObj["instrument_id"]}\", ");
            }

            //foreach(var JsonEl in jsonObj)
            //{
            //    Console.WriteLine(JsonEl);
            //}
            //Console.WriteLine(responseString.Result);
            //Console.WriteLine(jsonObj[0]["instrument_id"].ToString());
        }
    }
}
