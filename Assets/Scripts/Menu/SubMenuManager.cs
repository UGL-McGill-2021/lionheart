using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Author: Ziqi Li
/// Script for sub menu
/// </summary>
public class SubMenuManager : MenuManager
{
    [Header("UI elements")]
    public Button BackButton;

    // Start is called before the first frame update
    void Start()
    {
        if (BackButton != null)
        {
            base.DefaultButton = BackButton;  // set the defaultButton in the parent class
            BackButton.Select();
        }
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
    /// Author: Ziqi Li
    /// Function to back to the main menu (call back function setting up in button object)
    /// </summary>
    public void Back()
    {
        SceneLoader.LoadSceneWithName("MainMenu");
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Call back function from input action
    /// </summary>
    public void OnBack()
    {
        PhotonNetwork.Disconnect();  // disconnect from Photon
        Back();
    }
}
