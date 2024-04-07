using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.0f;
    [SerializeField]
    private float jumpForce = 15.0f;
    [SerializeField]
    private int lives = 5;

    new private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;

    private bool isGrounded = false;

    private charState State
    {
        get { return (charState)animator.GetInteger("state"); }
        set { animator.SetInteger("state", (int)value);}
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (isGrounded)
            State = charState.idle;
        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButton("Jump"))
            Jump();
    }

    private void Run()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0f;

        if (isGrounded)
            State = charState.walk;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

        State = charState.jump;
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3F);

        isGrounded = colliders.Length > 1;

        if (!isGrounded)
            State = charState.jump;
    }


}

public enum charState
{
    idle,
    walk,
    jump
}
