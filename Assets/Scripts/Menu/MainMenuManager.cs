using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Ziqi, Kaya
/// Script for main menu
/// </summary>
public class MainMenuManager : MenuManager {

    [Header("UI elements")]
    public Button StartButton;
    public bool cursor;
    public Slider Music_VolumeSlider;
    public Slider SFX_Slider;
    public Slider VC_Slider;
    public Toggle VibToggle;

    public GameObject OptionGroup;

    private void Awake() {
        // initialize settings
        Music_VolumeSlider.value = PlayerGameSettings.AudioVolume;
        SFX_Slider.value = PlayerGameSettings.SFXVolume;
        VC_Slider.value = PlayerGameSettings.VCVolume;
        VibToggle.isOn = PlayerGameSettings.IsVibrationOn;

        OptionGroup.SetActive(false);

        if (PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();  // make sure disconnecting when finishing game

        if (StartButton != null) {
            base.DefaultButton = StartButton;  // set the defaultButton in the parent class
            StartButton.Select();
        }

        if (!cursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else
            Cursor.visible = true;
    }

    /// <summary>
    /// Author: Feiyang Li
    /// Initialize volume
    /// </summary>
    private void Start() {
        if (VolumeManager.instance.OnBackgroundMusicVolumeChanged != null)
            VolumeManager.instance.OnBackgroundMusicVolumeChanged(PlayerGameSettings.AudioVolume);
        else
            Debug.LogWarning("MainMenuManager: background music volume delegate null");

        if (VolumeManager.instance.OnSFXVolumeChanged != null)
            VolumeManager.instance.OnSFXVolumeChanged(PlayerGameSettings.SFXVolume);
        else
            Debug.LogWarning("MainMenuManager: SFX volume delegate null");

        if (VolumeManager.instance.OnVoiceChatVolumeChanged != null)
            VolumeManager.instance.OnVoiceChatVolumeChanged(PlayerGameSettings.VCVolume);
        else
            Debug.LogWarning("MainMenuManager: VC volume delegate null");
    }

    /// <summary>
    /// Author: Ziqi
    /// An Update function that override (extend) the parent class's Update function
    /// </summary>
    protected override void Update() {
        base.Update();
    }

    /// <summary>
    /// Author: Kaya
    /// </summary>
    public void Play() {
        SceneLoader.LoadSceneWithName("Matching");
    }

    /// <summary>
    /// Author: Kaya
    /// </summary>
    public void Quit() {
        Application.Quit();
    }

    /// <summary>
    /// Author: Kaya
    /// </summary>
    public void URL(string url) {
        Application.OpenURL(url);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function for loading credits scene
    /// </summary>
    public void Credits() {
        SceneLoader.LoadSceneWithName("Credits");
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function for loading controls scene
    /// </summary>
    public void Controls() {
        SceneLoader.LoadSceneWithName("Controls");
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function for loading controls scene
    /// </summary>
    public void ConceptArt() {
        SceneLoader.LoadSceneWithName("ConceptArt");
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Slider callback function for music volume slider
    /// </summary>
    public void ChangeVolume() {
        PlayerGameSettings.AudioVolume = Music_VolumeSlider.value;
        if (VolumeManager.instance.OnBackgroundMusicVolumeChanged != null)
            VolumeManager.instance.OnBackgroundMusicVolumeChanged(Music_VolumeSlider.value);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Slider callback function for SFX volume slider
    /// </summary>
    public void ChangeSFXVolume() {
        PlayerGameSettings.SFXVolume = SFX_Slider.value;
        if (VolumeManager.instance.OnSFXVolumeChanged != null)
            VolumeManager.instance.OnSFXVolumeChanged(SFX_Slider.value);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Slider callback function for voice chat volume slider
    /// </summary>
    public void ChangeVCVolume() {
        PlayerGameSettings.VCVolume = VC_Slider.value;
        if (VolumeManager.instance.OnVoiceChatVolumeChanged != null)
            VolumeManager.instance.OnVoiceChatVolumeChanged(VC_Slider.value);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Toggle callback function for vibration toggle
    /// </summary>
    public void TurnVibration() {
        PlayerGameSettings.IsVibrationOn = VibToggle.isOn;
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Toggle callback function for option button
    /// </summary>
    public void Options() {
        OptionGroup.SetActive(!OptionGroup.activeSelf);
    }
}
