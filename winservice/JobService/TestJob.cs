using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace LComplete.Framework.JobService
{
    class TestJob:BaseJob
    {
        public override void Execute(IJobExecutionContext context)
        {
            Log.Info("test");
        }
    }
}
