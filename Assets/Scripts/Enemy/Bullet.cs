using System.Collections;
using UnityEngine;


public class Bullet : MonoBehaviour {

    public float BulletAttackTimeSpan = 0.3f;
    public float Force = 500;
    public GameObject owner;

    private void Awake() {
        StartCoroutine(DestroyDelay());
    }

    /// <summary>
    /// Author: Feiyang Li
    /// integrating with combat system
    /// </summary>
    private void OnTriggerStay(Collider Other) {
        if (Other.gameObject.tag == "Player") {
            PlayerCombatManager _playerCombatManager = Other.gameObject.GetComponent<PlayerCombatManager>();
            if (_playerCombatManager != null) {
                Vector3 _AttackVector = (Other.transform.position - this.transform.position).normalized * Force;
                _playerCombatManager.ReceivePlayerAttack(_AttackVector, BulletAttackTimeSpan);
            }

            Destroy(this.gameObject);

        } else if (Other.gameObject.tag == "Enemy" && Other.gameObject != owner) {
            EnemyCombatManager _enemyCombatManager = Other.gameObject.GetComponent<EnemyCombatManager>();
            if (_enemyCombatManager != null) {
                Vector3 _AttackVector = (Other.transform.position - this.transform.position).normalized * Force;
                _enemyCombatManager.ReceiveAttack(_AttackVector, BulletAttackTimeSpan);
            }

            Destroy(this.gameObject);
        }
    }

    IEnumerator DestroyDelay() {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
