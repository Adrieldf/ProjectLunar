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


    void Start()
    {

        rb.velocity = new Vector2(startVelocity, startVelocity);
    }

    void Update()
    {
        if(rb.velocity.x > 20)
        {
            rb.velocity = new Vector2(20, rb.velocity.y);
        }
        if (rb.velocity.y > 20)
        {
            rb.velocity = new Vector2(rb.velocity.x, 20);
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
