using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Ziqi
/// </summary>
public class IconAdjuster : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera;

    private void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if(MainCamera == null) MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // adjust the rotation to make icon always facing the camera
        this.transform.forward = new Vector3(MainCamera.transform.forward.x, MainCamera.transform.forward.y, this.transform.forward.z);
    }
}
