using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : Interactable
{
    public Outline _outline;
    GameObject playerCamera;
    //TODO:
    // add a requirement for component Outline or at least an error log when its not present (this may be better as it will cause the user to config the outline)
    // Set outline settings in code so they are consisten across all objects


    void Start()
    {
       _outline = GetComponent<Outline>();

        gameObject.layer = LayerMask.NameToLayer("Interactable");
  
    }

    private void Update()
    {
        if (playerCamera == null)
        {
            playerCamera = GameObject.Find("Main Camera");
        }
    }

    public override void OnFocus()
    {
        print("Looking at " + gameObject.name);
        _outline.enabled = true;
    }

    public override void OnInteract()
    {
        gameObject.transform.parent = playerCamera.transform;
        gameObject.transform.localRotation = new Quaternion(0, -90, 0, 0);
        gameObject.transform.localPosition = new Vector3(-0.5f, 2, 3);
    }

    public override void OnLoseFocus()
    {
       _outline.enabled = false;
       print("Stopped Looking at " + gameObject.name);
    }
}
