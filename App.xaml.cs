using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using TaskManager.Data;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;

        public App()
        {
            var services = new ServiceCollection();

            /// Rejestracja DBContext
            services.AddDbContext<TaskDbContext>(options =>
            {
                options.UseSqlite("Data Source = Tasks.db");
            });

            /// Rejestracja MainWindow jako singleton
            services.AddSingleton<MainWindow>();
            serviceProvider = services.BuildServiceProvider();
        }

        private void OnStartup(object s, StartupEventArgs e)
        {
            /// Pobranie głównego okna i jego wyświetlenie
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();

        }
    }
}