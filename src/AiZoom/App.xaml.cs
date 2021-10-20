using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NLog.Fluent;
using WPFLocalizeExtension.Engine;

namespace AiZoom
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        public App()
        {
            InitialiseProductionMode();
            LocalizeDictionary.Instance.Culture = new CultureInfo("zh-Hant");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-Hant");
        }

        public void InitialiseProductionMode()
        {
            
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var ex = new Exception("System has detected an uncaught exception, see inner exception.", e.Exception);
            Log.Fatal().Message(ex.Message).Exception(ex).Write();
            MessageBox.Show(ex.ToString(), "Uncaught Exception");
        }
    }

   
}
