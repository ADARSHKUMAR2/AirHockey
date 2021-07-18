using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSPlayerMovement : MonoBehaviour
{
    [SerializeField] private float  playerSpeed;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Move();
        }
    }

    private void Move()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalMove, 0f, verticalMove);
        transform.Translate(direction*playerSpeed*Time.deltaTime);
    }
}
