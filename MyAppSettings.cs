using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyTrader
{
    public class MyAppSettings
    {
        public int AccountID { get; set; }

        public string AccountNumber { get; set; }

        public int InstrumentID { get; set; }

        public string InteractiveBrokersIP { get; set; }

        public int InteractiveBrokersPort { get; set; }
        public int MessagesServerPullPort { get; set; }
        public int StrategyID { get; set; }
        public string LogDirectory { get; set; }
        public int RequestSocketPort { get; set; }
        public int PushSocketPort { get; set; }
        public int RealTimeDataServerRequestPort{get;set;}
    public int RealTimeDataServerPublishPort{get;set;}
    public int HistoricalServerPort{get;set;}
    public string Host{get;set;}
    public int InstrumentUpdateRequestSocketPort{get;set;}
    public bool TradingOnBankingHoliday{get;set;}
 
    }
}
