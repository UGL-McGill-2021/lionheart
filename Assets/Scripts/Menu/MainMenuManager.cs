using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Author: Ziqi, Kaya
/// Script for main menu
/// </summary>
public class MainMenuManager : MenuManager
{

    [Header("UI elements")]
    public Button StartButton;
    public bool cursor;
    public Slider VolumeSlider;

    private void Awake()
    {
        VolumeSlider.value = PlayerGameSettings.AudioVolume;
        if (StartButton != null)
        {
            base.DefaultButton = StartButton;  // set the defaultButton in the parent class
            StartButton.Select();
        }

        if (!cursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
            Cursor.visible = true;
    }

    /// <summary>
    /// Author: Ziqi
    /// An Update function that override (extend) the parent class's Update function
    /// </summary>
    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Author: Kaya
    /// </summary>
    public void Play()
    {
        SceneLoader.LoadSceneWithName("Matching");
    }

    /// <summary>
    /// Author: Kaya
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Author: Kaya
    /// </summary>
    public void URL(string url)
    {
        Application.OpenURL(url);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function for loading credits scene
    /// </summary>
    public void Credits()
    {
        SceneLoader.LoadSceneWithName("Credits");
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function for loading controls scene
    /// </summary>
    public void Controls()
    {
        SceneLoader.LoadSceneWithName("Controls");
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Slider callback function for volume slider
    /// </summary>
    public void ChangeVolume()
    {
        PlayerGameSettings.AudioVolume = VolumeSlider.value;
    }
}
