﻿using DBLibrary;
using DBLibrary.Entity;
using DBLibrary.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Tasker.Jobs;

namespace Tasker
{
    public class JobManager: IJobControl
    {
        private readonly IGenericRepository<Task> _repository;
        private readonly Dictionary<int, Timer> _timers;

        public JobManager(IGenericRepository<Task> repository)
        {
            _repository = repository;
            _timers = new Dictionary<int, Timer>();
        }

        public void Init(Task task)
        {
            var job = JobFactory.CreateJob(task);
            task.Status = Status.Scheduled;
            _repository.Update(task);

            Log.Info($"Task with TaskId: {job.TaskId} is scheduled!");

            var interval = task.ExpectedStart.Subtract(DateTime.Now);
            var timer = new Timer(interval.TotalMilliseconds) { AutoReset = false, Enabled = true };
            timer.Elapsed += (sender, e) => RunJob(job);
            _timers[task.Id] = timer;
        }

        public List<int> GetScheduledTaskIds()
        {
            return _timers.Keys.ToList();
        }

        private void RunJob(Job job)
        {
            Status status;
            if (job.Run())
            {
                status = Status.Completed;
                Log.Info($"Task with TaskId: {job.TaskId} is completed!");
            }
            else
            {
                status = Status.Failed;
                Log.Info($"Task with TaskId: {job.TaskId} is failed!");
            }

            var task = _repository.FindById(job.TaskId);
            task.Status = status;
            _repository.Update(task);

            _timers.Remove(job.TaskId);
        }
    }
}