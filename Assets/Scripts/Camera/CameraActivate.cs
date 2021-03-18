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
    private int PlayersInArea;

    
    private void Update()
    {
        if (PlayersInArea < 0) { PlayersInArea = 0; }

        if (PlayersInArea > 2) { PlayersInArea = 2; }

        if (PlayersInArea == 2)
        {
            Camera.gameObject.SetActive(true);
        } else if (PlayersInArea == 0)
        {
          Camera.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayersInArea++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayersInArea--;
        }
    }


}
