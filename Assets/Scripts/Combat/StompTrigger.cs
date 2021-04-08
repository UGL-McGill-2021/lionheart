using UnityEngine;

public class StompTrigger : MonoBehaviour {

    [Header("Stomp Settings")]
    public float StompForce = 5;
    public float StompTime = 0.6f;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Stomped trigger enter " + other.gameObject + " with " + this.transform.forward.normalized * -1 * StompForce);
        if (other.gameObject.tag == "EnemyHat") {
            EnemyCombatManager _enemyCombatManager = other.transform.parent.GetComponent<EnemyCombatManager>();
            if (_enemyCombatManager != null) {
                _enemyCombatManager.ReceiveStomp(this.transform.forward.normalized * -1 * StompForce, 0.5f);
                //Debug.Log("Stomped " + other.gameObject + " with " + this.transform.forward.normalized * -1 * StompForce);
            }
        }
    }
}
