using System.Reflection;
using Autofac;
using MahApps.Metro.Controls.Dialogs;

namespace AiZoom
{
    public static class Locator
    {
        public static IContainer Container { get; }

        static Locator()
        {
            ContainerBuilder builder = new ContainerBuilder();
            RegisterType(builder);
            Container = builder.Build();
        }

        /// <summary>
        /// register all viewmodels, services, and repositories
        /// </summary>
        /// <param name="builder"></param>
        static void RegisterType(ContainerBuilder builder)
        {
            var app = Assembly.GetAssembly(typeof(App));

            // register all ViewModels
            builder.RegisterAssemblyTypes(app)
                .Where(t => t.Name.EndsWith("ViewModel"));

            // register all services
            builder.RegisterAssemblyTypes(app)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();


            // register special one
            builder.RegisterInstance(DialogCoordinator.Instance).As<IDialogCoordinator>().SingleInstance();


        }
    }
}
