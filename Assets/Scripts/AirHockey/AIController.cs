using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public float maxMovementSpeed;
    private Rigidbody2D rb;
    private Vector2 startingPosition;

    public Rigidbody2D puck;

    public Transform aIBoundaryHolder;
    private Boundary aIBoundary;

    public Transform puckBoundaryHolder;
    private Boundary puckBoundary;

    private Vector2 targetPosition;

    private bool isFirstTimeInOpponentsHalf = true;
    private float offsetXFromTarget;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = rb.position;

        aIBoundary = new Boundary(aIBoundaryHolder.GetChild(0).position.y,
            aIBoundaryHolder.GetChild(1).position.y,
            aIBoundaryHolder.GetChild(2).position.x,
            aIBoundaryHolder.GetChild(3).position.x);

        puckBoundary = new Boundary(puckBoundaryHolder.GetChild(0).position.y,
            puckBoundaryHolder.GetChild(1).position.y,
            puckBoundaryHolder.GetChild(2).position.x,
            puckBoundaryHolder.GetChild(3).position.x);
    }

    private void FixedUpdate()
    {
        float movementSpeed;

        if (puck.position.y < puckBoundary.Down) //means in Player's half
        {
            if (isFirstTimeInOpponentsHalf)
            {
                isFirstTimeInOpponentsHalf = false;
                offsetXFromTarget = Random.Range(-1f, 1f);
            }

            movementSpeed = maxMovementSpeed * Random.Range(0.1f, 0.3f);
            targetPosition = new Vector2(
                Mathf.Clamp
                (puck.position.x + offsetXFromTarget,
                    aIBoundary.Left,
                    aIBoundary.Right),
                startingPosition.y
                );
        }
        else //means in AI's half
        {
            isFirstTimeInOpponentsHalf = true;

            movementSpeed = Random.Range(maxMovementSpeed * 0.4f, maxMovementSpeed);
            targetPosition = new Vector2(
                Mathf.Clamp
                (puck.position.x, aIBoundary.Left, aIBoundary.Right),
                Mathf.Clamp
                (puck.position.y, aIBoundary.Down, aIBoundary.Up));
        }

        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition,
            movementSpeed * Time.fixedDeltaTime));
    }
    
    public void ResetPosition()
    {
        rb.position = startingPosition;
    }
}
