using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AiZoom.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        #region Relay Commands

        public IAsyncRelayCommand SaveCmd { get; set; }
        public IAsyncRelayCommand OpenCmd { get; set; }
        public IRelayCommand ExitCmd { get; set; }

        public IAsyncRelayCommand ContentRenderedCmd { get; set; }
        #endregion

        #region Fields


        private IDialogCoordinator _dialog;
        #endregion

        #region Properties

        public string Title { get; set; }



        #endregion


        public MainViewModel()
        {
            ExitCmd = new RelayCommand(() => { Environment.Exit(0); });
            SaveCmd = new AsyncRelayCommand(OnSaveCmd);
            ContentRenderedCmd = new AsyncRelayCommand(async () => await OnContentRenderedCmd());
            _dialog = Locator.Container.Resolve<IDialogCoordinator>();

        }

        private async Task OnContentRenderedCmd()
        {

        }


        private async Task OnSaveCmd()
        {
            await _dialog.ShowMessageAsync(this, "Test title", "Test message");
            Debug.WriteLine("test");
            Title = "New test titel";
            //return null;
        }
    }
}
