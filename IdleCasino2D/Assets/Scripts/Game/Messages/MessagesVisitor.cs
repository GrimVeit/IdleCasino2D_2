using System;
using System.Collections;
using System.Collections.Generic;

public enum MessagesVisitorType
{
    Waiting,            // Ожидание в очереди
    GoToRoulette,
    PlayingRoulette,
    GoToSlot,
    PlayingSlot,
    GoToPoker,
    PlayingPoker,
    GoToWheel,
    PlayingWheel,
    GoToBar,
    AtBar,
    GoToSinger,
    AtSinger,
    Exit       // Уход из казино
}

public static class MessagesVisitor
{
    private static readonly Dictionary<MessagesVisitorType, string[]> quotes = new Dictionary<MessagesVisitorType, string[]>
    {
        // 🔹 Waiting
        { MessagesVisitorType.Waiting, new string[]
            {
                "How long is this gonna take?",
                "Can't wait to play!",
                "Hope my turn comes soon!",
                "I'm ready, let's go!",
                "Hurry up, I’m excited!",
                "Waiting is the hardest part!",
                "Is it my turn yet?",
                "I’m itching to try this!",
                "This line feels endless!",
                "Almost there, I can feel it!"
            }
        },

        // 🔹 Going to tables / zones
        { MessagesVisitorType.GoToRoulette, new string[]
            {
                "Roulette, here I come!",
                "Let’s try my luck!",
                "Time to spin the wheel!",
                "I hope I hit the jackpot!",
                "Roulette, don’t fail me now!",
                "Off to the roulette table!",
                "This is my lucky day, I feel it!",
                "Let’s see what the ball decides!",
                "I’m ready for some action!",
                "Spin it and win!"
            }
        },

        { MessagesVisitorType.GoToSlot, new string[]
            {
                "Slots, my favorite!",
                "Let’s pull the lever!",
                "I feel lucky today!",
                "Time to hit a jackpot!",
                "Slots never disappoint!",
                "I hope for three cherries!",
                "Can’t wait to see the reels spin!",
                "Fingers crossed for a big win!",
                "Let’s go, slots!",
                "Maybe today’s my lucky day!"
            }
        },

        { MessagesVisitorType.GoToPoker, new string[]
            {
                "Poker time!",
                "Let’s see what hands I get!",
                "Ready for a good bluff!",
                "I hope I get a royal flush!",
                "Poker, don’t let me down!",
                "Time to test my strategy!",
                "I’m feeling confident today!",
                "Let’s play smart!",
                "Cards are ready, let’s go!",
                "Show me the best hand!"
            }
        },

        { MessagesVisitorType.GoToWheel, new string[]
            {
                "Wheel of Fortune, spin it!",
                "I hope for big prizes!",
                "Time to try my luck!",
                "Spin and win!",
                "Let’s see what fate gives me!",
                "I’m excited for this!",
                "Hope the wheel is kind today!",
                "Round and round it goes!",
                "This could be my jackpot moment!",
                "Let’s spin the golden wheel!"
            }
        },

        { MessagesVisitorType.GoToBar, new string[]
            {
                "A drink would be perfect!",
                "Time to grab a cocktail!",
                "Bar, here I come!",
                "I need a refreshment!",
                "Let’s see what’s on the menu!",
                "Bartender, I’m ready!",
                "Something fruity, please!",
                "A quick drink before the games!",
                "I could use a little break!",
                "Cheers to the fun night!"
            }
        },

        { MessagesVisitorType.GoToSinger, new string[]
            {
                "Can’t wait for the show!",
                "Let’s see the performance!",
                "Time for some music!",
                "I hope she sings my favorite!",
                "Stage, here I come!",
                "This is going to be amazing!",
                "Let’s enjoy the tunes!",
                "Music makes the night better!",
                "I’m ready to clap along!",
                "Excited for the live show!"
            }
        },

        // 🔹 Playing / at place
        { MessagesVisitorType.PlayingRoulette, new string[]
            {
                "Spin, spin, spin!",
                "Come on, big win!",
                "Fingers crossed!",
                "Hope luck is on my side!",
                "This is my moment!",
                "Let’s see the ball move!",
                "Feeling lucky today!",
                "Almost there… jackpot?",
                "Yes, I can win this!",
                "Let’s go, roulette!"
            }
        },

        { MessagesVisitorType.PlayingSlot, new string[]
            {
                "Pull the lever!",
                "Three cherries, please!",
                "Yes, spin!",
                "Big win incoming!",
                "I feel lucky!",
                "Come on, jackpot!",
                "Spin those reels!",
                "Fingers crossed!",
                "Let’s hit a combo!",
                "Almost… almost!"
            }
        },

        { MessagesVisitorType.PlayingPoker, new string[]
            {
                "Let’s see my cards!",
                "I’ll bluff now!",
                "Fold or raise?",
                "Time to win!",
                "Hope for a good hand!",
                "This is exciting!",
                "Let’s play smart!",
                "Feeling confident!",
                "I can win this pot!",
                "Check, call, raise!"
            }
        },

        { MessagesVisitorType.PlayingWheel, new string[]
            {
                "Spin the wheel!",
                "Big prize, come on!",
                "Fingers crossed!",
                "Let’s see what happens!",
                "I hope it lands on gold!",
                "Exciting!",
                "Almost there!",
                "Spin, spin, spin!",
                "Luck, don’t fail me!",
                "Come on, jackpot!"
            }
        },

        { MessagesVisitorType.AtBar, new string[]
            {
                "This drink is perfect!",
                "Cheers!",
                "Refreshing!",
                "Just what I needed!",
                "Time to relax!",
                "Sip sip!",
                "Love this cocktail!",
                "Delicious!",
                "Perfect taste!",
                "Feeling fancy!"
            }
        },

        { MessagesVisitorType.AtSinger, new string[]
            {
                "Love this song!",
                "Amazing performance!",
                "Clap clap clap!",
                "She’s incredible!",
                "Wow, beautiful voice!",
                "This is so fun!",
                "Can’t stop smiling!",
                "I’m enjoying every note!",
                "Encore!",
                "So talented!"
            }
        },

        // 🔹 Leaving casino
        { MessagesVisitorType.Exit, new string[]
            {
                "Time to go, had fun!",
                "See you next time!",
                "That was exciting!",
                "I need a break!",
                "Thanks for the fun!",
                "Won some, lost some!",
                "Goodbye, casino!",
                "I’ll be back soon!",
                "Had a blast tonight!",
                "See you later!"
            }
        }
    };

    private static readonly Random random = new();

    public static string GetRandomQuote(MessagesVisitorType state)
    {
        if (!quotes.ContainsKey(state)) return "";
        var arr = quotes[state];
        return arr[random.Next(arr.Length)];
    }
}
