using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public LayerMask solidObjectsLayer;
    public JoystickController movementJoystick;
    public float playerSpeed;
    private Rigidbody2D rb;
    public float moveSpeed;
    private bool isMoving;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movementJoystick.joystickVec.y != 0)
        {
            animator.SetFloat("moveX", movementJoystick.joystickVec.x);
            animator.SetFloat("moveY", movementJoystick.joystickVec.y);

            var targetPos = transform.position;
            targetPos.x += movementJoystick.joystickVec.x;
            targetPos.y += movementJoystick.joystickVec.y;

            if (isWalkable(targetPos))
            {
                StartCoroutine(Move(targetPos));
                rb.velocity = new Vector2(movementJoystick.joystickVec.x * playerSpeed, movementJoystick.joystickVec.y * playerSpeed);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        animator.SetBool("isMoving", isMoving);
    }
    private bool isWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.3f, solidObjectsLayer) != null)
        {
            return false;
        }
        return true;
    }
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        rb.velocity = new Vector2(movementJoystick.joystickVec.x * playerSpeed, movementJoystick.joystickVec.y * playerSpeed);
        isMoving = false;
    }
}
