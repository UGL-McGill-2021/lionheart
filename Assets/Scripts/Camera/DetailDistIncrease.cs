using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailDistIncrease : MonoBehaviour
{

	public List<Terrain> TerrainList;  //reference to your terrain
	public float DrawDistance; // how far you want to be able to see the grass

	void Start()
	{
		foreach (Terrain T in TerrainList) {
			T.detailObjectDistance = DrawDistance;
		}

	}
}