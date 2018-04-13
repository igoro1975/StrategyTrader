using StrategyTrader.Logic;

namespace StrategyTrader
{
    public class StrategyLauncher
    {
        private IStrategy strategy;

        public StrategyLauncher(IbClient wrapper,  MyAppSettings settings, bool useMatlab = false)
        {
            if (useMatlab)
            {
               //InitMatlabStrategy();
            }
            else
            {
                InitNetStrategy(wrapper, settings);
            }
        }

        private void InitNetStrategy(IbClient wrapper, MyAppSettings settings)
        {
            strategy = new SimplestNetStrategy(wrapper, settings);
            strategy.StartTrading();
        }

        //private void InitMatlabStrategy()
        //{
        //    var activationContext = Type.GetTypeFromProgID("matlab.application.single");
        //    strategy = new MatlabStrategy((MLApp.MLApp)Activator.CreateInstance(activationContext));
        //    Task.Run(()=>strategy.StartTrading());

        //}
    }
}