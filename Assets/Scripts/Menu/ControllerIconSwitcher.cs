using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

/// <summary>
/// Author: Ziqi
/// Script for changing the UI controller icons depending on the connected controller type
/// </summary>
public class ControllerIconSwitcher : MonoBehaviour
{
    public List<Image> IconImageList = new List<Image>(); // these images use xbox sprites by default
    [Header("PS4 icons")]
    public Sprite PS4_NORTH;
    public Sprite PS4_SOUTH;
    public Sprite PS4_EAST;
    public Sprite PS4_WEST;
    public Sprite PS4_UP;
    public Sprite PS4_DOWN;
    public Sprite PS4_LEFT;
    public Sprite PS4_RIGHT;
    public Sprite PS4_L1;
    public Sprite PS4_L2;
    public Sprite PS4_R1;
    public Sprite PS4_R2;
    public Sprite PS4_Menu;
    [Header("XBOX icons")]
    public Sprite XBOX_NORTH;
    public Sprite XBOX_SOUTH;
    public Sprite XBOX_EAST;
    public Sprite XBOX_WEST;
    public Sprite XBOX_UP;
    public Sprite XBOX_DOWN;
    public Sprite XBOX_LEFT;
    public Sprite XBOX_RIGHT;
    public Sprite XBOX_L1;
    public Sprite XBOX_L2;
    public Sprite XBOX_R1;
    public Sprite XBOX_R2;
    public Sprite XBOX_Menu;

    private string PreviousGampad;
    public const string PS4_controllerName = "DualShock4GamepadHID";

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null && PreviousGampad != Gamepad.current.name)
        {
            UpdateIcons(Gamepad.current.name);
            PreviousGampad = Gamepad.current.name;
        }
    }


    /// <summary>
    /// Author: Ziqi
    /// Update Icons depending on the controller type
    /// </summary>
    void UpdateIcons(string controllerType)
    {
        // switch the icons to PS4 icons
        if(controllerType == PS4_controllerName)
        {
            foreach (Image icon in IconImageList)
            {
                if(icon != null)
                {
                    string iconSpriteName = icon.sprite.name;
                    if (iconSpriteName == XBOX_NORTH.name) icon.sprite = PS4_NORTH;
                    else if (iconSpriteName == XBOX_SOUTH.name) icon.sprite = PS4_SOUTH;
                    else if (iconSpriteName == XBOX_EAST.name) icon.sprite = PS4_EAST;
                    else if (iconSpriteName == XBOX_WEST.name) icon.sprite = PS4_WEST;
                    else if (iconSpriteName == XBOX_UP.name) icon.sprite = PS4_UP;
                    else if (iconSpriteName == XBOX_DOWN.name) icon.sprite = PS4_DOWN;
                    else if (iconSpriteName == XBOX_LEFT.name) icon.sprite = PS4_LEFT;
                    else if (iconSpriteName == XBOX_RIGHT.name) icon.sprite = PS4_RIGHT;
                    else if (iconSpriteName == XBOX_L1.name) icon.sprite = PS4_L1;
                    else if (iconSpriteName == XBOX_L2.name) icon.sprite = PS4_L2;
                    else if (iconSpriteName == XBOX_R1.name) icon.sprite = PS4_R1;
                    else if (iconSpriteName == XBOX_R2.name) icon.sprite = PS4_R2;
                    else if (iconSpriteName == XBOX_Menu.name) icon.sprite = PS4_Menu;
                    else Debug.Log("ERROR in controller icon initial assignment");
                }
            }
        }
        else  // switch to XBOX icons
        {
            foreach (Image icon in IconImageList)
            {
                if (icon != null)
                {
                    string iconSpriteName = icon.sprite.name;
                    if (iconSpriteName == PS4_NORTH.name) icon.sprite = XBOX_NORTH;
                    else if (iconSpriteName == PS4_SOUTH.name) icon.sprite = XBOX_SOUTH;
                    else if (iconSpriteName == PS4_EAST.name) icon.sprite = XBOX_EAST;
                    else if (iconSpriteName == PS4_WEST.name) icon.sprite = XBOX_WEST;
                    else if (iconSpriteName == PS4_UP.name) icon.sprite = XBOX_UP;
                    else if (iconSpriteName == PS4_DOWN.name) icon.sprite = XBOX_DOWN;
                    else if (iconSpriteName == PS4_LEFT.name) icon.sprite = XBOX_LEFT;
                    else if (iconSpriteName == PS4_RIGHT.name) icon.sprite = XBOX_RIGHT;
                    else if (iconSpriteName == PS4_L1.name) icon.sprite = XBOX_L1;
                    else if (iconSpriteName == PS4_L2.name) icon.sprite = XBOX_L2;
                    else if (iconSpriteName == PS4_R1.name) icon.sprite = XBOX_R1;
                    else if (iconSpriteName == PS4_R2.name) icon.sprite = XBOX_R2;
                    else if (iconSpriteName == PS4_Menu.name) icon.sprite = XBOX_Menu;
                }
            }
        }
    }
}
