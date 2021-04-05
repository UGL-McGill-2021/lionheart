using UnityEngine;

public class BackgroundAudioManager : MonoBehaviour {
    [HideInInspector]
    public static BackgroundAudioManager instance;

    [Header("Audio Components")]
    public AudioSource AudioSource;

    [Header("Audio Sources")]
    public AudioClip BackgroundMusic;

    public delegate void BackgroundVolumeChangedDelegate(float Volume);
    public BackgroundVolumeChangedDelegate OnBackgroundVolumeChanged;

    private void Start() {
        if (instance == null) {
            instance = this;
            if (AudioSource.isPlaying) {
                AudioSource.Stop();
                AudioSource.clip = BackgroundMusic;
                AudioSource.loop = true;
                AudioSource.Play();
                OnBackgroundVolumeChanged += UpdateVolume;
            }
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void UpdateVolume(float Volume) {
        this.AudioSource.volume = Volume;
    }

    private void OnDestroy() {
        OnBackgroundVolumeChanged -= UpdateVolume;
    }

}
