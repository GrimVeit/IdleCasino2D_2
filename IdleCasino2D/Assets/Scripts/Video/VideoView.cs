using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoView : View
{
    [SerializeField] private VideoPlayers videoPlayers;

    public void Prepare(string id)
    {
        var videoPlay = videoPlayers.GetVideoPlayById(id);

        if (videoPlay.VideoPlayer == null)
        {
            Debug.LogWarning($"VideoPlayer with id: {id} not found!");
            return;
        }

        videoPlay.Image.texture = videoPlay.Texture;

        videoPlay.VideoPlayer.time = 0;
        videoPlay.VideoPlayer.Prepare();
    }

    public void Play(string id, Action onComplete = null)
    {
        var videoPlay = videoPlayers.GetVideoPlayById(id);

        if (videoPlay.VideoPlayer == null)
        {
            Debug.LogWarning($"VideoPlayer with id: {id} not found!");
            return;
        }

        StartCoroutine(WaitForVideoEndByFrame(videoPlay.VideoPlayer, onComplete));

        videoPlay.VideoPlayer.Play();

        void OnVideoEnd(VideoPlayer vp)
        {
            vp.loopPointReached -= OnVideoEnd;
            onComplete?.Invoke();
        }

        IEnumerator WaitForVideoEndByFrame(VideoPlayer vp, Action onCompleteCallback)
        {
            while (!vp.isPrepared)
                yield return null;

            while (vp.frame < (long)vp.frameCount - 1)
                yield return null;

            onCompleteCallback?.Invoke();
        }
    }
}

[System.Serializable]
public class VideoPlayers
{
    [SerializeField] private List<VideoPlay> videoPlays = new();

    public VideoPlay GetVideoPlayById(string id) => videoPlays.FirstOrDefault(data => data.Id == id);
}

[System.Serializable]
public class VideoPlay
{
    [SerializeField] private string id;
    [SerializeField] private RawImage image;
    [SerializeField] private Texture texture;
    [SerializeField] private VideoPlayer videoPlayer;

    public string Id => id;
    public VideoPlayer VideoPlayer => videoPlayer;
    public RawImage Image => image;
    public Texture Texture => texture;
}
