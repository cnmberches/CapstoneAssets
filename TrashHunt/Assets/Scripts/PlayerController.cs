using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerController : MonoBehaviour
{
    public LayerMask solidObjectsLayer;
    public JoystickController movementJoystick;
    public float playerSpeed;
    private Rigidbody2D rb;
    public float moveSpeed;
    private bool isMoving;
    private Animator animator;
    private bool facingRight = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(!isMoving)
        {
            Vector2 input;
            input.x = movementJoystick.joystickVec.x;
            input.y = movementJoystick.joystickVec.y;

            Debug.Log("Joystick X: " + movementJoystick.joystickVec.x);
            Debug.Log("Joystick Y: " + movementJoystick.joystickVec.y);

            

            if (input != Vector2.zero)
            {
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (isWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                    if (movementJoystick.joystickVec.x > 0 && !facingRight)
                    {
                        Flip();
                        Debug.Log("Flipped :" + facingRight);
                    }
                    else if (movementJoystick.joystickVec.x < 0 && facingRight)
                    {
                        Flip();
                        Debug.Log("Flipped :" + facingRight);
                    }
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }

        /*
         * Debugger
         * Debug.Log("Animator Right: " + animator.GetBool("isMovingRight"));
         * Debug.Log("Animator Left: " + animator.GetBool("isMovingLeft"));
         * Debug.Log("Animator Move: " + animator.GetBool("isMoving"));
        */
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
        //start movement
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            rb.velocity = new Vector2(movementJoystick.joystickVec.x * playerSpeed, movementJoystick.joystickVec.y * playerSpeed);

            if (movementJoystick.joystickVec.x > 0)
            {
                //start animation of walking to right
                animator.SetBool("isMovingLeft", false);
                animator.SetBool("isMovingRight", true);
            }
            else if (movementJoystick.joystickVec.x < 0)
            {
                //start animation of walking to left
                animator.SetBool("isMovingLeft", true);
                animator.SetBool("isMovingRight", false);
            }
            else if (movementJoystick.joystickVec.x == 0 && movementJoystick.joystickVec.y == 0)
            {
                //stop animation of walking to left
                animator.SetBool("isMoving", false);
                animator.SetBool("isMovingRight", false);
                animator.SetBool("isMovingLeft", false);
                isMoving = false;
            }

            yield return null;
        }
    }
    void Flip()
    {
        Vector3 currentScale = rb.transform.localScale;
        currentScale.x *= -1;
        rb.transform.localScale = currentScale;
        facingRight = !facingRight;
    }
}
