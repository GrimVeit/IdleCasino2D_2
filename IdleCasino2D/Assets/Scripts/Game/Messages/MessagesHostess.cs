using System;
using System.Collections.Generic;

public enum MessagesHostessType
{
    InviteVisitor,   // приглашает гост€ идти за ней
    CallingVisitor,  // стоит и машет, зовЄт
    Walking,         // ходит между точками
    NoFreePlaces     // говорит что мест нет
}

public static class MessagesHostess
{
    private static readonly Dictionary<MessagesHostessType, string[]> quotes = new()
    {
        { MessagesHostessType.InviteVisitor, new string[]
            {
                "Please follow me!",
                "Right this way!",
                "Come with me please!",
                "Your table is ready!",
                "I'll guide you!",
                "Step this way!",
                "Follow me to the game!",
                "Let me show you the way!",
                "Come along!",
                "This way please!"
            }
        },

        { MessagesHostessType.CallingVisitor, new string[]
            {
                "Over here!",
                "This way please!",
                "Come here!",
                "Your seat is ready!",
                "Right over here!",
                "Follow me!",
                "Step right up!",
                "This way!",
                "Come along!",
                "Over here please!"
            }
        },

        { MessagesHostessType.Walking, new string[]
            {
                "Busy night tonight!",
                "Lots of guests today!",
                "Everything looks great!",
                "Hope everyone is having fun!",
                "Another guest to guide!",
                "Casino is lively tonight!",
                "LetТs keep things moving!",
                "Always something happening here!",
                "Time to help the next guest!",
                "What a wonderful evening!"
            }
        },

        { MessagesHostessType.NoFreePlaces, new string[]
            {
                "Sorry, we're full right now.",
                "Unfortunately, there are no free tables.",
                "Please come back later.",
                "All tables are occupied at the moment.",
                "Sorry, no seats available right now.",
                "The casino is full tonight.",
                "No free spots at the moment.",
                "Please try again a bit later.",
                "Everything is taken right now.",
                "We're full, sorry!"
            }
        }
    };

    private static readonly Random random = new();

    public static string GetRandomQuote(MessagesHostessType type)
    {
        if (!quotes.ContainsKey(type))
            return "";

        var arr = quotes[type];
        return arr[random.Next(arr.Length)];
    }
}
