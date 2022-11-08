using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : Interactable
{
    public Outline _outline;
    //TODO:
    // add a requirement for component Outline or at least an error log when its not present (this may be better as it will cause the user to config the outline)
    // Set outline settings in code so they are consisten across all objects


    void Start()
    {
       _outline = GetComponent<Outline>();

        gameObject.layer = LayerMask.NameToLayer("Interactable");

        
    }

    public override void OnFocus()
    {
        //print("Looking at " + gameObject.name);
        _outline.enabled = true;
    }

    public override void OnInteract()
    {
        print("Interacted with " + gameObject.name);
    }

    public override void OnLoseFocus()
    {
        _outline.enabled = false;
       // print("Stopped Looking at " + gameObject.name);
    }
}
