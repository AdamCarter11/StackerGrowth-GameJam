using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveSpeed = 2.0f;   // Speed of the movement
    public float moveDistance = 4.0f; // Distance to move in either direction

    private Vector3 startingPosition;
    private Vector3 leftPosition;
    private Vector3 rightPosition;

    private bool movingRight = true;

    private void Start()
    {
        startingPosition = transform.position;
        leftPosition = startingPosition + Vector3.left * moveDistance;
        rightPosition = startingPosition + Vector3.right * moveDistance;
    }

    private void Update()
    {
        if (movingRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, rightPosition, moveSpeed * Time.deltaTime);

            if (transform.position.x >= rightPosition.x)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, leftPosition, moveSpeed * Time.deltaTime);

            if (transform.position.x <= leftPosition.x)
            {
                movingRight = true;
            }
        }
    } 
}
