using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    [Header("Flashlight Components")]
    public GameObject flashlightOn;
    public GameObject flashlightOff;
    public Slider batterySlider;

    [Header("PowerStats")]
    public float batteryPower = 100;
    public float drainSpeed = 5;

    bool batteryIsEmpty = false;
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

        DisplayUI();

        if (player.holdingFlashlight == true)
        {
            player.HandleFlashlightInput(true);

            CheckBattery();
            if (batteryIsEmpty == false)
            {
                if (player.flashlightSwitchOn)
                {
                    LightOn();
                }

                else if (player.flashlightSwitchOn == false)
                {
                    LightOff();
                }
            }
            else LightOff();
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
        if (batteryPower > 100) batteryPower = 100;

        if (batteryPower < 0)
        {
            batteryPower = 0;
            batteryIsEmpty = true;
        }
        else if (batteryPower > 0) batteryIsEmpty = false;
    }

    void DisplayUI()
    {
        batterySlider.value = batteryPower;
    }

    void LightOn()
    {
        flashlightOn.SetActive(true);
        flashlightOff.SetActive(false);
        BatteryDrain(true);
    }

    void LightOff()
    {
        flashlightOn.SetActive(false);
        flashlightOff.SetActive(true);
        BatteryDrain(false);
    }
}
