using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameLanguageUtility
{
    //public static string GetHandRankName(HandRank rank, Language language = Language.English)
    //{
    //    return (rank, language) switch
    //    {
    //        (HandRank.HighCard, Language.English) => "High Card",
    //        (HandRank.HighCard, Language.Russia) => "Старшая карта",

    //        (HandRank.OnePair, Language.English) => "One Pair",
    //        (HandRank.OnePair, Language.Russia) => "Пара",

    //        (HandRank.TwoPair, Language.English) => "Two Pair",
    //        (HandRank.TwoPair, Language.Russia) => "Две пары",

    //        (HandRank.ThreeOfAKind, Language.English) => "Three of a Kind",
    //        (HandRank.ThreeOfAKind, Language.Russia) => "Тройка",

    //        (HandRank.Straight, Language.English) => "Straight",
    //        (HandRank.Straight, Language.Russia) => "Стрит",

    //        (HandRank.Flush, Language.English) => "Flush",
    //        (HandRank.Flush, Language.Russia) => "Флеш",

    //        (HandRank.FullHouse, Language.English) => "Full House",
    //        (HandRank.FullHouse, Language.Russia) => "Фулл-хаус",

    //        (HandRank.FourOfAKind, Language.English) => "Four of a Kind",
    //        (HandRank.FourOfAKind, Language.Russia) => "Каре",

    //        (HandRank.StraightFlush, Language.English) => "Straight Flush",
    //        (HandRank.StraightFlush, Language.Russia) => "Стрит-флеш",

    //        (HandRank.RoyalFlush, Language.English) => "Royal Flush",
    //        (HandRank.RoyalFlush, Language.Russia) => "Роял-флеш",

    //        _ => ""
    //    };
    //}

    //public static string GetRoundName_Open(int roundNumber, Language language = Language.English)
    //{
    //    return language switch
    //    {
    //        Language.English => $"Round {roundNumber}",
    //        Language.Russia => $"Раунд {roundNumber}",
    //        _ => $"Round {roundNumber}"
    //    };
    //}

    //public static string GetRoundName_Complete(int roundNumber, Language language = Language.English)
    //{
    //    return language switch
    //    {

    //        Language.English => $"Round <color=#F0BE61>{roundNumber}</color> completed",
    //        Language.Russia => $"Раунд <color=#F0BE61>{roundNumber}</color> завершён",
    //        _ => $"Round {roundNumber}"
    //    };
    //}

    //public static string GetPassCountName(int currentPassCount, int total, Language language = Language.English)
    //{
    //    return language switch
    //    {

    //        Language.English => $"Passes: {currentPassCount}/{total}",
    //        Language.Russia => $"Пас: {currentPassCount}/{total}",
    //        _ => $"Passes: {currentPassCount}/{total}"
    //    };
    //}

    //public static string GetGameInfo_Description(GameDifficulty difficulty, Language language = Language.English)
    //{
    //    return (difficulty, language) switch
    //    {
    //        // Лёгкий / Easy
    //        (GameDifficulty.Easy, Language.Russia) =>
    //            "Боты делают слабые ходы, могут часто пасовать, даже если нужная карта есть. При выборе покерной комбинации часто берут менее выгодные карты.",

    //        (GameDifficulty.Easy, Language.English) =>
    //            "Bots make weak moves and often pass, even if they have a playable card. When choosing poker hands, they often pick suboptimal cards.",

    //        // Средний / Medium
    //        (GameDifficulty.Medium, Language.Russia) =>
    //            "Боты играют разумнее, иногда пропускают свои ходы, даже если нужная карта есть. При выборе покерной комбинации стараются выбирать хорошие карты, но иногда совершают ошибки.",

    //        (GameDifficulty.Medium, Language.English) =>
    //            "Bots play smarter and sometimes skip their turn, even if they have a playable card. When selecting poker hands, they usually pick stronger cards, but sometimes make mistakes.",

    //        // Про / Hard
    //        (GameDifficulty.Hard, Language.Russia) =>
    //            "Боты почти всегда используют свои карты эффективно, почти не пропускают ходы и выбирают покерные комбинации максимально выгодно.",

    //        (GameDifficulty.Hard, Language.English) =>
    //            "Bots almost always use their cards efficiently, rarely pass, and select poker hands optimally.",

    //        _ => ""
    //    };
    //}

    //public static string GetGameInfo_Difficulty(GameDifficulty difficulty, Language language = Language.English)
    //{
    //    return (difficulty, language) switch
    //    {
    //        (GameDifficulty.Easy, Language.Russia) => "Сложность: <color=#F0C8B1>Лёгкая</color>",
    //        (GameDifficulty.Easy, Language.English) => "Difficulty: <color=#F0C8B1>Easy</color>",

    //        (GameDifficulty.Medium, Language.Russia) => "Сложность: <color=#F0C8B1>Средняя</color>",
    //        (GameDifficulty.Medium, Language.English) => "Difficulty: <color=#F0C8B1>Medium</color>",

    //        (GameDifficulty.Hard, Language.Russia) => "Сложность: <color=#F0C8B1>Сложная</color>",
    //        (GameDifficulty.Hard, Language.English) => "Difficulty: <color=#F0C8B1>Hard</color>",

    //        _ => ""
    //    };
    //}

    //public static string GetGameInfo_RoundCount(int count, Language language = Language.English)
    //{
    //    return language switch
    //    {

    //        Language.English => $"Rounds: <color=#F0C8B1>{count}</color>",
    //        Language.Russia => $"Раунды: <color=#F0C8B1>{count}</color>",
    //        _ => $"Rounds: <color=#F0C8B1>{count}</color>"
    //    };
    //}

    //public static string GetGameInfo_PlayersCount(int count, Language language = Language.English)
    //{
    //    return language switch
    //    {

    //        Language.English => $"Players: <color=#F0C8B1>{count}</color>",
    //        Language.Russia => $"Игроки: <color=#F0C8B1>{count}</color>",
    //        _ => $"Players: <color=#F0C8B1>{count}</color>"
    //    };
    //}
}
