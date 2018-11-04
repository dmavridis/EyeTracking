using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EyeTrackingWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindowModel _mainWindowModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _mainWindowModel = new MainWindowModel();
            MainWindow = new MainWindow()
            {
                Visibility = Visibility.Visible,
                DataContext = _mainWindowModel
            };
        }

        /// <summary>
        /// We have to dispose the WpfEyeXHost on exit. This makes sure
        /// that all resources are cleaned up and that the connection to
        /// the EyeX Engine is closed. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _mainWindowModel.Dispose();
        }
    }
}
