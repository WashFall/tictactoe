using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NamePositions : MonoBehaviour
{
    public Vector3 landscapePos;
    public Vector3 portraitPos;
    private TMP_Text text;
    public bool right;
    private TextAlignmentOptions align;

    private void Start()
    {
         text = GetComponent<TMP_Text>();
         if (right)
             align = TextAlignmentOptions.Right;
         else
             align = TextAlignmentOptions.Left;
    }

    void Update()
    {
        if (Input.deviceOrientation == DeviceOrientation.Portrait)
        {
            text.alignment = TextAlignmentOptions.Center;
            transform.localPosition = Vector3.Lerp(transform.localPosition, portraitPos, Time.deltaTime * 10f);
        }
        else if(Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            text.alignment = align;
            transform.localPosition = Vector3.Lerp(transform.localPosition, landscapePos, Time.deltaTime * 10f);
        }
    }
}
