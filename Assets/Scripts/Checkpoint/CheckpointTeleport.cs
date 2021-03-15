using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CheckpointTeleport : MonoBehaviour
{
    [PunRPC]
    public void Teleport(Vector3 Target)
    {
        this.gameObject.transform.position = Target;
    }
}
