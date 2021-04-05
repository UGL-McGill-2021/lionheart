using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lionheart.Player.Movement
{
    /// <summary>
    /// Author: Denis
    /// Class that applies a hit vector to the player
    /// </summary>
    public class Knockback : MonoBehaviour, MovementModifier
    {
        [Header("References")]
        [SerializeField] MovementHandler PlayerMovementHandler;
        [SerializeField] PlayerCombatManager CombatManager;
        [SerializeField] Jump PlayerJump;
        [SerializeField] Rotation PlayerRotation;
        [SerializeField] Animator AnimatorController;

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

        private void Start()
        {
            CombatManager = gameObject.GetComponent<PlayerCombatManager>();
            PlayerJump = gameObject.GetComponent<Jump>();
            PlayerRotation = gameObject.GetComponent<Rotation>();
            WasHit = false;
            IsKnockback = false;
            TookOff = false;
        }

        public void AddKnockback(Vector3 Dir)
        {
            PlayerRotation.enabled = false;
            Value = new Vector3(BulletHitForce * Dir.x, VerticalForce, BulletHitForce * Dir.z);
            WasHit = true;
            IsKnockback = true;
            StartCoroutine(TakeOffTimer());
        }

        public void AddExplosiveKnockback(float Force, float Radius, Vector3 V0)
        {
            Vector3 Dir = gameObject.transform.position-V0;
            float Scale = GruntSmashForce / Dir.magnitude;
            PlayerRotation.enabled = false;
            Value = new Vector3(Scale * Dir.x, VerticalForce, Scale * Dir.z);
            WasHit = true;
            IsKnockback = true;
            StartCoroutine(TakeOffTimer());
        }

        private void FixedUpdate()
        {
            if (WasHit == true)
            {
                AnimatorController.SetBool("IsKnockback", true);
                StartCoroutine(AnimationTrigger("IsKnockback"));
                WasHit = false;
            }

            //landing 
            if (HitCount > 0 && PlayerJump.IsGrounded == true)
            {
                HitCount = 0;
                Value = Vector3.zero;
                IsKnockback = false;
                TookOff = false;
                PlayerRotation.enabled = true;
            }
        }

        private IEnumerator TakeOffTimer()
        {
            yield return new WaitForSecondsRealtime(0.3f);
            HitCount++;
            TookOff = true;
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
