using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConceptArtMenuManager : MenuManager
{
    [Header("UI elements")]
    public Button NextButton;
    public Button PreviousButton;
    public Button BackButton;

    // Start is called before the first frame update
    void Start()
    {
        if (BackButton != null)
        {
            base.DefaultButton = NextButton;  // set the defaultButton in the parent class
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
    /// Function to back to the main menu
    /// </summary>
    public void Back()
    {
        SceneLoader.LoadSceneWithName("MainMenu");
    }
}
