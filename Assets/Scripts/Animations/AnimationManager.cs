using UnityEngine;
using System.Collections;

/// <summary>
/// Author: Ziqi Li
/// Abstract parent class for animation managers
/// </summary>
public abstract class AnimationManager : MonoBehaviour
{
    protected Animator ThisAnimator;

    // Use this for initialization
    protected virtual void Awake()
    {
        ThisAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// Author:Ziqi
    /// Since the Trigger doesn't work well with PhotonAnimatorView, we have to use boolean
    /// to trigger transition (set the bool to true and change it back in the next frame (for stablility, wait 0.1 second))
    /// </summary>
    /// <returns></returns>
    protected IEnumerator TriggerAnimTransition(string paramName)
    {
        ThisAnimator.SetBool(paramName, true);
        //yield return new WaitForFixedUpdate();
        //yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.1f);
        ThisAnimator.SetBool(paramName, false);
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
