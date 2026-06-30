using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserGrid : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] private TextMeshProUGUI textRecord;

    public void SetData(string nickname, int record)
    {
        textNickname.text = nickname;
        textRecord.text = $"×{record}";
    }
}
