using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


///<summary>
///Author: Daniel
///Checks if two players are in the area and
///activates its respective virtual camera if so
///</summary>

public class CameraActivate : MonoBehaviour
{
    public CinemachineVirtualCamera Camera;
    public HashSet<GameObject> PlayersInArea;

    private void Awake()
    {
        PlayersInArea = new HashSet<GameObject>();
    }

    private void Update()
    {
        if (PlayersInArea.Count == 2)
        {
            Camera.gameObject.SetActive(true);
        } else
        {
            Camera.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayersInArea.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayersInArea.Remove(other.gameObject);
        }
    }


}
