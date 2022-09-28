using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;

namespace AiZoom.ViewModel
{
    public class AboutViewModel : ObservableObject
    {
        public string CurrentVersion => $"Version - {Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
    }
}
