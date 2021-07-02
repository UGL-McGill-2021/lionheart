using System.Collections;
using System.Collections.Generic;
using Lionheart.Player.Movement;
using UnityEngine;

public class TransitionSkiper : MonoBehaviour
{
    public Animator Animator;  // the crossfade transition animator
    private bool hasSkipped = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        resetAnimationSpeed();
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Call back function from input system to skip the animation
    /// </summary>
    public void OnSkip()
    {
        // if we are at the enter state
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("CrossFadeEnter") && !hasSkipped)
        {
            hasSkipped = true;
            Animator.speed = 10.0f;
            EnablePlayerControl();
        }
        
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to reset the animation speed to 1
    /// </summary>
    public void resetAnimationSpeed()
    {
        if (!Animator.GetCurrentAnimatorStateInfo(0).IsName("CrossFadeEnter") && Animator.speed > 1)
        {
            Animator.speed = 1.0f;
        }
    }

    void EnablePlayerControl()
    {
        foreach (GameObject player in GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList)
        {
            player.GetComponent<MultiplayerActivator>().ActivatePlayer();
        }
    }
}
