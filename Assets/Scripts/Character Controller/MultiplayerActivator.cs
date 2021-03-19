using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


namespace Lionheart.Player.Movement
{
    /// <summary>
    /// Author: Ziqi Li
    /// A script for activate the components of the multiplayer character
    /// </summary>
    public class MultiplayerActivator : MonoBehaviour, IPunInstantiateMagicCallback
    {
        public Camera cam;
        public AudioListener aud;
        public WalkMotion script1;
        public Rotation script2;
        public Jump script3;
        public Dash script4;
        public MovementHandler script5;
        public PullDash script6;
        public PlayerCombatManager script7;
        public GroundPound script8;

        void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
        {
            if (this.gameObject.GetComponent<PhotonView>().IsMine)
            {
                //cam.enabled = true;
                aud.enabled = true;
                EnableControls();
            }
        }

        public void DisableControls()
        {
            script1.enabled = false;
            script2.enabled = false;
            script3.enabled = false;
            script4.enabled = false;
            script5.enabled = false;
            script6.enabled = false;
            script7.enabled = false; 
            script8.enabled = false;
        }

        public void EnableControls()
        {
            script1.enabled = true;
            script2.enabled = true;
            script3.enabled = true;
            script4.enabled = true;
            script5.enabled = true;
            script6.enabled = true;
            script7.enabled = true;
            script8.enabled = true;
        }
    }
}

