using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        BackButton.Select();
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
