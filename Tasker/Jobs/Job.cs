﻿using System.Threading.Tasks;

namespace Tasker.Jobs
{
    public abstract class Job
    {
        public abstract Task Run();
    }
}