using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform camera;

    public GameObject cameraObj;

    private void Update()
    {
        if (cameraObj == null)
        {
            cameraObj = GameObject.Find("Main Camera");
            camera = cameraObj.GetComponent<Transform>();
        }   
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + camera.forward);
    }
}
