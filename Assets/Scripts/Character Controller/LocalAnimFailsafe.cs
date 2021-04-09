using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalAnimFailsafe : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator AnimatorController;
    [SerializeField] public GameObject GroundCheck;
    [SerializeField] public Rigidbody Rb;

    [Header("Parameters")]
    [SerializeField] private float GroundDistance = 0.6f;
    [SerializeField] private LayerMask GroundMask;

    private bool IsGrounded;

    private void Update()
    {
        IsGrounded = Physics.CheckSphere(GroundCheck.transform.position, GroundDistance, GroundMask);

        if (IsGrounded == true && Rb.velocity.y <= 0.5f) 
        {
            AnimatorStateInfo St = AnimatorController.GetCurrentAnimatorStateInfo(0);
            if (St.IsName("Airborne"))
            {
                AnimatorController.SetBool("IsLanding", true);
                StartCoroutine(AnimationTrigger("IsLanding"));
            }
            else if (St.IsName("GPAirborne"))
            {
                AnimatorController.SetBool("IsSmashing", true);
                StartCoroutine(AnimationTrigger("IsSmashing"));
            }
            else if (St.IsName("KBAirborne"))
            {
                AnimatorController.SetBool("IsKBLanding", true);
                StartCoroutine(AnimationTrigger("IsKBLanding"));
            }
        }
    }

    /// <summary>
    /// Author: Denis
    /// Simulates animation trigger for bools
    /// </summary>
    /// <param name="Name"></param>
    /// <returns></returns>
    IEnumerator AnimationTrigger(string Name)
    {
        yield return new WaitForSecondsRealtime(1f);

        switch (Name)
        {
            case "IsJumping":
                AnimatorController.SetBool("IsJumping", false);
                break;
            case "IsFalling":
                AnimatorController.SetBool("IsFalling", false);
                break;
            case "IsLanding":
                AnimatorController.SetBool("IsLanding", false);
                break;
            case "IsSmashing":
                AnimatorController.SetBool("IsSmashing", false);
                break;
            case "IsKBLanding":
                AnimatorController.SetBool("IsKBLanding", false);
                break;
        }
    }
}
