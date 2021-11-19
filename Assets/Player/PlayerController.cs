using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Setup

    PlayerMovement moveScript;

    Vector3 movement;

    private void Start()
    {
        moveScript = GetComponent<PlayerMovement>();
    }

    #endregion

    #region Inputs

    // Update is called once per frame
    void Update()
    {
        if (moveScript != null)
        {
            if (Input.GetButton("Horizontal"))
            {
                //x
                movement.x = Input.GetAxisRaw("Horizontal");
            }
            else
            {
                movement.x = 0;
            }

            if (Input.GetButton("Vertical"))
            {
                //z
                movement.z = Input.GetAxisRaw("Vertical");
            }
            else
            {
                movement.z = 0;
            }

            if (Input.GetButton("Sprint"))
            {
                moveScript.SetSprint(true);
            }
            else
            {
                moveScript.SetSprint(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (moveScript != null)
        {
            moveScript.TryMove(movement.x, movement.z);
        }
    }

    #endregion
}