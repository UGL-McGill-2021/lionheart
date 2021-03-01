using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Author: Ziqi Li
/// Script for sub menu
/// </summary>
public class SubMenuManager : MonoBehaviour
{
    [Header("UI elements")]
    public Button BackButton;
    public SceneLoader SceneLoader;  // scene transition loader
    // Start is called before the first frame update
    void Start()
    {
        if(BackButton != null) BackButton.Select();
    }

    private void Update()
    {
        // make sure the button is always selected despite to mouse input
        if (BackButton != null && !EventSystem.current.IsPointerOverGameObject()) BackButton.Select();
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to back to the main menu
    /// </summary>
    public void Back()
    {
        SceneLoader.LoadSceneWithName("MainMenu");
    }
}
