using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TeleportRPC : MonoBehaviour
{
    [PunRPC]
    public void Teleport(Transform Target)
    {
        this.gameObject.transform.position = Target.position;
    }
}
