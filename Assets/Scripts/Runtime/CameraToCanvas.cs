using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var canvas = GetComponent<Canvas>();

        if (canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
    }
}
