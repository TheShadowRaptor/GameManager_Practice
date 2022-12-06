using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    GameObject PlayerObj;
    CharacterController characterController;

    private void Start()
    {
        
    }
    private void Update()
    {
        if (PlayerObj == null)
        {
            PlayerObj = GameObject.Find("Player");
            characterController = PlayerObj.GetComponent<CharacterController>();
        }
    }
    public void RespawnPlayer()
    {
        characterController.enabled = false;
        PlayerObj.transform.position = gameObject.transform.position;
        characterController.enabled = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.9f);
    }
}
