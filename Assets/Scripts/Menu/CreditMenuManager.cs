using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Author: Ziqi Li
/// Script for credit menu
/// </summary>
public class CreditMenuManager : MenuManager
{
    [Header("UI elements")]
    public Button NextButton;
    public Button PreviousButton;
    public Button BackButton;

    public List<GameObject> TextGroupList = new List<GameObject>();  // list store the parent gameObject of texts

    private int CurrentPage = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (NextButton != null)
        {
            base.DefaultButton = NextButton;  // set the defaultButton in the parent class
            NextButton.Select();
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
    /// Function to show next page (call back function setting up in button object)
    /// </summary>
    public void Next()
    {
        if (CurrentPage < TextGroupList.Count - 1)
        {
            // disable current page
            TextGroupList[CurrentPage].gameObject.SetActive(false);
            CurrentPage++;
            // enable next page
            TextGroupList[CurrentPage].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to show previous page (call back function setting up in button object)
    /// </summary>
    public void Previous()
    {
        if (CurrentPage > 0)
        {
            // disable current page
            TextGroupList[CurrentPage].gameObject.SetActive(false);
            CurrentPage--;
            // enable next page
            TextGroupList[CurrentPage].gameObject.SetActive(true);
        }
    }
}
