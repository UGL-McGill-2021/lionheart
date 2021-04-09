using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Lionheart.Player.Movement;

/// <summary>
/// Author: Denis
/// Script that handles the Player VFX
/// </summary>
public class VFXHandler : MonoBehaviour
{
    [Header("References")]
    public PhotonView NetworkView;
    [SerializeField] PullDash PlayerPullDash;
    [SerializeField] GameObject OtherPlayer;
    [SerializeField] GameObject PullDashBeam;
    [SerializeField] GameObject PullDashBeamBegin;
    [SerializeField] GameObject PullDashBeamEnd;
    [SerializeField] GameObject PullDashBeamTarget;
    [SerializeField] LineRenderer PullDashLR;
    [SerializeField] GameObject GroundPoundPS;
    [SerializeField] ParticleSystem DashPS;
    [SerializeField] ParticleSystem HitPS;

    [Header("Parameters")]
    public float StepScalar = 1.4f; 

    private void Start()
    {
        PullDashLR = PullDashBeam.GetComponent<LineRenderer>();
    }

    /// <summary>
    /// Author: Denis
    /// Pull dash beam position and width updates
    /// </summary>
    private void Update()
    {
        UpdateRefs();

        if (PullDashBeam.activeSelf == true)
        {
            PullDashLR.SetPosition(0, PullDashBeamBegin.transform.position);
            PullDashLR.SetPosition(1, PullDashBeamTarget.transform.position);
            Vector3 Vd = (OtherPlayer.transform.position - transform.position);
            float _Width = 0.25f + (Vd.magnitude * StepScalar) / 50f;
            PullDashLR.startWidth = _Width;
            PullDashLR.endWidth = _Width;
        }
    }

    /// <summary>
    /// Author: Denis
    /// Gets the required refs if they are null
    /// </summary>
    private void UpdateRefs()
    {
        if (PullDashBeamTarget==null || PullDashBeamEnd == null)
        {
            GameObject[] _Players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < _Players.Length; i++)
            {
                if (_Players[i].Equals(gameObject) == false)
                {
                    OtherPlayer = _Players[i];
                    PullDashBeamTarget = _Players[i].GetComponent<VFXHandler>().PullDashBeamBegin;
                    PullDashBeamEnd = PullDashBeamTarget;
                }
            }
        }
    }

    /// <summary>
    /// Author: Denis
    /// To update the vfx over the network
    /// </summary>
    /// <param name="B"></param>
    [PunRPC]
    public void UpdateBeamBegin(bool B)
    {
        PullDashBeamBegin.SetActive(B);
    }

    [PunRPC]
    public void UpdateBeamAndBegin(bool B)
    {
        PullDashBeamBegin.SetActive(B);
        PullDashBeam.SetActive(B);
    }

    [PunRPC]
    public void UpdateBeamAndEnd(bool B)
    {
        PullDashBeamEnd.SetActive(B);
        PullDashBeam.SetActive(B);
    }

    [PunRPC]
    public void UpdateBeamAll(bool B)
    {
        PullDashBeamBegin.SetActive(B);
        PullDashBeamEnd.SetActive(B);
        PullDashBeam.SetActive(B);
    }

    [PunRPC]
    public void PlayGroundPound(Vector3 Pos)
    {
        GameObject G = GameObject.Instantiate(GroundPoundPS, Pos, Quaternion.identity);
    }

    [PunRPC]
    public void PlayDash()
    {
        DashPS.Emit(100);
    }

    [PunRPC]
    public void PlayHit()
    {
        DashPS.Emit(100);
    }
}
