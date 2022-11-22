using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractable : Interactable
{
    bool animationOn = false;
    float time = 1f;

    public Outline _outline;
    public Animator animator;
    public PlayerStats playerStats;

    public GameObject damageGraphicObj;
    public Image damageGraphic;
    GameObject playerObj;
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
        if (damageGraphicObj == null)
        {
            damageGraphicObj = GameObject.Find("DamageGraphic");
            damageGraphic = damageGraphicObj.GetComponent<Image>();
        }

        if (playerObj == null)
        {
            playerObj = GameObject.Find("Player");
            playerStats = playerObj.GetComponent<PlayerStats>();
        }

        if (animationOn)
        {
            time -= 0.1f;
            if (time <= 0)
            {
                animator.SetBool("ButtonPressed", false);
                animationOn = false;
                time = 1f;
            }
        }
    }
    private void FixedUpdate()
    {
        
    }

    public override void OnFocus()
    {
        _outline.enabled = true;
    }

    public override void OnInteract()
    {
        animator.SetBool("ButtonPressed", true);
        DamagePlayer();
        animationOn = true;
    }

    public override void OnLoseFocus()
    {
        _outline.enabled = false;
    }

    public void DamagePlayer()
    {
        playerStats.health -= 10;
        damageGraphic.enabled = true;
    }
}
