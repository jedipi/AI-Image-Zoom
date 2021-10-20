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
using AiZoom.Models;
using NLog.Fluent;
using TG.INI;
using TG.INI.Serialization;
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
            Initialise();            
        }

        public void Initialise()
        {
            var locale = "en";
            if (File.Exists(@"config.ini"))
            {
                var document = new IniDocument(@"config.ini");
                var settings = IniSerialization.DeserializeDocument<SettingModel>(document);

                if (!string.IsNullOrEmpty(settings.Locale))
                    locale = settings.Locale;                
            }
            LocalizeDictionary.Instance.Culture = new CultureInfo(locale);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var ex = new Exception("System has detected an uncaught exception, see inner exception.", e.Exception);
            Log.Fatal().Message(ex.Message).Exception(ex).Write();
            MessageBox.Show(ex.ToString(), "Uncaught Exception");
        }
    }

   
}
