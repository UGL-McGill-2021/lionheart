using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackNode : Node
{
    private float CooldownTime;
    private MonoBehaviour MonoBehaviour;
    private bool AttackRunning = false;
    private bool AttackComplete = false;

    public MeleeAttackNode (float CooldownTime, MonoBehaviour MonoBehaviour)
    {
        this.CooldownTime = CooldownTime;
        this.MonoBehaviour = MonoBehaviour;
    }
    public override NodeState Evaluate()
    {
        if (!AttackRunning)
        {
            AttackRunning = true;
            MonoBehaviour.StartCoroutine(AttackAndCooldown());
        }

        if (AttackComplete)
        {
            AttackComplete = false;
            AttackRunning = false;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }

    private void MeleeAttack()
    {
        Debug.Log("Melee attack executed!");
        MonoBehaviour.gameObject.GetComponent<EnemyCombatManager>().Smash();
    }

    IEnumerator AttackAndCooldown()
    {
        MeleeAttack();
        yield return new WaitForSeconds(CooldownTime);
        AttackRunning = false;
    }
}
