using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackNode : Node
{
    private float CooldownTime;
    private MonoBehaviour MonoBehaviour;
    private bool AttackRunning = false;
    private bool AttackComplete = false;
    private AnimationManager AnimManager;
    private float AnimDelay;

    public MeleeAttackNode(float CooldownTime, MonoBehaviour MonoBehaviour, AnimationManager AnimationManager, float delay)
    {
        this.CooldownTime = CooldownTime;
        this.MonoBehaviour = MonoBehaviour;
        this.AnimManager = AnimationManager;
        this.AnimDelay = delay;
    }
    public override NodeState Evaluate()
    {
        if (!AttackRunning)
        {
            AttackRunning = true;
            MonoBehaviour.StartCoroutine(AttackAndCooldown());
            return NodeState.RUNNING;
        }
        else
        {
            if (AttackComplete)
            {
                AttackComplete = false;
                AttackRunning = false;
                return NodeState.SUCCESS;
            }
            return NodeState.RUNNING;
        }
    }

    private void MeleeAttack()
    {
        Debug.Log("Starting Attack");
        MonoBehaviour.gameObject.GetComponent<EnemyCombatManager>().Smash();
        Debug.Log("Ending Attack");
    }

    private void PerformAnimation()
    {
        if (AnimManager != null) AnimManager.TriggerAttack();
    }

    IEnumerator AttackAndCooldown()
    {
        PerformAnimation();
        yield return new WaitForSeconds(AnimDelay);
        MeleeAttack();
        yield return new WaitForSeconds(CooldownTime);
        AttackComplete = true;
    }
}