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
public class MainMenuManager : MonoBehaviour
{

    [Header("UI elements")]
    public Button StartButton;
    public SceneLoader SceneLoader;  // scene transition loader
    public bool cursor;

    private void Awake()
    {
        if (StartButton != null) StartButton.Select();

        if (!cursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
            Cursor.visible = true;
    }

    private void Update()
    {
        // make sure the button is always selected despite having mouse interference
        if (StartButton != null && !EventSystem.current.currentSelectedGameObject) StartButton.Select();
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
}
