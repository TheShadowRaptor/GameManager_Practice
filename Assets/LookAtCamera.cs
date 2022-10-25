using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform camera;
    private void LateUpdate()
    {
        transform.LookAt(transform.position + camera.forward);
    }
}
