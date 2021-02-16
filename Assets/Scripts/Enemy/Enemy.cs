using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IPunObservable
{
    public abstract void Attacked();

    public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);

}
