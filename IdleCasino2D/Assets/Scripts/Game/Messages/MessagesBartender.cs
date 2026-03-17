using System;
using System.Collections.Generic;

public enum MessagesBartenderType
{
    Idle,
    Serving
}

public static class MessagesBartender
{
    private static readonly Dictionary<MessagesBartenderType, string[]> quotes = new()
    {
        { MessagesBartenderType.Idle, new string[]
            {
                "Waiting for orders.",
                "The bar is ready.",
                "Standing by.",
                "Clean glasses.",
                "Stocking the shelves.",
                "Bar is open.",
                "Ready for customers.",
                "Checking the drinks.",
                "Idle for now.",
                "Keeping things tidy."
            }
        },

        { MessagesBartenderType.Serving, new string[]
            {
                "Here's your drink!",
                "Enjoy!",
                "One cocktail coming up.",
                "Fresh drink for you.",
                "Cheers!",
                "Bottoms up!",
                "A little something special.",
                "Drink ready.",
                "Hope you like it.",
                "Careful, it's hot!"
            }
        }
    };

    private static readonly Random random = new();

    public static string GetRandomQuote(MessagesBartenderType type)
    {
        if (!quotes.ContainsKey(type))
            return "";

        var arr = quotes[type];
        return arr[random.Next(arr.Length)];
    }
}
