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
        [SerializeField] PlayerCombatManager CombatManager;


        public Vector3 Value { get; private set; }
        public MovementModifier.MovementType Type { get; private set; }

        private void Awake()
        {
            Type = MovementModifier.MovementType.Knockback;
        }

        private void Start()
        {
            CombatManager = gameObject.GetComponent<PlayerCombatManager>();
        }

        public void AddKnockback(Vector3 Dir)
        {

        }
    }

}
