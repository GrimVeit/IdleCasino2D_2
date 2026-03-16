using System;
using System.Collections;
using System.Collections.Generic;

public enum MessagesSongstressType
{
    Idle,        // отдыхает между выступлениями
    Performing   // поёт / танцует
}

public static class MessagesSongstress
{
    private static readonly Dictionary<MessagesSongstressType, string[]> quotes = new()
    {
        { MessagesSongstressType.Idle, new string[]
            {
                "Hope you're enjoying the night!",
                "What a lovely crowd tonight!",
                "The show continues soon!",
                "Stay tuned for the next song!",
                "I'm glad you're all here!",
                "Ready for more music?",
                "This casino has such great energy!",
                "I love performing here!",
                "Hope everyone is having fun!",
                "The night is still young!"
            }
        },

        { MessagesSongstressType.Performing, new string[]
    {
        "Feel the rhythm tonight!",
        "Lights are shining bright!",
        "Dance into the night!",
        "Lucky stars above!",
        "Let the music play!",
        "Spin the wheel of fate!",
        "Dreams are in the air!",
        "Hearts are beating fast!",
        "Luck is on your side!",
        "Raise your hands tonight!",
        "Golden lights around!",
        "Hear the crowd tonight!",
        "Feel the casino vibe!",
        "Tonight we celebrate!",
        "Let the night begin!"
    }
},
    };

    private static readonly Random random = new();

    public static string GetRandomQuote(MessagesSongstressType type)
    {
        if (!quotes.ContainsKey(type))
            return "";

        var arr = quotes[type];
        return arr[random.Next(arr.Length)];
    }
}
