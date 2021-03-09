using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Author: Daniel Holker
/// Sets up camera to keep two targets in frame
/// </summary>
public class FollowGroupCamera : MonoBehaviour
{
    public bool IsOnline;   //Set true for online play, set false if you wish to set the targets manually
    public float Radius;      //Radius to add to targets in target group
    public CinemachineTargetGroup TargetGroup;
    private List<GameObject> PlayerList;
	private bool ready = false;
	
	
	
	void Update() {
		if (!ready) {
			PlayerList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PlayerList;
			
			if (PlayerList.Count > 0) {
				ready = true;
				foreach (GameObject G in PlayerList)
				{
					TargetGroup.AddMember(G.transform, 1, Radius);
				}
			}
		}
	}

}
