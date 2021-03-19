using System.Collections;
using UnityEngine;


public class Bullet : MonoBehaviour {

    public float BulletAttackTimeSpan = 0.3f;
    public float Force = 500;

    private void Awake() {
        StartCoroutine(DestroyDelay());
    }

    /// <summary>
    /// Author: Feiyang Li
    /// integrating with combat system
    /// </summary>
    private void OnTriggerStay(Collider Other) {
        PlayerCombatManager _playerCombatManager = Other.gameObject.GetComponent<PlayerCombatManager>();
        if (_playerCombatManager != null) {
            Vector3 _AttackVector = (Other.transform.position - this.transform.position).normalized * Force;
            _playerCombatManager.ReceivePlayerAttack(_AttackVector, BulletAttackTimeSpan);
        }

        Destroy(this.gameObject);
    }

    IEnumerator DestroyDelay() {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
