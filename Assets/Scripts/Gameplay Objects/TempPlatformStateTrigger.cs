using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Author: Feiyang Li
/// </summary>
public class TempPlatformStateTrigger : MonoBehaviour {
    public TempPlatform platform;

    private List<EnemyCombatManager> OverlappingNPCs = new List<EnemyCombatManager>();

    private void Start() {
        platform.OnPlatformStateChanged += UpdateNPCControl;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy") {
            EnemyCombatManager _enemy = other.gameObject.GetComponent<EnemyCombatManager>();
            if (_enemy != null) {
                OverlappingNPCs.Add(_enemy);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Enemy") {
            EnemyCombatManager _enemy = other.gameObject.GetComponent<EnemyCombatManager>();
            if (_enemy != null) {
                OverlappingNPCs.Remove(_enemy);
            }
        }
    }

    private void UpdateNPCControl(bool PlatformAppears) {
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
