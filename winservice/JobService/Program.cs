﻿using System;
using LComplete.Framework.JobService.Common;
using LComplete.Framework.Log4Net;
using LComplete.Framework.Logging;
using Topshelf;

namespace LComplete.Framework.JobService
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            LogManager.LogFactory=new Log4NetFactory("log4net.config");

            HostFactory.Run(x =>
            {
                x.Service<QuartzService>(s =>
                {
                    s.ConstructUsing(
                        name => new QuartzService());
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("LComplete.Framework.JobService");
                x.SetDisplayName("LComplete.Framework.JobService");
                x.SetServiceName("LComplete.Framework.JobService");
            });
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException((Exception)e.ExceptionObject, e.IsTerminating);
        }

        static void HandleException(Exception e, bool isTerminating)
        {
            ILog log = LogManager.GetLogger(typeof(Program));
            log.Fatal("-----------------------服务出现未捕获异常", e);

            if (isTerminating)
            {
                EmailUtils.SendToMaintainer("服务出现异常，非正常终止",
                                            "服务出现非正常终止，异常如下：<br><blockquote>" + e.ToString() + "</blockquote>");
            }
        }
    }
}
