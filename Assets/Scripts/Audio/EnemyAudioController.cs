using Photon.Pun;
using UnityEngine;

public enum EnemySFX {
    SHOOT_PROJECTILE = 0,
    SLAP = 1
}

public class EnemyAudioController : MonoBehaviour {
    public AudioSource source;

    [Header("Audio Clips")]
    public AudioClip ShootProjectile;

    public AudioClip Slap;

    [Header("Network")]
    public PhotonView view;

    public void Start() {
        if (source.isPlaying) {
            source.Stop();
        }

        VolumeManager.instance.OnSFXVolumeChanged += UpdateVolume;
    }

    public void TriggerPlaySFXOnAll(int SFXIndex) {
        view.RPC("PlaySFXByIndex", RpcTarget.All, SFXIndex);
    }

    [PunRPC]
    public void PlaySFXByIndex(int SFXIndex) {
        EnemySFX _RequestedSFX = (EnemySFX)SFXIndex;
        PlaySFX(_RequestedSFX);
    }

    public void PlaySFX(EnemySFX requestedSFX) {
        switch (requestedSFX) {
            case EnemySFX.SHOOT_PROJECTILE: {
                    source.PlayOneShot(ShootProjectile);
                    break;
                }

            case EnemySFX.SLAP: {
                    source.PlayOneShot(Slap);
                    break;
                }
        }

        Debug.Log("Enemy SFX Sound Played!");
    }

    public void TriggerStopSFXOnAll() {
        view.RPC("StopSFX", RpcTarget.All);
    }

    [PunRPC]
    public void StopSFX() {
        source.Stop();
    }

    public void UpdateVolume(float Volume) {
        source.volume = Volume;
    }

    private void OnDestroy() {
        if (VolumeManager.instance.OnSFXVolumeChanged != null)
            VolumeManager.instance.OnSFXVolumeChanged -= UpdateVolume;
    }

}