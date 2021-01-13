using System;
using System.ServiceModel;
using System.Threading;
using log4net;

namespace Service1
{
    [ServiceContract]
    public interface ISampleService
    {
        [OperationContract]
        void TimedOperation(int timeIntervalInSeconds);

        [OperationContract]
        void ResetTimedOperationsCount();
        
    }

    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall, ConcurrencyMode=ConcurrencyMode.Multiple)]
    public class SampleService : ISampleService
    {
        private static int _connections;
        private int _instanceCount;
        private static ILog _logger = LogManager.GetLogger(typeof (SampleService));
        public SampleService()
        {
            Interlocked.Increment(ref _instanceCount);
        }

        public void TimedOperation(int timeIntervalInSeconds)
        {
            int count = Interlocked.Increment(ref _connections);
            _logger.InfoFormat("time: {0} connection no: {1} instance no: {2} thread id: {3}", DateTime.Now.TimeOfDay, count, _instanceCount, Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(timeIntervalInSeconds*1000);
            _logger.InfoFormat("time: {0} finished connection no: {1}", DateTime.Now.TimeOfDay, count);
        }

        public void ResetTimedOperationsCount()
        {
            _connections = 0;
        }
    }
}
