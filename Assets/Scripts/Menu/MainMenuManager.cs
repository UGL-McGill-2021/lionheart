using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        StartButton.Select();

        if (!cursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
            Cursor.visible = true;
    }


    public void Play()
    {
        SceneLoader.LoadSceneWithName("Matching");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void URL(string url)
    {
        Application.OpenURL(url);
    }

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
