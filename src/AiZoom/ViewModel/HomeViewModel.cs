using AiZoom.Models;
using Autofac;
using CliWrap;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.IO;
using System.Threading.Tasks;
using TG.INI;
using TG.INI.Serialization;

namespace AiZoom.ViewModel
{
    class HomeViewModel : ObservableObject
    {
        #region
        public IRelayCommand OpenImageCmd { get; set; }
        public IAsyncRelayCommand StartCmd { get; set; }

        public IAsyncRelayCommand OpenSourceImageCmd { get; set; }
        public IAsyncRelayCommand OpenResultImageCmd { get; set; }

        
        #endregion
        #region Fields

        private IDialogCoordinator _dialog;
        #endregion

        #region Properties

        public string SourceImage { get; set; }

        public string ResultImage { get; set; }
        public double ProgressValue { get; set; }
        public string ProgressString { get; set; } = "0%";

        #endregion

        public HomeViewModel()
        {
            _dialog = Locator.Container.Resolve<IDialogCoordinator>();

            OpenImageCmd = new RelayCommand(OnOpenImageCmd);
            StartCmd = new AsyncRelayCommand(async()=> await OnStartCmd());
            OpenSourceImageCmd = new AsyncRelayCommand(OnOpenSourceImageCmd);
            OpenResultImageCmd = new AsyncRelayCommand(OnOpenResultImageCmd);

        }

        private async Task OnOpenResultImageCmd()
        {
            await OpenImage(ResultImage);
        }

        private async Task OnOpenSourceImageCmd()
        {
            await OpenImage(SourceImage);
        }

        private async Task OpenImage(string imagePath)
        {
            var arg = $"/open, \"{imagePath}\"";
            var result = await Cli.Wrap("explorer.exe")
                .WithArguments(arg)
                .ExecuteAsync();
        }

        private async Task OnStartCmd()
        {
            // get setting
            if (!File.Exists(@"config.ini"))
            {
                await _dialog.ShowMessageAsync(this, "ERROR", "config file is missing.");
                return;
            }

            var document = new IniDocument(@"config.ini");
            var settings = IniSerialization.DeserializeDocument<SettingModel>(document);

            if (string.IsNullOrEmpty(settings.OutputDirectory))
            {
                await _dialog.ShowMessageAsync(this, "ERROR", "Output directory is missing.");
                return;
            }

            var sourceImageFile = new FileInfo(SourceImage);
            var sourceImageName = Path.GetFileNameWithoutExtension(sourceImageFile.Name);

            

            

            var engine = @"realesrgan\realesrgan-ncnn-vulkan.exe";

            var outputFileName = sourceImageName;

            if (!string.IsNullOrEmpty(settings.FileNameSuffix))
            {
                outputFileName = $"{outputFileName}_{settings.FileNameSuffix}";
            }

            var outputFileFullName = $"{settings.OutputDirectory}\\{outputFileName}.{settings.OutputFormat}";
            
            var arg = $" -i \"{SourceImage}\" -o \"{outputFileFullName}\" -n {settings.Module} -f {settings.OutputFormat} -s 4 -g auto -j 2:2:2 -v";
            
            var result = await Cli.Wrap(engine)
                .WithArguments(arg)
                .WithWorkingDirectory(AppContext.BaseDirectory)
                .WithStandardErrorPipe(PipeTarget.ToDelegate(ProcessImage))
                .ExecuteAsync();
        }

        private void ProcessImage(string output)
        {
            var isPercentage = output.IndexOf("%", 0);
            var isDone = output.IndexOf("done", 0);

            // process
            if(isPercentage != -1)
            {
                ProgressString = output;
                ProgressValue = Double.Parse(output.Remove(output.Length-1,1));
            }

            // done
            if(isDone != -1)
            {
                ProgressString = "100%";
                ProgressValue = 100;

                // get final output file here. if the source images has alpha channel, the original file extension is preserved
                var startIndex = output.IndexOf(" -> ", 0);

                var tail = output.Substring(startIndex+4);
                ResultImage = tail.Substring(0, tail.Length - 5);
            }


            
        }

        private void OnOpenImageCmd()
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog() { Filter= "jpeg (*.jpg)|*.jpg|PNG (*.png)|*.png|webp (*.webp)|*.webp"};

            bool? success = dialog.ShowDialog();
            if (success == true)
            {
                SourceImage = dialog.FileName;
                ResultImage = null;
            }
            
        }
    }
}
