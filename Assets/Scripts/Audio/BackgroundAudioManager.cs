using UnityEngine;

public class BackgroundAudioManager : MonoBehaviour {
    [HideInInspector]
    public static BackgroundAudioManager instance;

    [Header("Audio Components")]
    public AudioSource AudioSource;

    [Header("Audio Sources")]
    public AudioClip BackgroundMusic;

    private void Start() {
        if (instance == null) {
            instance = this;
            AudioSource.Stop();
            AudioSource.clip = BackgroundMusic;
            AudioSource.loop = true;
            AudioSource.Play();

            VolumeManager.instance.OnBackgroundMusicVolumeChanged += UpdateVolume;

            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void UpdateVolume(float Volume) {
        this.AudioSource.volume = Volume;
    }

    private void OnDestroy() {
        VolumeManager.instance.OnBackgroundMusicVolumeChanged -= UpdateVolume;
    }

}