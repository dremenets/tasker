using Autofac;
using Tasker.DB;
using Topshelf;

namespace Tasker
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            var builder = new ContainerBuilder();

            IGenericRepository<Task> repository = new GenericRepository<Task>(new TaskerContext());
            builder.RegisterInstance<IGenericRepository<Task>>(repository).SingleInstance();
            IJobControl manager = new JobManager(repository);
            builder.RegisterInstance<IJobControl>(manager).SingleInstance();
            builder.Build();
            
            HostFactory.Run(configure =>
            {
                configure.Service<TaskerService>(service =>
                {
                    service.ConstructUsing(
                        s => new TaskerService(manager, repository));
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