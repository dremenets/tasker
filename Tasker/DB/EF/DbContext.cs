using System.Data.Entity;

namespace Tasker.DB.EF
{
    class TaskerContext : DbContext
    {
        public TaskerContext() :
            base("TaskerDB")
        { }

        public DbSet<Task> Tasks { get; set; }
    }
}