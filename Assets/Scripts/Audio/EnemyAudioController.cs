using Photon.Pun;
using UnityEngine;

public enum EnemySFX {
    SHOOT_PROJECTILE = 0,
    SLAP = 1,
    IMPACT = 2
}

public class EnemyAudioController : MonoBehaviour {
    public AudioSource source;

    [Header("Audio Clips")]
    public AudioClip ShootProjectile;

    public AudioClip Slap;

    public AudioClip Impact1;
    public AudioClip Impact2;

    [Header("Network")]
    public PhotonView view;

    public void Start() {
        if (source.isPlaying) {
            source.Stop();
        }

        source.volume = VolumeManager.instance.SFXVolume;
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

            case EnemySFX.IMPACT: {
                    source.pitch = 1 + Random.Range(-0.5f, 0.5f);
                    if (Random.Range(0, 10) < 5) {
                        source.PlayOneShot(Impact1);
                    } else {
                        source.PlayOneShot(Impact2);
                    }
                    break;
                }
        }
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