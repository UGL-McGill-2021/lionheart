using UnityEngine;
using System.Collections;
using Photon.Pun;
using Lionheart.Player.Movement;

/// <summary>
/// Author: Ziqi Li
/// A script for spirit wall
/// </summary>
public class SpiritWall : MonoBehaviour
{
    public Collider ThisCollider;
    public bool isOneWay = false;


    private ArrayList PastPlayerList = new ArrayList();  // keep a list for player passed this wall (used for "one way" feature)
    private PhotonView PhotonView;

    // Start is called before the first frame update
    void Awake()
    {
        PhotonView = this.GetComponent<PhotonView>();
    }


    /// <summary>
    /// Author: Ziqi Li
    /// setter exposed to game manager
    /// </summary>
    /// <param name="isOneWay"></param>
    public void SetIsOneWay(bool isOneWay)
    {
        if(PhotonView.IsMine) PhotonView.RPC("RPC_SetIsOneWay", RpcTarget.All, isOneWay);
    }

    /// <summary>
    /// Author: Ziqi Li
    /// RPC function for isOneWay setter
    /// </summary>
    /// <param name="isOneWay"></param>
    [PunRPC]
    void RPC_SetIsOneWay(bool isOneWay)
    {
        this.isOneWay = isOneWay;
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to disable the collision between two colliders if necessary
    /// </summary>
    //[PunRPC]
    void DisableCollision(Collider c1, Collider c2)
    {
        if (isOneWay)
        {
            // if the player has not yet passed this wall
            if (!PastPlayerList.Contains(c1.gameObject))
            {
                PastPlayerList.Add(c1.gameObject);
                Physics.IgnoreCollision(c1, c2, true);
            }
        }
        else
        {
            Physics.IgnoreCollision(c1, c2, true);
        }
    }

    /// <summary>
    /// Author: Ziqi Li
    /// Function to enable the collision between two colliders if necessary
    /// </summary>
    //[PunRPC]
    void EnableCollision(Collider c1, Collider c2)
    {
        if (isOneWay)
        {
            // if the player has passed this wall 
            if (PastPlayerList.Contains(c1.gameObject))
            {
                Physics.IgnoreCollision(c1, c2, false);
            }
        }
        else
        {
            Physics.IgnoreCollision(c1, c2, false);
        }
    }


    /// <summary>
    /// Call back function of collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player"/*PhotonView.IsMine*/) 
        {
            if(other.gameObject.GetComponent<Dash>().IsDashing) DisableCollision(other, ThisCollider);
        }
    }

    /// <summary>
    /// Call back function of collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EnableCollision(other, ThisCollider);
            //object[] args = { other, ThisCollider };
            //PhotonView.RPC("RPC_EnableCollision", RpcTarget.All, args);
        }
    }

}
