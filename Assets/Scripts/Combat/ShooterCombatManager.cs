using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ShooterCombatManager : CombatManager {

    NavMeshAgent navMeshAgent;

    private void Start() {
        this.body = GetComponent<Rigidbody>();
        this.navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public override IEnumerator Knockback(int KnockBackTimeSpan) {
        // temporarily disable NavMeshAgent component
        navMeshAgent.enabled = false;
        body.isKinematic = false;

        // TODO: trigger animation here

        // wait the time span
        yield return new WaitForSeconds(KnockBackTimeSpan);

        // re-enable NavMesh and Rigidbody
        navMeshAgent.enabled = true;
        body.isKinematic = true;
    }
}
