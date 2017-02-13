using Topshelf;
namespace Tasker
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<TaskerService>(service =>
                {
                    service.ConstructUsing(s => new TaskerService());
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