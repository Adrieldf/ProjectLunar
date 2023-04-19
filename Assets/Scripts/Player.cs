using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private GameMainController controller;
    [SerializeField]
    private GrapplingGun grapplingGun;
    [SerializeField]
    private float startVelocity = 0.3f;

    private float playerMaxVelocity = 18f;

    void Start()
    {

        rb.velocity = new Vector2(startVelocity, startVelocity);
    }

    void Update()
    {
        if(rb.velocity.x > playerMaxVelocity)
        {
            rb.velocity = new Vector2(playerMaxVelocity, rb.velocity.y);
        }
        if (rb.velocity.y > playerMaxVelocity)
        {
            rb.velocity = new Vector2(rb.velocity.x, playerMaxVelocity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("OxygenTank") && (grapplingGun != null && !grapplingGun.IsPlayerDead))
        {
            Destroy(collision.gameObject);
            controller.RestockOxygen();
        }
    }
}
