using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScale : MonoBehaviour
{
    private Vector3 landscapeSize;
    private Vector3 portraitSize;
    
    private void Start()
    {
        landscapeSize = transform.localScale;
        portraitSize = landscapeSize / 2;
    }

    void Update()
    {
        if (Input.deviceOrientation == DeviceOrientation.Portrait)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, portraitSize, Time.deltaTime * 10f);
        }
        else if(Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, landscapeSize, Time.deltaTime * 10f);
        }
    }
}
