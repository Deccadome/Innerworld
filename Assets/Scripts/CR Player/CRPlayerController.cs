using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRPlayerController : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody2D rb;
    private Vector2 lastMoveVector;
    private Vector2 moveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.WakeUp();
        lastMoveVector = new Vector2(1, 0);
    }

    void Update()
    {
        InputManagement();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void InputManagement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        moveDir = new Vector2(moveX, moveY);

        if(moveDir.x != 0) lastMoveVector = new Vector2(moveDir.x, 0f);
        if(moveDir.y != 0) lastMoveVector = new Vector2(0f, moveDir.y);
        if(moveDir.x != 0 && moveDir.y != 0) lastMoveVector = new Vector2(moveDir.x, moveDir.y);
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveSpeed * moveDir.x, moveSpeed * moveDir.y);
    }
}
