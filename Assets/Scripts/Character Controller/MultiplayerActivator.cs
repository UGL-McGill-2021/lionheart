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
        public bool isOffLine;
        public Camera cam;
        public WalkMotion script1;
        public Rotation script2;
        public Vertical script3;
        public Dash script4;
        public MovementHandler script5;


        void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
        {
            if (!isOffLine && this.gameObject.GetComponent<PhotonView>().IsMine)
            {
                cam.enabled = true;
                script1.enabled = true;
                script2.enabled = true;
                script3.enabled = true;
                script4.enabled = true;
                script5.enabled = true;
            }
        }
    }
}

