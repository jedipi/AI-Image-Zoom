using AiZoom.Models;
using AiZoom.Services.Interfaces;
using Autofac;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TG.INI;
using TG.INI.Serialization;

namespace AiZoom.ViewModel
{
    public class SettingViewModel : ObservableObject
    {
        #region Commands
        public IRelayCommand LoadedCmd { get; set; }
        public IRelayCommand SaveCmd { get; set; }
        public IRelayCommand DiscardCmd { get; set; }
        public IRelayCommand RestoreCmd { get; set; }
        public IRelayCommand BrowseFolderCmd { get; set; }

        #endregion

        #region Fields

        private IDialogCoordinator _dialog;
        #endregion

        #region Properties
        public List<string> Modules { get; set; }
        
        public string SelectedModule { get; set; }

        public string FileNameSuffix { get; set; }

        public List<string> OutputFormats => new List<string>() { "jpg", "png", "webp" };
        public string SelectedOutputFormat { get; set; }

        public string OutputDirectory { get; set; }
        #endregion

        public SettingViewModel()
        {
            _dialog = Locator.Container.Resolve<IDialogCoordinator>();
            LoadedCmd = new RelayCommand(OnLoadedCmd);
            SaveCmd = new RelayCommand(OnSaveCmd);
            DiscardCmd = new RelayCommand(OnDiscardCmd);
            RestoreCmd = new RelayCommand(OnRestoreCmd);
            BrowseFolderCmd = new RelayCommand(OnBrowseFolderCmd);            
        }

        private void OnBrowseFolderCmd()
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

            bool? success = dialog.ShowDialog();
            if (success == true)
            {
                OutputDirectory = dialog.SelectedPath;
            }
        }

        private void OnRestoreCmd()
        {
            SelectedModule = "realesrgan-x4plus";
            FileNameSuffix = "";
            SelectedOutputFormat = "jpg";
            OutputDirectory = $"{AppContext.BaseDirectory}output";

            OnSaveCmd();
        }

        private void OnDiscardCmd()
        {
            if (File.Exists(@"config.ini"))
            {
                var document = new IniDocument(@"config.ini");
                var settings = IniSerialization.DeserializeDocument<SettingModel>(document);

                if (Modules.Contains(settings.Module))
                    SelectedModule = settings.Module;

                FileNameSuffix = settings.FileNameSuffix;
                SelectedOutputFormat = settings.OutputFormat;
                OutputDirectory = settings.OutputDirectory;
            }
            else
            {
                OnRestoreCmd();
            }
        }

        private void OnSaveCmd()
        {
            var settings = new SettingModel()
            {
                Module = SelectedModule,
                FileNameSuffix = FileNameSuffix,
                OutputFormat = SelectedOutputFormat,
                OutputDirectory = OutputDirectory
            };
            var doc = IniSerialization.SerializeObjectToNewDocument(settings);
            doc.Write(@"config.ini");
        }

        private void OnLoadedCmd()
        {
            var moduleService = Locator.Container.Resolve<IModuleService>();
            Modules = moduleService.GetModules();
            OnDiscardCmd();
        }
    }
}