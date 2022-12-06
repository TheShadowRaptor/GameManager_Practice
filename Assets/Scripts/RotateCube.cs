using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotateAmount = new Vector3(180, 360, 0);
        transform.Rotate(rotateAmount * Time.deltaTime);
    }
}
