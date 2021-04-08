using UnityEngine;
using Photon.Pun;

public enum PlayerSFX {
    JUMPLAND = 0,
    GROUND_POUND = 1,
    DASH = 2,
    IMPACT = 3
}

public class PlayerAudioController : MonoBehaviour {
    public AudioSource source;

    [Header("Audio Clips")]
    public float JumpLandingVolume = 0.5f;
    public AudioClip JumpLanding;

    public float GroundPoundVolume = 0.5f;
    public AudioClip GroundPound;

    public float DashVolume = 0.5f;
    public AudioClip Dash;

    public float ImpactVolume = 0.5f;
    public AudioClip Impact1;
    public AudioClip Impact2;

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
        PlayerSFX _RequestedSFX = (PlayerSFX)SFXIndex;
        PlaySFX(_RequestedSFX);
    }

    public void PlaySFX(PlayerSFX requestedSFX) {
        switch (requestedSFX) {
            case PlayerSFX.DASH: {
                    //source.PlayOneShot(Dash);
                    Debug.Log("Dash sound played");
                    break;
                }

            case PlayerSFX.GROUND_POUND: {
                    source.PlayOneShot(GroundPound);
                    break;
                }

            case PlayerSFX.IMPACT: {
                    source.pitch = 1 + Random.Range(-0.5f, 0.5f);
                    if (Random.Range(0, 10) < 5) {
                        source.PlayOneShot(Impact1);
                    } else {
                        source.PlayOneShot(Impact2);
                    }

                    break;
                }

            case PlayerSFX.JUMPLAND: {
                    source.pitch = 1;
                    source.PlayOneShot(JumpLanding);
                    break;
                }

            default:
                Debug.LogWarning("Requested SFX not found");
                break;
        }

        Debug.Log("SFX Sound Played!");
    }

    public void UpdateVolume(float Volume) {
        source.volume = Volume;
    }

    private void OnDestroy() {
        VolumeManager.instance.OnSFXVolumeChanged -= UpdateVolume;
    }
}