using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Ziqi
/// </summary>
public class IconAdjuster : MonoBehaviour
{

    private void Start()
    {
    }

    void Update()
    {
        // rotate the icon
        this.transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime, Space.World);
    }
}
