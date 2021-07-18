using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuckScript : MonoBehaviour
{
    public ScoreManager ScoreScriptInstance;
    public AudioManager audioManager;
    public static bool WasGoal { get; private set; }
    private Rigidbody2D rb;

    public float MaxSpeed;
    
    void Start () 
    {
        rb = GetComponent<Rigidbody2D>();
        WasGoal = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!WasGoal)
        {
            if (other.CompareTag("AIGoal"))
            {
                ScoreScriptInstance.Increment(ScoreManager.Score.PlayerScore);
                WasGoal = true;
                audioManager.PlayGoal();
                StartCoroutine(ResetPuck(false));
            }
            else if (other.CompareTag("PlayerGoal"))
            {
                ScoreScriptInstance.Increment(ScoreManager.Score.AiScore);
                WasGoal = true;
                audioManager.PlayGoal();
                StartCoroutine(ResetPuck(true));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioManager.PlayPuckCollision();
    }

    private IEnumerator ResetPuck(bool didAiScore)
    {
        yield return new WaitForSecondsRealtime(1);
        WasGoal = false;
        rb.velocity = rb.position = new Vector2(0, 0);

        if (didAiScore)
            rb.position = new Vector2(0, -1);
        else
            rb.position = new Vector2(0, 1);
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);
    }
    
    public void CenterPuck()
    {
        rb.position = new Vector2(0, 0);
    }
}
