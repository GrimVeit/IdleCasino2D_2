using System;
using System.Collections;
using System.Collections.Generic;

public enum MessagesDealerType
{
    Idle,
    Game
}

public static class MessagesDealer
{
    private static readonly Dictionary<MessagesDealerType, string[]> quotes = new()
    {
        { MessagesDealerType.Idle, new string[]
            {
                "Place your bets.",
                "Table is open.",
                "Waiting for players.",
                "Step right up.",
                "Game is ready.",
                "Anyone for a round?",
                "Table is available.",
                "Ready when you are.",
                "Looking for players.",
                "Join the table."
            }
        },

        { MessagesDealerType.Game, new string[]
            {
                "Good luck!",
                "Place your bets.",
                "No more bets.",
                "Let's see the cards.",
                "Round begins.",
                "Your move.",
                "Here we go.",
                "Let's play.",
                "Next round.",
                "Best of luck."
            }
        }
    };

    private static readonly Random random = new();

    public static string GetRandomQuote(MessagesDealerType type)
    {
        if (!quotes.ContainsKey(type))
            return "";

        var arr = quotes[type];
        return arr[random.Next(arr.Length)];
    }
}
