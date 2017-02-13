using System.Data.Entity;

namespace Tasker.DB
{
    class TaskerContext : DbContext
    {
        public TaskerContext() :
            base("TaskerDB")
        { }

        public DbSet<Task> Tasks { get; set; }
    }
}