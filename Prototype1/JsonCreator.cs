using AdaptiveCards;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Prototype1
{
    class JsonCreator
    {

        public static async Task<string> CreateJson()
        {
            StorageFile cardFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Schemas/AdaptiveCard.json"));
            string cardString = await FileIO.ReadTextAsync(cardFile);
            return cardString;
        }

        public static async Task<string> CreateTodoJson(string title, string dueDate, string url, string importance)
        {
            AdaptiveCard card = new AdaptiveCard();

            AdaptiveColumnSet columnSet = new AdaptiveColumnSet();
            AdaptiveColumn colum = new AdaptiveColumn
            {
                Width = AdaptiveColumnWidth.Stretch
            };

            AdaptiveContainer container = new AdaptiveContainer
            {
                Style = AdaptiveContainerStyle.Emphasis
            };

            AdaptiveTextColor color = AdaptiveTextColor.Default;
            if (String.Compare(importance, "1", false) == 0) color = AdaptiveTextColor.Warning;
            else if (String.Compare(importance, "2", false) == 0) color = AdaptiveTextColor.Accent;

            AdaptiveTextBlock text_title = new AdaptiveTextBlock
            {
                Text = title,
                Wrap = true,
                Weight = AdaptiveTextWeight.Bolder,
                Size = AdaptiveTextSize.ExtraLarge,
                Color = color
            };
            AdaptiveTextBlock text_url = new AdaptiveTextBlock
            {
                Text = url,
                Weight = AdaptiveTextWeight.Lighter,
                Wrap = true
            };
            AdaptiveTextBlock text_dueDate = new AdaptiveTextBlock
            {
                Text = "DueDate: " + dueDate,
                Wrap = true
            };

            AdaptiveSubmitAction action = new AdaptiveSubmitAction
            {
                Title = "Done"
            };

            if (url != null && url != "")
            {
                card.BackgroundImage = new Uri(await GetImageURL(url));
            }

            container.Items.Add(text_title);
            container.Items.Add(text_dueDate);
            if (url != null && url != String.Empty)
            {
                container.Items.Add(text_url);
            }

            colum.Items.Add(container);
            columnSet.Columns.Add(colum);
            card.Body.Add(columnSet);
            card.Actions.Add(action);
            return card.ToJson();
        }

        private static async Task<string> GetImageURL(string url)
        {
            string apiEndPoint = "http://api.linkpreview.net/";
            string apiKey = "5b1f7bf8b7ffaad99eee6489ffa0c43c0478d5463e41d"; //L"123456";

            WebRequest webRequest = WebRequest.Create(apiEndPoint + "?key=" + apiKey + "&q=" + url);
            //WebRequest webRequest = WebRequest.Create(requestUriString: "http://api.linkpreview.net/?key=123456&q=https://www.google.com");
            webRequest.Method = "GET";
            WebResponse response = await webRequest.GetResponseAsync();
            Stream objStream = response.GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            string str = objReader.ReadLine();

            string[] separators = { ",\"image\":\"", "\",\"url\":\"" };
            string[] words = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return words[1].Replace("\\", "");
        }
    }
}
