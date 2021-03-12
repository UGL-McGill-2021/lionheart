using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/// <summary>
/// Author: Ziqi
/// Abstract parent class for MenuManagers
/// </summary>
public abstract class MenuManager : MonoBehaviour
{
    public SceneLoader SceneLoader;  // scene transition loader
    protected Button DefaultButton;  // the defaultly selected button

    /// <summary>
    /// Author: Ziqi
    /// A virtual Update function that can be overrided (extended) by children class
    /// </summary>
    protected virtual void Update()
    {
        // make sure the button is always selected despite to mouse interference
        if (DefaultButton != null && !EventSystem.current.currentSelectedGameObject) DefaultButton.Select();
    }
}
