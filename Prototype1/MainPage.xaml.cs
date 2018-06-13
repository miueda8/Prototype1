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
using Windows.UI.Shell;
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

        private UserActivityChannel _userActivityChannel;
        private UserActivity _userActivity;
        private UserActivitySession _userActivitySession;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            _userActivityChannel = UserActivityChannel.GetDefault();
            _userActivity = await _userActivityChannel.GetOrCreateUserActivityAsync("Todo");
        }

        private static async Task<string> LoadHostConfig()
        {
            StorageFile hostConfigFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Schemas/HostConfig.json"));
            string jsonString = await FileIO.ReadTextAsync(hostConfigFile);
            return jsonString;
        }

        private async void OnNewTodo(object sender, RoutedEventArgs e)
        {
            AdaptiveCardRenderer renderer = new AdaptiveCardRenderer();

            string hostConfigString = await LoadHostConfig();
            var hostConfig = AdaptiveHostConfig.FromJsonString(hostConfigString);
            renderer.HostConfig = hostConfig.HostConfig;

            string json = await JsonCreator.CreateJson();
            AdaptiveCardParseResult card = AdaptiveCard.FromJsonString(json);
            var renderResult = renderer.RenderAdaptiveCard(card.AdaptiveCard);
            renderResult.Action += RenderResult_OnAction;

            if (renderResult != null)
            {
                CreatePanel.Children.Add(renderResult.FrameworkElement);
            }
        }

        private async void RenderResult_OnAction(RenderedAdaptiveCard sender, AdaptiveActionEventArgs e)
        {
            if (e.Action.ActionType == ActionType.Submit)
            {
                ValueSet set = e.Inputs.AsValueSet();

                AdaptiveCardRenderer renderer = new AdaptiveCardRenderer();
                var hostConfig = new AdaptiveHostConfig()
                {
                    ContainerStyles =
                    {
                        Default =
                        {
                            BackgroundColor = Windows.UI.Color.FromArgb(20,0,0,0)
                        }
                    }
                };
                renderer.HostConfig = hostConfig;

                string todoJson = await JsonCreator.CreateTodoJson(set["title"] as string, set["dueDate"] as string, set["url"] as string, set["importance"] as string);
                AdaptiveCardParseResult card = AdaptiveCard.FromJsonString(todoJson);
                var renderResult = renderer.RenderAdaptiveCard(card.AdaptiveCard);
                renderResult.Action += TodoCard_OnAction;
                if (renderResult != null)
                {
                    TodoList.Items.Insert(0, renderResult.FrameworkElement);
                }

                string url = set["url"] as string;
                if (url != null && url != "")
                {
                    _userActivity.ActivationUri = new Uri(url);
                    _userActivity.VisualElements.Content = AdaptiveCardBuilder.CreateAdaptiveCardFromJson(todoJson);

                    await _userActivity.SaveAsync();
                    _userActivitySession?.Dispose();
                    _userActivitySession = _userActivity.CreateSession();
                }
            }
        }

        private async void TodoCard_OnAction(RenderedAdaptiveCard sender, AdaptiveActionEventArgs e)
        {
            TodoList.Items.Remove(sender.FrameworkElement);
        }
    }
}
