using System;
using System.Collections;
using System.Collections.Generic;

public enum MessagesManagerType
{
    Idle,       // просто стоят
    RadioTalk   // общаются по рации
}

public static class MessagesManager
{
    private static readonly Dictionary<MessagesManagerType, string[]> quotes = new()
    {
        { MessagesManagerType.Idle, new string[]
            {
                "Everything looks good.",
                "Busy night tonight.",
                "Keep things running smoothly.",
                "Guests seem happy.",
                "All tables are active.",
                "Casino is lively tonight.",
                "Looks like a good crowd.",
                "So far, so good.",
                "Everything under control.",
                "Let's keep it that way."
            }
        },

        { MessagesManagerType.RadioTalk, new string[]
            {
                "Control, everything clear here.",
                "Copy that.",
                "Table status looks good.",
                "Keep an eye on the roulette area.",
                "All stations report in.",
                "Roger that.",
                "Monitoring the floor.",
                "Everything running smoothly.",
                "Stay alert tonight.",
                "Let me know if anything changes."
            }
        }
    };

    private static readonly Random random = new();

    public static string GetRandomQuote(MessagesManagerType type)
    {
        if (!quotes.ContainsKey(type))
            return "";

        var arr = quotes[type];
        return arr[random.Next(arr.Length)];
    }
}
