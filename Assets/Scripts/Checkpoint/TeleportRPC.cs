using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TeleportRPC : MonoBehaviour
{
    [PunRPC]
    public void Teleport(Vector3 Target)
    {
        this.gameObject.transform.position = Target;
    }
}
