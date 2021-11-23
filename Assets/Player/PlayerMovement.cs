using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Setup

    bool canMove = true;

    public bool showDebug = true;

    Rigidbody rb;

    Vector3 movement;

    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    float speed;

    public CharacterController controller;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        speed = moveSpeed;
    }

    #endregion

    #region Movement

    public void TryMove(float xMove, float zMove)
    {
        if (canMove)
        {
            Move(xMove, zMove);
        }
    }

    void Move(float xMove, float zMove)
    {
        if (rb != null)
        {
            if (showDebug)
                Debug.Log("X:" + xMove + " |  Z:" + zMove);

            movement.x = xMove;
            movement.z = zMove;

            //rb.velocity += speed * movement * Time.deltaTime;

            controller.Move(speed * movement * Time.deltaTime);

            controller.Move(new Vector3(0, -9.81f, 0));
        }
    }

    public void SetSprint(bool sprint)
    {
        if (sprint)
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = moveSpeed;
        }
    }

    #endregion
}