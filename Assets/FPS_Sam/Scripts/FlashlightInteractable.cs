using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightInteractable : Interactable
{
    public Outline _outline;
    public GameObject clickPrompt;
    GameObject playerCamera;
    GameObject playerObj;
    FirstPersonController_Sam player;
    //TODO:
    // add a requirement for component Outline or at least an error log when its not present (this may be better as it will cause the user to config the outline)
    // Set outline settings in code so they are consisten across all objects


    void Start()
    {
       _outline = GetComponent<Outline>();

        gameObject.layer = LayerMask.NameToLayer("Interactable");
        clickPrompt.SetActive(false);

    }

    private void Update()
    {
        if (playerCamera == null)
        {
            playerCamera = GameObject.Find("Main Camera");
        }

        if (playerObj == null)
        {
            playerObj = GameObject.Find("Player");
            player = playerObj.GetComponent<FirstPersonController_Sam>();
        }
    }

    public override void OnFocus()
    {
        //print("Looking at " + gameObject.name);
        clickPrompt.SetActive(true);
        _outline.enabled = true;
    }

    public override void OnInteract()
    {
        // Place Flashlight in players hand
        gameObject.transform.parent = playerCamera.transform;
        gameObject.transform.localRotation = Quaternion.Euler(0, -90, 0);
        gameObject.transform.localPosition = new Vector3(-0.33f, -0.6f, 0.63f);
        player.holdingFlashlight = true;
    }

    public override void OnLoseFocus()
    {
        clickPrompt.SetActive(false);
        _outline.enabled = false;
       //print("Stopped Looking at " + gameObject.name);
    }
}
