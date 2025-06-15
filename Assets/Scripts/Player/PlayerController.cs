using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float moveSpeed;

    private PlayerControls playerControls;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator anim;

    public bool canMove = true;
    public string areaTransitionName;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        PlayerInput();
        Move();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    /*--------------------------------------------------------------------Movement Methods-------------------------------------------------------------------------*/

    private void PlayerInput()
    {
        if(canMove)
        {
            moveInput = playerControls.Movement.Move.ReadValue<Vector2>();

            anim.SetFloat("MoveX", moveInput.x);
            anim.SetFloat("MoveY", moveInput.y);
        }
    }

    private void Move()
    {
        if (canMove)
        {
            rb.MovePosition(rb.position + moveInput * (moveSpeed * Time.fixedDeltaTime));
        }
    }
}
