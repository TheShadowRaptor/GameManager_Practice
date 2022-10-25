using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("PlayerValues")]
    public int maxHealth = 100;
    public int minHealth = 0;
    public int currentHealth;
    public int speed = 10;

    [Header("Scripts")]
    public HealthBar healthBar;
    public HealthBar worldHealthBar;

    //Input Buttons
    KeyCode takeDamageButton = KeyCode.Space;
    KeyCode healButton = KeyCode.H;
    float horizontal;
    float vertical;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        worldHealthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        InputManager();
        
        //Health clamp
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        else if (currentHealth < minHealth)
        {
            currentHealth = minHealth;
        }
        //-----------------------------
    }

    private void FixedUpdate()
    {
        Move();
    }

    void InputManager()
    {     
        if (Input.GetKeyDown(takeDamageButton))
        {
            TakeDamage(20);
        }

        else if (Input.GetKeyDown(healButton))
        {
            Heal(20);
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        rb.velocity = new Vector3(horizontal, 0, vertical) * speed * Time.fixedDeltaTime;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        worldHealthBar.SetHealth(currentHealth);
    }

    void Heal(int heal)
    {
        currentHealth += heal;
        healthBar.SetHealth(currentHealth);
        worldHealthBar.SetHealth(currentHealth);
    }
}
