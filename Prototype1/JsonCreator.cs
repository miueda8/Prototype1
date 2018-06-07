using AdaptiveCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype1
{
    class JsonCreator
    {
        public string CreateJson(string str)
        {
            AdaptiveCard card = new AdaptiveCard();

            AdaptiveTextBlock title = new AdaptiveTextBlock
            {
                Text = str,
                Size = AdaptiveTextSize.Medium,
                Wrap = true
            };

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

            AdaptiveTextBlock body = new AdaptiveTextBlock
            {
                Text = str,
                Wrap = true
            };

            //AdaptiveOpenUrlAction action = new AdaptiveOpenUrlAction
            //{
            //    Url = new Uri(lastNews.Link),
            //    Title = "View"
            //};

            card.Body.Add(title);
            //card.Body.Add(columnSet);
            card.Body.Add(body);
            //card.Actions.Add(action);

            return card.ToJson();
        }
    }
}
