using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lionheart.Player.Movement
{
    public interface MovementModifier
    {
        public Vector3 Value { get; }
    }
}

