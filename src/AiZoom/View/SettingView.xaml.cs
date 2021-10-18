using AiZoom.ViewModel;
using Autofac;
using System.Windows.Controls;

namespace AiZoom.View
{
    /// <summary>
    /// Interaction logic for SettingView.xaml
    /// </summary>
    public partial class SettingView : UserControl
    {
        public SettingView()
        {
            InitializeComponent();
            DataContext = Locator.Container.Resolve<SettingViewModel>();
        }
    }
}
