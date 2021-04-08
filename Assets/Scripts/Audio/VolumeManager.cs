using UnityEngine;

public class VolumeManager : MonoBehaviour {
    public static VolumeManager instance;

    public float BackgroundMusicVolume;
    public float SFXVolume;
    public float VoiceChatVolume;

    public delegate void BackgroundMusicVolumeDelegate(float NewBackgrounVolume);
    public BackgroundMusicVolumeDelegate OnBackgroundMusicVolumeChanged;

    public delegate void SFXVolumeDelegate(float NewSFXVolume);
    public SFXVolumeDelegate OnSFXVolumeChanged;

    public delegate void VoiceChatVolumeDelegate(float NewVoiceChatVolumeDelegate);
    public VoiceChatVolumeDelegate OnVoiceChatVolumeChanged;

    void Awake() {
        if (instance == null) {
            instance = this;

            OnBackgroundMusicVolumeChanged += UpdateBackgroundMusicVolume;
            OnSFXVolumeChanged += UpdateSFXVolume;
            OnVoiceChatVolumeChanged += UpdateVoiceChatVolume;
        }
    }

    public void RefreshVolume() {
        OnBackgroundMusicVolumeChanged(BackgroundMusicVolume);
        OnSFXVolumeChanged(SFXVolume);
        OnVoiceChatVolumeChanged(VoiceChatVolume);
    }

    public void UpdateBackgroundMusicVolume(float NewBackgrounVolume) {
        this.BackgroundMusicVolume = NewBackgrounVolume;
    }

    public void UpdateSFXVolume(float NewSFXVolume) {
        this.SFXVolume = NewSFXVolume;
    }

    public void UpdateVoiceChatVolume(float NewVCVolume) {
        this.VoiceChatVolume = NewVCVolume;
    }
}