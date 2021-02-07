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
        if (InvitationCode.Length != Slots.Count) {
            Debug.LogError("Networking(ERROR) - InvitationCodePresenter: Invalid Invitation Code " + InvitationCode + " , bad length");
            return;
        }

        for (int i = 0; i < InvitationCode.Length; i++) {
            if (Slots[i] != null) {
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
            }
        }
    }

}
