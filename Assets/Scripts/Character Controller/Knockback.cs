using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lionheart.Player.Movement
{
    /// <summary>
    /// Author: Denis
    /// Class that applies a hit vector to the player
    /// </summary>
    public class Knockback : MonoBehaviour, MovementModifier
    {
        [Header("References")]
        [SerializeField] public MovementHandler PlayerMovementHandler;
        [SerializeField] public PlayerCombatManager CombatManager;
        [SerializeField] public Jump PlayerJump;
        [SerializeField] public Rotation PlayerRotation;
        [SerializeField] public Animator AnimatorController;
        [SerializeField] public MultiplayerActivator PlayerMultiplayer;
        [SerializeField] public Gamepad Controller;

        [Header("Parameters")]
        [SerializeField] private float BulletHitForce = 0.005f;
        [SerializeField] private float GruntSmashForce = 15f;
        [SerializeField] private float VerticalForce = 15f;

        [Header("State")]
        [SerializeField] private bool WasHit;
        [SerializeField] public bool IsKnockback;
        [SerializeField] public bool TookOff;

        private int HitCount = 0;

        public Vector3 Value { get; private set; }
        public MovementModifier.MovementType Type { get; private set; }

        /// <summary>
        /// Author: Denis
        /// Initial setup
        /// </summary>
        private void Awake()
        {
            Type = MovementModifier.MovementType.Knockback;
        }

        /// <summary>
        /// Author: Denis
        /// Adding this modifier to the movement modifiers list
        /// </summary>
        private void OnEnable()
        {
            PlayerMovementHandler.AddModifier(this);
        }

        /// <summary>
        /// Author: Denis
        /// Removing this modifier from the movement modifiers list
        /// </summary>
        private void OnDisable()
        {
            PlayerMovementHandler.RemoveModifier(this);
        }

        /// <summary>
        /// Author: Denis
        /// Caching components
        /// </summary>
        private void Start()
        {
            CombatManager = gameObject.GetComponent<PlayerCombatManager>();
            PlayerJump = gameObject.GetComponent<Jump>();
            PlayerRotation = gameObject.GetComponent<Rotation>();
            PlayerMultiplayer = gameObject.GetComponent<MultiplayerActivator>();
            WasHit = false;
            IsKnockback = false;
            TookOff = false;
        }

        /// <summary>
        /// Author: Denis
        /// Applies a bullet hit knockback 
        /// </summary>
        /// <param name="Dir"></param>
        public void AddKnockback(Vector3 Dir)
        {
            PlayerRotation.enabled = false;

            Value = new Vector3(BulletHitForce * Dir.x, VerticalForce, BulletHitForce * Dir.z);
            
            WasHit = true;
            IsKnockback = true;
            StartCoroutine(TakeOffTimer());

            if (PlayerMultiplayer.hasVibration == true)
            {
                StartCoroutine(PlayHaptics());
            }
        }

        /// <summary>
        /// Author: Denis
        /// Applies a grunt smash hit knockback to the player
        /// The knockback vector is more intense the closer the player and the grunt are
        /// </summary>
        /// <param name="Force"></param>
        /// <param name="Radius"></param>
        /// <param name="V0"></param>
        public void AddExplosiveKnockback(float Force, float Radius, Vector3 V0)
        {
            Vector3 Dir = gameObject.transform.position-V0;
            float Scale = GruntSmashForce / Dir.magnitude;

            PlayerRotation.enabled = false;

            Value = new Vector3(Scale * Dir.x, VerticalForce, Scale * Dir.z);
            
            WasHit = true;
            IsKnockback = true;
            StartCoroutine(TakeOffTimer());

            if (PlayerMultiplayer.hasVibration == true)
            {
                StartCoroutine(PlayHaptics());
            }
        }

        /// <summary>
        /// Author: Denis
        /// Controls the start of the animation and the knockback end logic
        /// </summary>
        private void FixedUpdate()
        {
            //play the animation after getting hit
            if (WasHit == true)
            {
                AnimatorController.SetBool("IsKnockback", true);
                StartCoroutine(AnimationTrigger("IsKnockback"));
                WasHit = false;
            }

            //landing on a knockback restores movement control
            if (HitCount > 0 && PlayerJump.IsGrounded == true)
            {
                HitCount = 0;
                Value = Vector3.zero;

                IsKnockback = false;
                TookOff = false;
                PlayerRotation.enabled = true;
            }
        }

        /// <summary>
        /// Author: Denis
        /// This coroutine prevents triggering the the landing animation too early after getting hit
        /// </summary>
        /// <returns></returns>
        private IEnumerator TakeOffTimer()
        {
            yield return new WaitForSecondsRealtime(0.3f);
            HitCount++;
            TookOff = true;
        }

        /// <summary>
        /// Author: Denis, Ziqi
        /// Simple Rumble feedback on knockback
        /// </summary>
        /// <returns></returns>
        IEnumerator PlayHaptics()
        {
            Gamepad.current.SetMotorSpeeds(0.2f, 0.9f);
            yield return new WaitForSecondsRealtime(0.1f);
            Gamepad.current.ResetHaptics();
        }

        /// <summary>
        /// Author: Denis
        /// Simulates animation trigger for bools
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        IEnumerator AnimationTrigger(string Name)
        {
            yield return new WaitForSecondsRealtime(0.5f);

            switch (Name)
            {
                case "IsKnockback":
                    AnimatorController.SetBool("IsKnockback", false);
                    break;
            }
        }
    }
}
