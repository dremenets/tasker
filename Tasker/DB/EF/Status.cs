﻿namespace Tasker.DB.EF
{
    public enum  Status: byte
    {
        None = 0,
        Scheduled = 1,
        Completed = 2,
        Failed = 3
    }
}