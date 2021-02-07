using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvitationCodePresenter : MonoBehaviour {
    [Header("Invitation Code Textures")]
    public Sprite ButtonXTexture;
    public Sprite ButtonYTexture;
    public Sprite ButtonBTexture;
    public Sprite ButtonATexture;

    [Header("Invitation Code Character Slots")]
    public List<Image> Slots;

    /// <summary>
    /// Author: Feiyang Li
    /// Present invitation code on UI
    /// </summary>
    /// <param name="InvitationCode"></param>
    public void Present(string InvitationCode) {
        Debug.Log("Invitation Code Presented: " + InvitationCode);
        for (int i = 0; i < Slots.Count; i++) {
            if (Slots[i] != null && i < InvitationCode.Length) {
                switch (InvitationCode[i]) {
                    case 'X':
                        Slots[i].sprite = ButtonXTexture;
                        break;
                    case 'Y':
                        Slots[i].sprite = ButtonYTexture;
                        break;
                    case 'A':
                        Slots[i].sprite = ButtonATexture;
                        break;
                    case 'B':
                        Slots[i].sprite = ButtonBTexture;
                        break;
                }
            } else {
                Slots[i].sprite = null;
            }
        }
    }

}
