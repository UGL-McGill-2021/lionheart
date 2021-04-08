using UnityEngine;

public class PlayerVoiceVoluimeController : MonoBehaviour {
    [Header("Voice Chat Componenets")]
    public AudioSource PlayerVoiceSource;

    private void Start() {
        PlayerVoiceSource.volume = VolumeManager.instance.VoiceChatVolume;
        VolumeManager.instance.OnVoiceChatVolumeChanged += UpdateVoiceChatVolume;
    }

    public void UpdateVoiceChatVolume(float Volume) {
        this.PlayerVoiceSource.volume = Volume;
    }

    private void OnDestroy() {
        VolumeManager.instance.OnVoiceChatVolumeChanged -= UpdateVoiceChatVolume;
    }
}
