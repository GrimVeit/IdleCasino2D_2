using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardUser : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textNick;
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private GameObject gameObjectUser;

    public void SetData(string nickname, int score)
    {
        gameObjectUser.SetActive(true);
        textNick.text = nickname;
        textScore.text = score.ToString();
    }

    public void Clear()
    {
        gameObjectUser.SetActive(false);
    }
}
