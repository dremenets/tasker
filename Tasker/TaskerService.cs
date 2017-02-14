﻿using System;
using System.Timers;

namespace Tasker
{
    public class TaskerService
    {
        private Timer _timer;

        public void Start()
        {
            var jobManager = new JobManager();
            jobManager.InitTasks();

            _timer = new Timer
            {
                AutoReset = true,
                Enabled = true,
                Interval = 5000
            };

            _timer.Elapsed += (o, e) => jobManager.InitTasks();

            Log.Info("Tasker Service is started!");
        }

        public void Stop()
        {
            Log.Info("Tasker Service is stoped!");
        }
    }
}