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
        public bool IgnoreControlInput;

        private bool CoroutineRunning = false;

        void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
        {
            if (this.gameObject.GetComponent<PhotonView>().IsMine)
            {
                //cam.enabled = true;
                aud.enabled = true;
                EnableControls();
            }
        }

        /// <summary>
        /// Author: Ziqi, Denis
        /// Function to disable all attached scripts
        /// </summary>
        public void DisableControls()
        {
            gameObject.GetComponent<Rotation>().enabled = false;
            gameObject.GetComponent<WalkMotion>().enabled = false;
            gameObject.GetComponent<Dash>().enabled = false;

            gameObject.GetComponent<GroundPound>().BlockInput = true;
            gameObject.GetComponent<PullDash>().BlockInput = true;
            gameObject.GetComponent<Jump>().BlockInput = true;

            StopAllCoroutines();
            StartCoroutine(WaitToDisableControls());
        }

        /// <summary>
        /// Author: Denis
        /// Disables the movement system after the player hits the ground
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitToDisableControls()
        {
            yield return new WaitUntil(() => gameObject.GetComponent<Jump>().IsGrounded == true);

            IgnoreControlInput = true;

            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }

            gameObject.GetComponent<MovementHandler>().enabled = true;
            //gameObject.GetComponent<Jump>().enabled = true;
        }

        /// <summary>
        /// Author: Ziqi
        /// Function to enable all attached scripts
        /// </summary>
        public void EnableControls()
        {
            StopAllCoroutines();

            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = true;
            }

            IgnoreControlInput = false;

            gameObject.GetComponent<GroundPound>().BlockInput = false;
            gameObject.GetComponent<PullDash>().BlockInput = false;
            gameObject.GetComponent<Jump>().BlockInput = false;
        }
    }
}

