using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    public int MaxFrameRate = 180;
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxFrameRate;
    }
}
