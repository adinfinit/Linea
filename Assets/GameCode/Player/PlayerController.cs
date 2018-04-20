using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float MaxSpeed = 10f;
    // The fastest the player can travel in the x axis.
    [SerializeField] private float JumpForce = 400f;

    private Rigidbody2D body;

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

    void OnDrawGizmos()
    {
    }

    void Update()
    {

    }
}
