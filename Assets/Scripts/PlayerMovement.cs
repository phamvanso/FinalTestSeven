using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [HideInInspector]
    public bool canMove = true;

    private Rigidbody rb;
    private Animator animator;

    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        HandleInput();
        HandleAnimation();
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleInput()
    {
        if (!canMove)
        {
            moveDirection = Vector3.zero;
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(
            moveX,
            0f,
            moveZ
        ).normalized;
    }

    void Move()
    {
        if (!canMove) return;

        if (moveDirection != Vector3.zero)
        {
            Vector3 move = moveDirection *
                           moveSpeed *
                           Time.fixedDeltaTime;

            rb.MovePosition(rb.position + move);

            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection);

            rb.rotation = Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }
    }

    void HandleAnimation()
    {
        float speed = moveDirection.magnitude > 0 ? 1f : 0f;

        animator.SetFloat("Speed", speed);
    }
}