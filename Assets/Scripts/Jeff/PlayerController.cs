using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour

{
    [SerializeField] Transform groundCheck = null;
    [SerializeField] LayerMask playerMask;
    [SerializeField] float movementSpeed = 5;
    [SerializeField] int jumpHeight = 5;
    [SerializeField] float boostingTime = 3;
    [SerializeField] float timeShieldIsActive = 5;
    [SerializeField] float boostingSpeed = 10;
    [SerializeField] GameObject shield;
    [SerializeField] float dashTime = 0.5f;
    [SerializeField] float dashSpeed = 20;

    bool jumpPressed;
    float horizontalInput;
    float verticalInput;
    float speedBoost;
    float dash;
    float dashingTimer = 0;
    float boostTimer = 0;
    bool isBoosting = false;
    bool shieldEnabled = false;
    float shieldTimer = 0;
    bool isDashing = false;
    Rigidbody rb;
    GameObject destroyShield;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dash = dashSpeed;
            isDashing = true;

        }

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontalInput * (movementSpeed + speedBoost + dash), rb.velocity.y, verticalInput * (movementSpeed + speedBoost + dash));

        if (isBoosting)
        {
            boostTimer += Time.deltaTime;
            if (boostTimer >= boostingTime)
            {
                speedBoost = 0;
                boostTimer = 0;
                isBoosting = false;
            }
        }

        if (isDashing)
        {
            dashingTimer += Time.deltaTime;
            if (dashingTimer >= dashTime)
            {
                dash = 0;
                dashingTimer = 0;
                isDashing = false;
            }
        }

        if (shieldEnabled)
        {
            shieldTimer += Time.deltaTime;
            if (shieldTimer >= timeShieldIsActive)
            {
                shieldTimer = 0;
                destroyShield = GameObject.FindGameObjectWithTag("Shield");
                Destroy(destroyShield);
                shieldEnabled = false;
            }
        }

        if (Physics.OverlapSphere(groundCheck.position, 0.1f, playerMask).Length != 0)
        {
            if (jumpPressed)
            {
                rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
                jumpPressed = false;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SpeedBooster")
        {
            isBoosting = true;
            speedBoost = boostingSpeed;
            DestroyObject(other.gameObject);
        }

        if (other.tag == "ShieldItem")
        {
            shieldEnabled = true;
            GameObject wearingShield = Instantiate(shield, transform.position, transform.rotation);
            wearingShield.transform.parent = GameObject.Find("Player").transform;
            DestroyObject(other.gameObject);
        }
    }


}