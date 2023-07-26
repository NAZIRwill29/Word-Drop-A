using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public AudioSource mainCameraAudioSource;
    public AudioClip mainCameraAudioClip;
    public float musicVolume = 1, soundVolume = 1;
    public bool isSoundOn = true, isMusicOn = true;
    private bool isMusicTemp;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ChangeMusicBackground(bool isInGame, int index)
    {
        Debug.Log("change music background");
        if (isInGame)
            mainCameraAudioSource.clip = GameManager.instance.inGame.inGameAudioClip[index];
        else
            mainCameraAudioSource.clip = mainCameraAudioClip;
        mainCameraAudioSource.Play();
    }

    //turn off/on when loading
    public void TurnOnMusicDueLoading(bool isOn)
    {
        if (!isOn)
        {
            //store prev value
            isMusicTemp = isMusicOn;
            TurnOnMusicVolume(false);
        }
        else
        {
            if (isMusicTemp)
                TurnOnMusicVolume(true);
            else
                TurnOnMusicVolume(false);
        }
    }

    public void TurnOnMusicVolume(bool isOn)
    {
        isMusicOn = isOn;
        //Debug.Log("change isMusicOn = " + isMusicOn);
        MuteMusicVolumeSystem();
    }
    public void MuteMusicVolumeSystem()
    {
        mainCameraAudioSource.mute = !isMusicOn;
    }

    public void TurnOnSoundVolume(bool isOn)
    {
        isSoundOn = isOn;
        MuteSoundVolumeEvent();
    }
    public void MuteSoundVolumeEvent()
    {
        if (GameManager.instance.inGame)
            GameManager.instance.inGame.TurnOnOffInGameSound(!isSoundOn);
        GameManager.instance.player.playerAudioSource.mute = !isSoundOn;
        GameManager.instance.gameMenuUi.gameMenuUiAudioSource.mute = !isSoundOn;
        GameManager.instance.mainMenuUI.mainMenuUIAudioSource.mute = !isSoundOn;
    }

    //change music volume
    public void ChangeMusicVolumeSystem(float volume)
    {
        musicVolume = volume;
        MusicSystem();
    }

    //change sound volume for all
    public void ChangeSoundVolumeSystem(float volume)
    {
        soundVolume = volume;
        SoundSystem();
    }

    public void MusicSystem()
    {
        mainCameraAudioSource.volume = musicVolume;
        MuteMusicVolumeSystem();
    }

    public void SoundSystem()
    {
        if (GameManager.instance.inGame)
            GameManager.instance.inGame.ChangeSoundVolume(soundVolume);
        GameManager.instance.player.playerAudioSource.volume = soundVolume;
        GameManager.instance.gameMenuUi.gameMenuUiAudioSource.volume = soundVolume;
        GameManager.instance.mainMenuUI.mainMenuUIAudioSource.volume = soundVolume;
        MuteSoundVolumeEvent();
    }

    // used when set according to savedData
    public void UpdateMenuVolumeSetting()
    {
        GameManager.instance.mainMenuUI.UpdateSoundSetting(musicVolume, soundVolume, isMusicOn, isSoundOn);
        GameManager.instance.gameMenuUi.UpdateSoundSetting(musicVolume, soundVolume, isMusicOn, isSoundOn);
    }
}
