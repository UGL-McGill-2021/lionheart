using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TempPlatform : MonoBehaviour
{
    public float DisappearDelay = 3f;
    public bool isReusable = false;
    public float RespawnDelay = 10f;

    public delegate void OnPlatformStateChangedDelegate(bool PlatformAppears);
    public OnPlatformStateChangedDelegate OnPlatformStateChanged;

    private Coroutine CurrentCoroutine;
    private PhotonView PhotonView;

    // Start is called before the first frame update
    void Awake()
    {
        CurrentCoroutine = null;
        PhotonView = this.GetComponent<PhotonView>();
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Coroutine for starting the disappearing of this platform
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartDisappearing(bool isReusable)
    {
        yield return new WaitForSeconds(DisappearDelay);
        PhotonView.RPC("PRC_DisableThisObject", RpcTarget.All);

        // if we want the platform to be respawnable
        if (isReusable)
        {
            yield return new WaitForSeconds(RespawnDelay);
            PhotonView.RPC("PRC_EnableThisObject", RpcTarget.All);
            CurrentCoroutine = null;
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// RPC function to disable the colliders and mesh of this object
    /// </summary>
    [PunRPC]
    void PRC_DisableThisObject()
    {
        foreach (Collider collider in this.gameObject.GetComponents<Collider>())
            collider.enabled = false;
        foreach (MeshRenderer mesh in this.gameObject.GetComponents<MeshRenderer>())
            mesh.enabled = false;

        OnPlatformStateChanged(false);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// RPC function to enable the colliders and mesh of this object
    /// </summary>
    [PunRPC]
    void PRC_EnableThisObject()
    {
        foreach (Collider collider in this.gameObject.GetComponents<Collider>())
            collider.enabled = true;
        foreach (MeshRenderer mesh in this.gameObject.GetComponents<MeshRenderer>())
            mesh.enabled = true;

        OnPlatformStateChanged(true);
    }

    /// <summary>
    /// Call back function of collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") {
            Debug.Log("Enemy collided with platform");
        }

        // only the master client will start the coroutine using an RPC call
        if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy") && CurrentCoroutine == null && PhotonView.IsMine)
        {
            if (!other.isTrigger) CurrentCoroutine = StartCoroutine(StartDisappearing(isReusable));
        }
    }
}
