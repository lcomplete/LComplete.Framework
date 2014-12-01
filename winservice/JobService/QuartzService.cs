using LComplete.Framework.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace LComplete.Framework.JobService
{
    class QuartzService
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(QuartzService));

        private IScheduler scheduler = null;

        public void Start()
        {
            _log.Info("服务正在启动");

            scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            FireImmediately();
        }


        /// <summary>
        /// 触发立即执行的任务
        /// </summary>
        private void FireImmediately()
        {
            var groupNames = scheduler.GetTriggerGroupNames();
            foreach (string groupName in groupNames)
            {
                var triggerKeys = scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.GroupEquals(groupName));
                foreach (TriggerKey triggerKey in triggerKeys)
                {
                    var trigger = scheduler.GetTrigger(triggerKey);
                    if (trigger.JobDataMap.ContainsKey("immediately") && trigger.JobDataMap["immediately"].ToString() == "1")
                        scheduler.TriggerJob(trigger.JobKey, trigger.JobDataMap);
                }
            }
        }

        public void Stop()
        {
            if (scheduler != null && !scheduler.IsShutdown)
                scheduler.Shutdown();

            _log.Info("服务已停止");
        }
    }
}
