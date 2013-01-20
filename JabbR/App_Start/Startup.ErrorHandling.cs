﻿using System;
using System.Threading.Tasks;
using Elmah;

namespace JabbR
{
    public partial class Startup
    {
        private static void SetupErrorHandling()
        {
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                try
                {
                    // Write all unobserved exceptions to elmah
                    ReportError(e.Exception);
                }
                catch
                {
                    // Swallow!
                }
                finally
                {
                    e.SetObserved();
                }
            };
        }

        private static void ReportError(Exception e)
        {
            ErrorLog.GetDefault(null).Log(new Error(e));
        }
    }
}