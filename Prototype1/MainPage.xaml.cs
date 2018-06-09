using AdaptiveCards.Rendering.Uwp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.UserActivities;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Prototype1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string jsonString;

        public MainPage()
        {
            this.InitializeComponent();
            LoadHostConfig();
        }

        private async void LoadHostConfig()
        {
            Windows.Storage.StorageFolder installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Schemas\\HostConfig.json"));
            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var streamReader = new StreamReader(classicStream))
            {
                while (streamReader.Peek() >= 0)
                {
                    jsonString += streamReader.ReadLine();
                }
            }
        }

        private async void OnNewTodo(object sender, RoutedEventArgs e)
        {
            AdaptiveCardRenderer renderer = new AdaptiveCardRenderer();

            var hostConfig = AdaptiveHostConfig.FromJsonString(jsonString);
            renderer.HostConfig = hostConfig.HostConfig;

            string json = new JsonCreator().CreateJson(TodoTitle.Text);
            AdaptiveCardParseResult card = AdaptiveCard.FromJsonString(json);
            var renderResult = renderer.RenderAdaptiveCard(card.AdaptiveCard);

            if (renderResult != null)
            {
                MainPanel.Children.Add(renderResult.FrameworkElement);
            }
        }
    }
}
