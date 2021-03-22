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
        public List<MonoBehaviour> scripts = new List<MonoBehaviour>();

        public bool hasVibration { get; set; } = true;

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
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
        }

        public void EnableControls()
        {
            foreach(MonoBehaviour script in scripts)
            {
                script.enabled = true;
            }
        }
    }
}

