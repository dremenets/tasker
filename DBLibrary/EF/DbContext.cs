using DBLibrary.Entity;
using System.Data.Entity;

namespace DBLibrary.EF
{
    class TaskerContext : DbContext
    {
        public TaskerContext() :
            base("TaskerDB")
        { }

        public DbSet<Task> Tasks { get; set; }
    }
}