using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour 
{

    private bool wasJustClicked = true;
    private bool canMove;
    public Collider2D PlayerCollider { get; private set; }

    private Rigidbody2D rb;
    
    private Vector2 startingPosition;
    public Transform BoundaryHolder;

    private Boundary playerBoundary;
    
    public PlayerController Controller;
    public int? LockedFingerID { get; set; }
	void Start () {
        PlayerCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        startingPosition = rb.position;
        
        playerBoundary = new Boundary(BoundaryHolder.GetChild(0).position.y,
                                      BoundaryHolder.GetChild(1).position.y,
                                      BoundaryHolder.GetChild(2).position.x,
                                      BoundaryHolder.GetChild(3).position.x);

    }
    private void OnEnable()
    {
        Controller.Players.Add(this);
    }
    private void OnDisable()
    {
        Controller.Players.Remove(this);
    }
	/*void Update () {
		if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (wasJustClicked)
            {
                wasJustClicked = false;

                if (playerCollider.OverlapPoint(mousePos))
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }
            }

            if (canMove)
            {
                Vector2 clampedMousePos = new Vector2(
                    Mathf.Clamp(mousePos.x, playerBoundary.Left, playerBoundary.Right),
                    Mathf.Clamp(mousePos.y, playerBoundary.Down, playerBoundary.Up));
                
                rb.MovePosition(clampedMousePos);
            }
        }
        else
        {
            wasJustClicked = true;
        }
	}*/
    public void MoveToPosition(Vector2 position)
    {
        Vector2 clampedMousePos = new Vector2(Mathf.Clamp(position.x, playerBoundary.Left,
                playerBoundary.Right),
            Mathf.Clamp(position.y, playerBoundary.Down,
                playerBoundary.Up));
        rb.MovePosition(clampedMousePos);
    }
    public void ResetPosition()
    {
        rb.position = startingPosition;
    }
}

