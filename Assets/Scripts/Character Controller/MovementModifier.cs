using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lionheart.Player.Movement
{
    /// <summary>
    /// Auhtor: Denis
    /// Interface for movement modifiers
    /// </summary>
    public interface MovementModifier
    {
        public Vector3 Value { get; }
        public MovementType Type { get; }
        public enum MovementType
        {
            Walk,
            Vertical,
            Dash
        }
    }
}

