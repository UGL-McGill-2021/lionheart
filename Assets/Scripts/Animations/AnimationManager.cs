using UnityEngine;
using System.Collections;

/// <summary>
/// Author: Ziqi Li
/// </summary>
public abstract class AnimationManager : MonoBehaviour
{
    protected Animator ThisAnimator;

    // Use this for initialization
    protected virtual void Start()
    {
        ThisAnimator = GetComponent<Animator>();
    }


    /// <summary>
    /// Author: Ziqi
    /// Function to trigger attack
    /// </summary>
    public abstract void TriggerAttack();

    /// <summary>
    /// Author: Ziqi
    /// Function to trigger moving
    /// </summary>
    public abstract void IsMoving(float speed);


}
