using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Author: Denis
/// Script that handles the Player VFX
/// </summary>
public class VFXHandler : MonoBehaviour
{
    [Header("References")]
    public PhotonView NetworkView;
    [SerializeField] GameObject PullDashBeam;
    [SerializeField] GameObject PullDashBeamBegin;
    [SerializeField] GameObject PullDashBeamEnd;
    [SerializeField] GameObject PullDashBeamTarget;
    [SerializeField] LineRenderer PullDashLR;

    private void Start()
    {
        PullDashLR = PullDashBeam.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateRefs();

        if (PullDashBeam.activeSelf == true)
        {
            PullDashLR.SetPosition(0, PullDashBeamBegin.transform.position);
            PullDashLR.SetPosition(1, PullDashBeamTarget.transform.position);
        }
    }

    private void UpdateRefs()
    {
        if (PullDashBeamTarget==null || PullDashBeamEnd == null)
        {

            GameObject[] _Players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < _Players.Length; i++)
            {
                if (_Players[i].Equals(gameObject) == false)
                {
                    PullDashBeamTarget = _Players[i].GetComponent<VFXHandler>().PullDashBeamBegin;
                    PullDashBeamEnd = PullDashBeamTarget;
                }
            }
        }
    }

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
}
