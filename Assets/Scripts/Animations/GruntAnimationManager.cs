using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Ziqi Li
/// </summary>
public class GruntAnimationManager : AnimationManager
{
    public string ParamName_Moving = "MovingSpeed";  // the parameter name in animator for moving
    public string ParamName_Attack = "isAttack";  // the parameter name in animator for attack

    /// <summary>
    /// Author: Ziqi
    /// Implementation of abstract method to trigger attack
    /// </summary>
    public override void TriggerAttack()
    {
        StartCoroutine(TriggerAnimTransition(ParamName_Attack));
    }

    /// <summary>
    /// Author: Ziqi
    /// Implementation of abstract method to trigger moving
    /// </summary>
    public override void IsMoving(float speed)
    {
        ThisAnimator.SetFloat(ParamName_Moving, Mathf.Abs(speed));
    }

}
