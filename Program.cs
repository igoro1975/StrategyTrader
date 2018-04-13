using IBApi;

using NLog;
using NLog.Targets;

using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RequestsClientStandard;


namespace StrategyTrader
{
    public class Program
    {
        public static int AccountID;
        
        private static IbClient wrapper;
        public static IConfigurationRoot ConfigurationRoot;
        private static MyAppSettings MyAppSettings;

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            ConfigurationRoot = builder.Build();
            var app = new MyAppSettings();
            ///ConfigurationRoot
            ConfigurationRoot.Bind("AppSettings", app);
            app.AccountID = 4;
            ConfigurationRoot.Reload();;
            MyAppSettings MyAppSettings = app;

            AccountID = MyAppSettings.AccountID;
            SetAndCreateLogDirectory(MyAppSettings.LogDirectory);
            MappingConfiguration.Register();
            GetAccountNumberAndAccountID();
            ConnectToIb();
            new StrategyLauncher(wrapper, MyAppSettings);
        }

        private static void SetAndCreateLogDirectory(string logDirectory)
        {
            if (Directory.Exists(logDirectory))
            {
                ((FileTarget)LogManager.Configuration.FindTargetByName("logfile")).FileName =
                   logDirectory + "Log.log";
            }
            else
            {
                Directory.CreateDirectory(logDirectory);
                ((FileTarget)LogManager.Configuration.FindTargetByName("logfile")).FileName =
                    logDirectory + "Log.log";
            }
        }

        private static void ConnectToIb()
        {

            RequestsClient client = new RequestsClient(
                MyAppSettings.PushSocketPort, MyAppSettings.RequestSocketPort);
            client.Connect();
            
            

            wrapper = new IbClient(client, MyAppSettings);
            EClientSocket clientSocket = wrapper.ClientSocket;
            EReaderSignal readerSignal = wrapper.Signal;

            clientSocket.eConnect(MyAppSettings.InteractiveBrokersIP, MyAppSettings.InteractiveBrokersPort, 0);
           
            //Create a reader to consume messages from the TWS. The EReader will consume the incoming messages and put them in a queue
            var reader = new EReader(clientSocket, readerSignal);
            reader.Start();
            //Once the messages are in the queue, an additional thread need to fetch them
            new Thread(() =>
            {
                while (clientSocket.IsConnected())
                {
                    readerSignal.waitForSignal();
                    reader.processMsgs();
                }
            })
            { IsBackground = true }.Start();

            while (wrapper.NextOrderId <= 0) { }
            
           
            
            
        }

        private static void GetAccountNumberAndAccountID()
        {
            var requestClient =  new RequestsClient(
                MyAppSettings.PushSocketPort, MyAppSettings.RequestSocketPort);
            var acc = requestClient.RequestAccount(MyAppSettings.AccountID);
            if (acc == null)
            {
                throw new ArgumentNullException("No account info received.");
            }
            requestClient.Dispose();

            ConfigurationRoot.GetSection("AccountNumber").Value = acc.AccountNumber;
            ConfigurationRoot.GetSection("AccountID").Value = acc.ID.ToString();
            ConfigurationRoot.Reload();
            MyAppSettings = JsonConvert.DeserializeObject<MyAppSettings>(ConfigurationRoot.GetSection("MyAppSettings").Value);



        }

        

        
    }
}