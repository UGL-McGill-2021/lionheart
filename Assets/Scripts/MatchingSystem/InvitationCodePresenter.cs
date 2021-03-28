using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InvitationCodePresenter : MonoBehaviour {
    [Header("Invitation Code Textures")]
    public Sprite XBOX_ButtonXTexture;
    public Sprite XBOX_ButtonYTexture;
    public Sprite XBOX_ButtonBTexture;
    public Sprite XBOX_ButtonATexture;
    public Sprite PS4_ButtonXTexture;
    public Sprite PS4_ButtonYTexture;
    public Sprite PS4_ButtonBTexture;
    public Sprite PS4_ButtonATexture;
    public Sprite EmptyTexture;

    [Header("Invitation Code Character Slots")]
    public List<Image> FIRST_Slots;
    public List<Image> SECOND_Slots;

    /// <summary>
    /// Author: Feiyang Li, Ziqi
    /// Present invitation code on UI
    /// </summary>
    /// <param name="InvitationCode"></param>
    public void Present(string InvitationCode) {
        //Debug.Log("Invitation Code Presented: " + InvitationCode);
        for (int i = 0; i < FIRST_Slots.Count; i++) {
            if (FIRST_Slots[i] != null && i < InvitationCode.Length) {
                switch (InvitationCode[i]) {
                    case 'X':
                        // if the second slot list is not fully used, we present the code depending on controller type
                        // (player enters room code case)
                        if (SECOND_Slots.Count != FIRST_Slots.Count)
                        {
                            if (Gamepad.current.name == ControllerIconSwitcher.PS4_controllerName) FIRST_Slots[i].sprite = PS4_ButtonXTexture;
                            else FIRST_Slots[i].sprite = XBOX_ButtonXTexture;
                        }
                        else  // (system presenting room code case)
                        {
                            FIRST_Slots[i].sprite = XBOX_ButtonXTexture;
                            SECOND_Slots[i].sprite = PS4_ButtonXTexture;
                        }
                        break;
                    case 'Y':
                        // if the second slot list is not used, we present the code depending on controller type
                        // (player enters room code case)
                        if (SECOND_Slots.Count != FIRST_Slots.Count)
                        {
                            if (Gamepad.current.name == ControllerIconSwitcher.PS4_controllerName) FIRST_Slots[i].sprite = PS4_ButtonYTexture;
                            else FIRST_Slots[i].sprite = XBOX_ButtonYTexture;
                        }
                        else  // (system presenting room code case)
                        {
                            FIRST_Slots[i].sprite = XBOX_ButtonYTexture;
                            SECOND_Slots[i].sprite = PS4_ButtonYTexture;
                        }
                        break;
                    case 'A':
                        if (SECOND_Slots.Count != FIRST_Slots.Count)
                        {
                            if (Gamepad.current.name == ControllerIconSwitcher.PS4_controllerName) FIRST_Slots[i].sprite = PS4_ButtonATexture;
                            else FIRST_Slots[i].sprite = XBOX_ButtonATexture;
                        }
                        else  // (system presenting room code case)
                        {
                            FIRST_Slots[i].sprite = XBOX_ButtonATexture;
                            SECOND_Slots[i].sprite = PS4_ButtonATexture;
                        }
                        break;
                    case 'B':
                        if (SECOND_Slots.Count != FIRST_Slots.Count)
                        {
                            if (Gamepad.current.name == ControllerIconSwitcher.PS4_controllerName) FIRST_Slots[i].sprite = PS4_ButtonBTexture;
                            else FIRST_Slots[i].sprite = XBOX_ButtonBTexture;
                        }
                        else  // (system presenting room code case)
                        {
                            FIRST_Slots[i].sprite = XBOX_ButtonBTexture;
                            SECOND_Slots[i].sprite = PS4_ButtonBTexture;
                        }
                        break;
                }
            } else {
                FIRST_Slots[i].sprite = EmptyTexture;
                if (SECOND_Slots.Count == FIRST_Slots.Count) SECOND_Slots[i].sprite = EmptyTexture;
            }
        }
    }

}
