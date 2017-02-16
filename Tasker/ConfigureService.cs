using Autofac;
using Tasker.DB;
using Tasker.DB.EF;
using Topshelf;

namespace Tasker
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            var builder = new ContainerBuilder();

            IGenericRepository<Task> repository = new EFGenericRepository<Task>();
            builder.RegisterInstance<IGenericRepository<Task>>(repository).SingleInstance();
            IJobControl manager = new JobManager(repository);
            builder.RegisterInstance<IJobControl>(manager).SingleInstance();

            var tasksService = new TaskerService(manager, repository);

            builder.Build();

            HostFactory.Run(configure =>
            {
                configure.Service<TaskerService>(service =>
                {
                    service.ConstructUsing(s => tasksService);
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });

                configure.RunAsLocalSystem();
                configure.SetServiceName("Tasker");
                configure.SetDisplayName("TaskerService");
                configure.SetDescription("Tasker .Net windows service with Topshelf");
            });
        }
    }
}