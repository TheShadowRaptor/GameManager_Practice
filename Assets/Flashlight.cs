using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("Flashlight Components")]
    public GameObject flashlightOn;
    public GameObject flashlightOff;

    [Header("PowerStats")]
    public float batteryPower = 100;
    public float drainSpeed = 5;

    bool BatteryIsEmpty = false;
    GameObject playerObj;
    FirstPersonController_Sam player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObj == null)
        {
            playerObj = GameObject.Find("Player");
            player = playerObj.GetComponent<FirstPersonController_Sam>();
        }

        if (player.holdingFlashlight == true)
        {
            player.HandleFlashlightInput(true);

            CheckBattery();
            if (BatteryIsEmpty == false)
            {
                if (player.flashlightSwitchOn)
                {
                    flashlightOn.SetActive(true);
                    flashlightOff.SetActive(false);
                    BatteryDrain(true);
                }

                else if (player.flashlightSwitchOn == false)
                {
                    flashlightOn.SetActive(false);
                    flashlightOff.SetActive(true);
                    BatteryDrain(false);
                }
            }
        }
    }

    void BatteryDrain(bool draining)
    {
        if (draining)
        {
            batteryPower -= drainSpeed * Time.deltaTime;
        }
    }

    void CheckBattery()
    {
        if (batteryPower > 100)
        {
            batteryPower = 100;
        }

        else if (batteryPower < 0)
        {
            batteryPower = 0;
            BatteryIsEmpty = true;
        }
    }
}
