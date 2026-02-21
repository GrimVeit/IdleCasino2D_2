using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPresenter : ISoundProvider, ISoundVolumeProvider
{
    private readonly SoundModel _model;
    private readonly SoundView _view;

    public SoundPresenter(SoundModel soundModel, SoundView soundView)
    {
        _model = soundModel;
        _view = soundView;
    }

    public void Initialize()
    {
        ActivateEvents();

        _model.Initialize();
        _view.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _model.Dispose();
        _view.Dispose();
    }

    private void ActivateEvents()
    {
        _view.OnClickSoundButton += _model.MuteUnmute;
    }

    private void DeactivateEvents()
    {
        _view.OnClickSoundButton -= _model.MuteUnmute;
    }

    #region Interface

    public void Play(string id)
    {
        _model.Play(id);
    }

    public void PlayOneShot(string id)
    {
        _model.PlayOneShot(id);
    }

    public ISound GetSound(string id)
    {
        return _model.GetSound(id);
    }

    #endregion

    #region ISoundVolumeProvider

    public float VolumeSound() => _model.VolumeSound;
    public float VolumeMusic() => _model.VolumeMusic;

    public void SetVolume(float value, AudioType type)
    {
        _model.SetVolume(value, type);
    }

    #endregion
}

public interface ISoundProvider
{
    void Play(string id);
    void PlayOneShot(string id);
    ISound GetSound(string id);
}

public interface ISoundVolumeProvider
{
    public float VolumeSound();
    public float VolumeMusic();

    public void SetVolume(float value, AudioType type);
}
