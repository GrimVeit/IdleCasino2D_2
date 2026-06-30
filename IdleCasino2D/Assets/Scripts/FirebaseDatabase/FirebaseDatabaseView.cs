using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseDatabaseView : View
{
    [SerializeField] private List<TopRecord> topRecords = new List<TopRecord>();
    [SerializeField] private SpriteAvatars spriteAvatars;
    private List<UserData> pagedPlayers = new List<UserData>();

    [SerializeField] private Transform transformParent;
    [SerializeField] private UserGrid userGridPrefab;
    [SerializeField] private Button buttonLeft;
    [SerializeField] private Button buttonRight;

    [Header("Config")]
    [SerializeField] private int topCount = 3;
    [SerializeField] private int playersPerPage = 2;

    private int currentPage = 0;
    private int totalPages => Mathf.CeilToInt(pagedPlayers.Count / (float)playersPerPage);

    private IEnumerator coroutineTimer;

    public void Initialize()
    {

    }

    public void Dispose()
    {

    }

    public void TestDebugNickname(string nickname)
    {

    }

    public void DisplayUsersRecords(List<UserData> users)
    {
        //var top = users.Take(topCount).ToList();

        //for (int i = 0; i < top.Count; i++)
        //{
        //    topRecords[i].SetData(top[i].Nickname, top[i].Record, spriteAvatars.GetSpriteById(top[i].Avatar));
        //}

        ////pagedPlayers = users.Skip(topCount).ToList();
        //pagedPlayers = users.ToList();

        //Debug.Log(totalPages);

        //ShowCurrentPage();
    }

    //private void ShowCurrentPage()
    //{
    //    if(coroutineTimer != null) Coroutines.Stop(coroutineTimer);

    //    foreach (Transform child in transformParent)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    coroutineTimer = Timer();
    //    Coroutines.Start(coroutineTimer);
    //}

    //public void NextPage()
    //{
    //    if(currentPage < totalPages - 1)
    //    {
    //        if (coroutineTimer != null) Coroutines.Stop(coroutineTimer);

    //        currentPage += 1;
    //        ShowCurrentPage();
    //    }
    //}

    //public void PreviosPage()
    //{
    //    if(currentPage > 0)
    //    {
    //        if (coroutineTimer != null) Coroutines.Stop(coroutineTimer);

    //        currentPage -= 1;
    //        ShowCurrentPage();
    //    }
    //}

    //private IEnumerator Timer()
    //{
    //    var pageData = pagedPlayers
    //        .Skip(currentPage * playersPerPage)
    //        .Take(playersPerPage)
    //        .ToList();

    //    int rankOffset = topCount + currentPage * playersPerPage + 1;

    //    foreach (var player in pageData)
    //    {
    //        var grid = Instantiate(userGridPrefab, transformParent);

    //        grid.SetData(rankOffset, player.Nickname, player.Record, spriteAvatars.GetSpriteById(player.Avatar));
    //        grid.ActivateRight();

    //        rankOffset += 1;

    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}
}

[Serializable]
public class TopRecord
{
    public int Id => id;

    [SerializeField] private int id;
    [SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] private Image imageAvatar;
    [SerializeField] private TextMeshProUGUI textRecord;

    public void SetData(string nickname, int record, Sprite avatar)
    {
        textNickname.text = nickname;
        imageAvatar.sprite = avatar;
        textRecord.text = record.ToString();
    }
}

[Serializable]
public class SpriteAvatars
{
    [SerializeField] private List<SpriteAvatar> avatars = new List<SpriteAvatar>();

    public Sprite GetSpriteById(int id)
    {
        return avatars.FirstOrDefault(x => x.Id == id).GetSprite();
    }
}

[Serializable]
public class SpriteAvatar
{
    public int Id => id;

    [SerializeField] private int id;
    [SerializeField] private Sprite sprite;

    public Sprite GetSprite()
    {
        return sprite;
    }
}
