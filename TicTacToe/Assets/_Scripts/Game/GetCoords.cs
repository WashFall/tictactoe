using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCoords : MonoBehaviour
{
    private Vector3 coord;
    private Rect coords;
    
    void Start()
    {
        var rend = gameObject.GetComponent<Renderer>();
        Debug.Log(rend.bounds.max);
        Debug.Log(rend.bounds.min);
    }

    void Update()
    {
        
    }
}
