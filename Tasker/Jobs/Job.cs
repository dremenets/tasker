namespace Tasker.Jobs
{
    public abstract class Job
    {
        public int TaskId { get; set; }

        public abstract void Run();
    }
}
