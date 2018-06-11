using AdaptiveCards;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Prototype1
{
    class JsonCreator
    {
        static bool useJsonFile = true;

        public static async Task<string> CreateJson()
        {
            if (useJsonFile)
            {
                StorageFile cardFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Schemas/AdaptiveCard.json"));
                string cardString = await FileIO.ReadTextAsync(cardFile);
                return cardString;
            }

            AdaptiveCard card = new AdaptiveCard();

            //AdaptiveTextBlock title = new AdaptiveTextBlock
            //{
            //    Text = str,
            //    Size = AdaptiveTextSize.Medium,
            //    Wrap = true
            //};

            //AdaptiveColumnSet columnSet = new AdaptiveColumnSet();

            //AdaptiveColumn photoColumn = new AdaptiveColumn
            //{
            //    Width = "auto"
            //};
            //AdaptiveImage image = new AdaptiveImage
            //{
            //    Url = new Uri("https://pbs.twimg.com/profile_images/587911661526327296/ZpWZRPcp_400x400.jpg"),
            //    Size = AdaptiveImageSize.Small,
            //    Style = AdaptiveImageStyle.Person
            //};
            //photoColumn.Items.Add(image);

            //AdaptiveTextBlock name = new AdaptiveTextBlock
            //{
            //    Text = "Matteo Pagani",
            //    Weight = AdaptiveTextWeight.Bolder,
            //    Wrap = true
            //};

            //AdaptiveTextBlock date = new AdaptiveTextBlock
            //{
            //    Text = lastNews.PublishDate.ToShortDateString(),
            //    IsSubtle = true,
            //    Spacing = AdaptiveSpacing.None,
            //    Wrap = true
            //};

            //AdaptiveColumn authorColumn = new AdaptiveColumn
            //{
            //    Width = "stretch"
            //};
            //authorColumn.Items.Add(name);
            //authorColumn.Items.Add(date);

            //columnSet.Columns.Add(photoColumn);
            //columnSet.Columns.Add(authorColumn);

            //AdaptiveTextBlock body = new AdaptiveTextBlock
            //{
            //    Text = str,
            //    Wrap = true
            //};

            //AdaptiveOpenUrlAction action = new AdaptiveOpenUrlAction
            //{
            //    Url = new Uri(lastNews.Link),
            //    Title = "View"
            //};

            //card.Body.Add(title);
            //card.Body.Add(columnSet);
            //card.Body.Add(body);
            //card.Actions.Add(action);

            return card.ToJson();
        }

        public static string CreateTodoJson(string title, string dueDate, string url)
        {
            AdaptiveCard card = new AdaptiveCard();
            AdaptiveTextBlock text_title = new AdaptiveTextBlock
            {
                Text = title,
                Wrap = true
            };
            AdaptiveTextBlock text_url = new AdaptiveTextBlock
            {
                Text = url,
                Wrap = true
            };
            AdaptiveTextBlock text_dueDate = new AdaptiveTextBlock
            {
                Text = "DueDate: " + dueDate,
                Wrap = true
            };

            AdaptiveSubmitAction action = new AdaptiveSubmitAction
            {
                Title = "✅"
            };

            card.Body.Add(text_title);
            if(url != null && url != String.Empty)
            {
                card.Body.Add(text_url);
            }
            card.Body.Add(text_dueDate);
            card.Actions.Add(action);
            return card.ToJson();
        }
    }
}
