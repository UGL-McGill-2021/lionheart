using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


namespace Lionheart.Player.Movement
{
    /// <summary>
    /// Author: Ziqi Li, Denis
    /// A script for activate the components of the multiplayer character
    /// </summary>
    public class MultiplayerActivator : MonoBehaviour, IPunInstantiateMagicCallback
    {
        //public Camera cam;
        //public AudioListener aud;
        public List<MonoBehaviour> scripts = new List<MonoBehaviour>();

        public bool hasVibration { get; set; } = true;

        [SerializeField] Dash PlayerDash;
        [SerializeField] GroundPound PlayerGroundPound;
        [SerializeField] Jump PlayerJump;
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] PullDash PlayerPullDash;
        [SerializeField] Rotation PlayerRotation;
        [SerializeField] WalkMotion PlayerWalkMotion;
        [SerializeField] Animator AnimatorController;
        //[SerializeField] SwitchCam PlayerSwitchCam;

        void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
        {
            //if (this.gameObject.GetComponent<PhotonView>().IsMine)
            //{
            //    EnableControls();
            //}
        }

        /// <summary>
        /// Author: Ziqi Li
        /// Function to activate players
        /// </summary>
        public void ActivatePlayer()
        {
            if (this.gameObject.GetComponent<PhotonView>().IsMine)
            {
                EnableControls();
            }
        }

        /// <summary>
        /// Author: Denis
        /// Gets the reference to the scene switch cam
        /// </summary>
        private void Update()
        {
            //if (PlayerSwitchCam == null) PlayerSwitchCam = GameObject.Find("SwitchCam").GetComponent<SwitchCam>();
        }

        /// <summary>
        /// Author: Ziqi, Denis
        /// Blocks the input of the movement scripts
        /// </summary>
        public void DisableControls()
        {
            if (this.gameObject.GetComponent<PhotonView>().IsMine)
            {
                PlayerDash.enabled = false;

                //StopCoroutine(PlayerSwitchCam.WaitForButtonRelease());
                StopCoroutine(PlayerJump.WaitForButtonRelease());
                PlayerGroundPound.BlockInput = true;
                PlayerPullDash.BlockInput = true;
                PlayerJump.BlockInput = true;
                PlayerWalkMotion.BlockInput = true;
                PlayerRotation.BlockInput = true;
                //PlayerSwitchCam.BlockInput = true;

                StopAllCoroutines();
                StartCoroutine(WaitToDisableControls());
            }
        }

        /// <summary>
        /// Author: Denis
        /// Blocks the controller input reading of movement system after 
        /// the player hits the ground. Reset the walk vector to avoid sliding.
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitToDisableControls()
        {
            yield return new WaitUntil(() => PlayerJump.IsGrounded == true);

            PlayerWalkMotion.ResetMovementVector();
            AnimatorController.SetFloat("MoveMagnitude", 0.0f);
        }

        /// <summary>
        /// Author: Ziqi, Denis
        /// Function to enable all attached scripts and their input reading 
        /// </summary>
        public void EnableControls()
        {
            StopAllCoroutines();

            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = true;
            }

            PlayerGroundPound.BlockInput = false;
            PlayerPullDash.BlockInput = false;
            PlayerWalkMotion.BlockInput = false;
            PlayerRotation.BlockInput = false;
            StartCoroutine(PlayerJump.WaitForButtonRelease());
            //if (PlayerSwitchCam != null) StartCoroutine(PlayerSwitchCam.WaitForButtonRelease());
        }
    }
}

