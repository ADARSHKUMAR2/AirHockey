using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip PuckCollision;
    [SerializeField] private  AudioClip Goal;
    [SerializeField] private  AudioClip LostGame;
    [SerializeField] private  AudioClip WonGame;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPuckCollision()
    {
        audioSource.PlayOneShot(PuckCollision);
    }

    public void PlayGoal()
    {
        audioSource.PlayOneShot(Goal);
    }
    
    public void PlayLostGame()
    {
        audioSource.PlayOneShot(LostGame);
    }

    public void PlayWonGame()
    {
        audioSource.PlayOneShot(WonGame);
    }
}
