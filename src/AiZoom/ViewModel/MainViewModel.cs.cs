using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AiZoom.Services.Interfaces;
using Autofac;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

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
            ExitCmd = new RelayCommand(()=> { Environment.Exit(0); });
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
