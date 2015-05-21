using LComplete.Framework.Logging;
using Quartz;

namespace LComplete.Framework.JobService
{
    public abstract class BaseJob : IJob
    {
        protected BaseJob()
        {
            _log = LogManager.GetLogger(this.GetType());
        }

        private ILog _log;

        protected ILog Log { get { return _log; } }

        public abstract void Execute(IJobExecutionContext context);
    }
}
