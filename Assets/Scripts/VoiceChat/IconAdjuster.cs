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

    // Update is called once per frame
    void Update()
    {
        // adjust the rotation to make icon always facing the camera
        this.transform.LookAt(MainCamera.transform);
    }
}
