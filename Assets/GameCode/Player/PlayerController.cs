using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float MaxSpeed = 10f;
    public float JumpForce = 400f;

    private Rigidbody2D body;
    bool grounded = false;

    public Vector3 groundCheck;
    public float groundRadius = 0.3f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(move * MaxSpeed, body.velocity.y);
        if (Input.GetButtonDown("Jump"))
        {
            body.AddForce(new Vector2(0f, JumpForce));
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + groundCheck, groundRadius);
    }

    void Update()
    {

    }
}
