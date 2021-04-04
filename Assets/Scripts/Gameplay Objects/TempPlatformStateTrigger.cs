using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Author: Feiyang Li
/// </summary>
public class TempPlatformStateTrigger : MonoBehaviour {
    public TempPlatform platform;
    public bool PlatformAppears = true;

    public List<EnemyCombatManager> OverlappingNPCs = new List<EnemyCombatManager>();

    private void Start() {
        platform.OnPlatformStateChanged += UpdateNPCControl;
    }

    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.tag == "EnemyTempPlatformProbe") {
            EnemyCombatManager _enemy = other.gameObject.transform.parent.GetComponent<EnemyCombatManager>();
            if (_enemy != null) {
                Debug.Log("Probed enemy combat manager");
                if (!PlatformAppears) {
                    NavMeshAgent _agent = _enemy.gameObject.GetComponent<NavMeshAgent>();
                    Rigidbody _body = _enemy.gameObject.GetComponent<Rigidbody>();
                    if (_agent != null && _body != null) {
                        if (_agent.enabled && _body.isKinematic) {
                            _enemy.TriggerGivePhysControlOnAll(true);
                        }
                    }
                }

                OverlappingNPCs.Add(_enemy);
            }
        }
        
    }

    private void OnTriggerExit(Collider other) {
        
        if (other.gameObject.tag == "EnemyTempPlatformProbe") {
            EnemyCombatManager _enemy = other.gameObject.transform.parent.GetComponent<EnemyCombatManager>();
            if (_enemy != null) {
                OverlappingNPCs.Remove(_enemy);
            }
        }
        
    }

    private void UpdateNPCControl(bool PlatformAppears) {
        this.PlatformAppears = PlatformAppears;

        if (PlatformAppears) {
            foreach (EnemyCombatManager manager in OverlappingNPCs) {
                manager.TriggerGivePhysControlOnAll(false);
            }

        } else {
            foreach (EnemyCombatManager manager in OverlappingNPCs) {
                manager.TriggerGivePhysControlOnAll(true);
            }
        }

    }

    private void OnDestroy() {
        platform.OnPlatformStateChanged -= UpdateNPCControl;
    }
}
