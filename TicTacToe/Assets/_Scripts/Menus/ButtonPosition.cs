using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPosition : MonoBehaviour
{
    public Vector3 portraitPos, landscapePos;

    void Update()
    {
        if (Input.deviceOrientation == DeviceOrientation.Portrait)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, portraitPos, Time.deltaTime * 10f);
        }
        else if(Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, landscapePos, Time.deltaTime * 10f);
        }
    }
}
