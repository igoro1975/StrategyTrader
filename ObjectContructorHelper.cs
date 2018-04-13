using Common.EntityModels;
using IBApi;
using System;
using Common.Enums;

namespace StrategyTrader
{
    public class ObjectConstructorHelper
    {
        public static int AccountID;
        public static int InstrumentID;

        public static ExecutionMessage GetExecutionMessage(int reqId, Contract contract,
                                                         Execution execution, int accountID, int instrumentID)
        {
            return new ExecutionMessage
            {
                RequestId = reqId,
                ExecutionId = execution.ExecId,
                PermanentId = execution.PermId,
                InstrumentID = instrumentID,
                AccountID = accountID,
                Quantity = (int)execution.CumQty,
                Side = execution.Side,
                OrderId = execution.OrderId,
                Price = new decimal(execution.Price),
                Time = DateTime.Now
            };
        }

        

        public static OpenOrder GetOpenOrder(Contract contract, Order order, OrderState orderState, int accountID, int instrumentID)
        {
            return new OpenOrder
            {
                PermanentId = order.PermId,
                AccountID = accountID,
                InstrumentID = instrumentID,
                Status = orderState.Status,
                LimitPrice = (decimal)order.LmtPrice,
                Quantity = (decimal)order.TotalQuantity,
                Type = order.OrderType,
                OrderId=order.OrderId
            };
        }

        public static OrderStatusMessage GetOrderStatusMessage(int orderId, string status, int filled, int remaining, double averageFillPrice,
            int permanentId, int parentId, double lastFillPrice, int clientId, string whyHeld)
        {
            return new OrderStatusMessage
            {
                OrderId = orderId,
                Status = status,
                Filled = filled,
                Remaining = remaining,
                AverageFillPrice = new decimal(averageFillPrice),
                PermanentId = permanentId,
                ParentId = parentId,
                LastFillPrice = new decimal(lastFillPrice),
                ClientId = clientId,
                WhyHeld = whyHeld
            };
        }

        internal static object GetLiveTrade(double position, double marketPrice, double averageCost, double unrealisedPnl, double realisedPnl,int accountID, int instrumentID)
        {
            return new LiveTrade()
            {
                Quantity = new decimal(Math.Abs(position)),
                MarketPrice = new decimal(marketPrice),
                AveragePrice = new decimal(averageCost),
                UnrealizedPnL = new decimal(unrealisedPnl),
                RealizedPnl = new decimal(realisedPnl),
                TradeDirection = position>0 ? TradeDirection.Long  : TradeDirection.Short,
                AccountID= accountID,
                InstrumentID = instrumentID,
                UpdateTime = DateTime.Now



            };
        }

        
    }
}